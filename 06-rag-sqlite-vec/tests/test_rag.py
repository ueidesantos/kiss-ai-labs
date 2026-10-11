"""Offline tests; persistence/ranking tests require the real sqlite-vec extension."""
import contextlib
import importlib.util
import io
import json
from pathlib import Path
import sqlite3
import struct
import sys
import tempfile
import unittest
from unittest.mock import patch
from urllib.error import HTTPError

sys.path.insert(0, str(Path(__file__).resolve().parents[1] / "src"))
import rag

HAS_VEC = importlib.util.find_spec("sqlite_vec") is not None
TEMP_ROOT = Path(__file__).resolve().parent


class InputTests(unittest.TestCase):
    def test_float32_and_dimensions(self):
        self.assertEqual(struct.unpack("<2f", rag.vector_blob([1, 0], 2)), (1, 0))
        with self.assertRaises(ValueError):
            rag.vector_blob([1], 2)

    def test_bad_vectors(self):
        for vector in ([], None, [0, 0], [True, 0], [float("nan")], [float("inf")], ["1"], [1e100], [10**400], [1e-100]):
            with self.subTest(vector=vector), self.assertRaises(ValueError):
                rag.vector_blob(vector)

    def test_cli_errors(self):
        cases = [["ask", "--top-k", "0"], ["ask", "--top-k", "-1"],
                 ["ask", "--top-k", "abc"], ["ask", "--question", " "],
                 ["ask", "--url", "invalid"], ["ask", "--url", "http://user:secret@localhost"],
                 ["ask", "--url", "http://localhost?x=1"], ["index", "--model", "bad"], []]
        for args in cases:
            with self.subTest(args=args), contextlib.redirect_stderr(io.StringIO()), self.assertRaises(SystemExit) as result:
                rag.parser().parse_args(args)
            self.assertEqual(result.exception.code, 2)

    def test_missing_database_does_not_create_it(self):
        with tempfile.TemporaryDirectory(dir=TEMP_ROOT) as temp:
            path = Path(temp) / "missing.sqlite3"
            with self.assertRaises(ValueError):
                rag.open_db(path, existing=True)
            self.assertFalse(path.exists())

    def test_empty_index_is_rejected_before_embedding(self):
        with tempfile.TemporaryDirectory(dir=TEMP_ROOT) as temp, patch.object(rag, "embed") as embed:
            (Path(temp) / "empty.md").write_text(" ", encoding="utf-8")
            args = rag.parser().parse_args(["index", "--docs", temp, "--db", str(Path(temp) / "db")])
            with self.assertRaises(ValueError):
                rag.index(args)
            embed.assert_not_called()

    def test_http_request_contract(self):
        with patch.object(rag, "urlopen", return_value=io.BytesIO(b'{"response":"ok","done":true}')) as call:
            result = rag.post_json("http://localhost:11434/", "/api/generate", {"prompt": "olá"})
        request = call.call_args.args[0]
        self.assertEqual(request.full_url, "http://localhost:11434/api/generate")
        self.assertEqual(json.loads(request.data), {"prompt": "olá"})
        self.assertEqual(request.method, "POST")
        self.assertTrue(result["done"])

    def test_http_error_body(self):
        error = HTTPError("http://localhost", 404, "missing model", {}, io.BytesIO(b'model not found'))
        with patch.object(rag, "urlopen", side_effect=error), self.assertRaisesRegex(ValueError, "404.*model not found"):
            rag.post_json("http://localhost", "/api/embed", {})

    def test_api_error_and_invalid_json_shape(self):
        for body in (b'{"error":"model missing"}', b'[]'):
            with patch.object(rag, "urlopen", return_value=io.BytesIO(body)), self.assertRaises(ValueError):
                rag.post_json("http://localhost", "/api/embed", {})

    def test_embed_contract_and_cpu(self):
        with patch.object(rag, "post_json", return_value={"embeddings": [[1, 0]]}) as post:
            self.assertEqual(rag.embed("http://localhost", "embed-model", "text", True), [1, 0])
        payload = post.call_args.args[2]
        self.assertFalse(payload["truncate"])
        self.assertEqual(payload["options"], {"num_gpu": 0})

    def test_embed_must_return_one_vector(self):
        for vectors in ([], [[1], [1]], None, [[0]]):
            with patch.object(rag, "post_json", return_value={"embeddings": vectors}), self.assertRaises(ValueError):
                rag.embed("http://localhost", "model", "text", False)


@unittest.skipUnless(HAS_VEC, "sqlite-vec não instalado; validação nativa pendente")
class PersistenceTests(unittest.TestCase):
    def setUp(self):
        self.temp = tempfile.TemporaryDirectory(dir=TEMP_ROOT)
        self.addCleanup(self.temp.cleanup)
        self.root = Path(self.temp.name)
        self.docs = self.root / "docs"
        self.docs.mkdir()
        (self.docs / "a.md").write_text("deployment", encoding="utf-8")
        (self.docs / "nested").mkdir()
        (self.docs / "nested" / "b.MD").write_text("credentials", encoding="utf-8")
        (self.docs / "notes.txt").write_text("ignored", encoding="utf-8")
        self.db = self.root / "rag.sqlite3"
        self.calls = []
        self.mock = patch.object(rag, "post_json", side_effect=self.ollama)
        self.mock.start()
        self.addCleanup(self.mock.stop)

    def ollama(self, url, route, payload):
        self.calls.append((route, payload))
        if route == "/api/embed":
            return {"embeddings": [[1, 0] if "credentials" in payload["input"] else [0, 1]]}
        return {"response": "Use environment variables [S1].", "done": True}

    def build(self):
        args = rag.parser().parse_args(["index", "--docs", str(self.docs), "--db", str(self.db), "--cpu"])
        with contextlib.redirect_stdout(io.StringIO()):
            rag.index(args)
        return args

    def query(self, *extra):
        args = rag.parser().parse_args(["ask", "--db", str(self.db), "--question", "credentials", "--cpu", *extra])
        output = io.StringIO()
        with contextlib.redirect_stdout(output):
            rag.ask(args)
        return output.getvalue()

    def test_native_rank_persistence_and_context(self):
        self.build()
        # Original files disappear; query must still use the saved text and vectors.
        (self.docs / "nested" / "b.MD").unlink()
        self.calls.clear()
        output = self.query("--top-k", "1")
        self.assertIn("[S1] nested/b.MD", output)
        self.assertIn("distância de cosseno: 0.000", output)
        self.assertEqual([route for route, _ in self.calls], ["/api/embed", "/api/generate"])
        prompt = self.calls[-1][1]["prompt"]
        self.assertIn("credentials", prompt)
        self.assertNotIn("deployment", prompt)
        for _, payload in self.calls:
            self.assertEqual(payload["options"]["num_gpu"], 0)

    def test_k_above_total(self):
        self.build()
        self.assertIn("arquivos recuperados: 2", self.query("--top-k", "5"))

    def test_reindex_removes_old_rows(self):
        self.build()
        (self.docs / "a.md").unlink()
        self.build()
        self.assertIn("arquivos recuperados: 1", self.query("--top-k", "5"))

    def test_failed_embedding_keeps_previous_index(self):
        args = self.build()
        with patch.object(rag, "embed", side_effect=ValueError("model unavailable")), self.assertRaises(ValueError):
            rag.index(args)
        self.assertIn("arquivos recuperados: 2", self.query("--top-k", "5"))

    def test_transaction_rolls_back_failed_insert(self):
        args = self.build()
        # Force a native CHECK failure after DROP/CREATE to verify DDL rollback too.
        with patch.object(rag, "vector_blob", return_value=struct.pack("<f", 1)), self.assertRaises(sqlite3.Error):
            rag.index(args)
        self.assertIn("arquivos recuperados: 2", self.query("--top-k", "5"))

    def test_endpoint_and_dimensions_mismatch(self):
        self.build()
        with self.assertRaisesRegex(ValueError, "Endpoint diferente"):
            self.query("--url", "http://other-host")
        with patch.object(rag, "embed", return_value=[1, 0, 0]), self.assertRaisesRegex(ValueError, "Dimensões"):
            self.query()

    def test_foreign_database_is_preserved(self):
        with sqlite3.connect(self.db) as db:
            db.execute("CREATE TABLE important (text)")
            db.execute("INSERT INTO important VALUES ('keep')")
        with self.assertRaisesRegex(ValueError, "outro projeto"):
            self.build()
        with sqlite3.connect(self.db) as db:
            self.assertEqual(db.execute("SELECT text FROM important").fetchone()[0], "keep")

    def test_invalid_generation_fails(self):
        self.build()
        for result in ({"response": "", "done": True}, {"response": "partial", "done": False}):
            with patch.object(rag, "post_json", side_effect=[{"embeddings": [[1, 0]]}, result]), self.assertRaises(ValueError):
                self.query()


if __name__ == "__main__":
    suite = unittest.defaultTestLoader.loadTestsFromModule(sys.modules[__name__])
    result = unittest.TextTestRunner(verbosity=2).run(suite)
    if result.skipped:
        print("INCOMPLETO: instale requirements.txt e execute novamente para validar SQLite-vec nativo.", file=sys.stderr)
    sys.exit(0 if result.wasSuccessful() and not result.skipped else 1)
