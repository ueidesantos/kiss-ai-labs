"""Integration tests with a fake Ollama transport; no claim of real AI validation."""
import http.server
import json
import pathlib
import subprocess
import tempfile
import threading
import importlib.util
import shutil

LAB = pathlib.Path(__file__).resolve().parents[1]
DLL = LAB / "runner/bin/Debug/net11.0/Squad.Runner.dll"


class Handler(http.server.BaseHTTPRequestHandler):
    scenario = "pass"
    calls = 0

    def log_message(self, *args):
        pass

    def reply(self, body):
        payload = json.dumps(body).encode()
        self.send_response(200)
        self.send_header("Content-Length", str(len(payload)))
        self.end_headers()
        self.wfile.write(payload)

    def do_GET(self):
        self.reply({"models": [{"name": "dev:latest", "digest": "abc"}, {"name": "review:latest", "digest": "abc" if self.scenario == "alias" else "def"}]})

    def do_POST(self):
        if self.headers.get("Transfer-Encoding") == "chunked":
            chunks = []
            while True:
                size = int(self.rfile.readline().strip(), 16)
                if size == 0:
                    self.rfile.readline()
                    break
                chunks.append(self.rfile.read(size))
                self.rfile.read(2)
            raw = b"".join(chunks)
        else:
            raw = self.rfile.read(int(self.headers["Content-Length"]))
        request = json.loads(raw)
        Handler.calls += 1
        n = Handler.calls
        if n == 1:
            answer = {"plan": "Front textos, Back política, QA independente."}
        elif n == 2:
            answer = {"title": "Simular", "help": "Informe valores", "standardLabel": "Padrão", "vipLabel": "VIP"}
        elif request["model"] == "review":
            answer = {"summary": "Teste de transporte, não IA", "findings": [{"id": "R1", "severity": "high", "evidence": "policy incorreta", "fix": "limite10"}] if self.scenario == "blocked" or n == 4 else []}
        else:
            answer = {"standardMax": 20 if n == 3 else 10, "vipMax": 20}
        self.reply({"response": "invalid JSON" if self.scenario == "malformed" else json.dumps(answer), "eval_count": 2, "prompt_eval_count": 3})


def main():
    subprocess.run(["dotnet", "build", str(LAB / "runner/Squad.Runner.csproj"), "--nologo", "-m:1", "-p:UseSharedCompilation=false"], check=True, capture_output=True, timeout=120)
    server = http.server.ThreadingHTTPServer(("127.0.0.1", 0), Handler)
    threading.Thread(target=server.serve_forever, daemon=True).start()
    try:
        with tempfile.TemporaryDirectory(prefix="squad-test-") as temp:
            for scenario, expected in [("pass", 0), ("blocked", 1), ("alias", 1), ("malformed", 1), ("build-failed", 1)]:
                Handler.scenario, Handler.calls = scenario, 0
                folder = pathlib.Path(temp) / scenario
                args = ["dotnet", str(DLL), "--model", "dev", "--review-model", "review", "--endpoint", f"http://127.0.0.1:{server.server_port}", "--out", str(folder)]
                if scenario == "build-failed":
                    broken = pathlib.Path(temp) / "broken" / "lab"
                    for name in ("app", "core", "business"):
                        shutil.copytree(LAB / name, broken / name, ignore=shutil.ignore_patterns("bin", "obj"))
                    shutil.copyfile(LAB / "DEMAND.md", broken / "DEMAND.md")
                    specs = broken.parent / "specs/lab-09-ai-squad"
                    specs.mkdir(parents=True)
                    shutil.copyfile(LAB.parent / "specs/lab-09-ai-squad/SPEC.md", specs / "SPEC.md")
                    (broken / "app/Program.cs").write_text("this is invalid C#")
                    args.extend(["--lab", str(broken)])
                result = subprocess.run(args, capture_output=True, text=True, timeout=120)
                assert result.returncode == expected, result.stdout + result.stderr
                report = json.loads((folder / "report.json").read_text())
                assert report["mode"] == "ollama", "Silent offline fallback"
                if scenario == "pass":
                    assert report["corrections"] == 1 and len(report["reviews"]) == 2 and report["modelCalls"] == 6
                    assert all(t["passed"] for t in report["tests"]) and report["buildPassed"]
                    module_spec = importlib.util.spec_from_file_location("publish", LAB / "scripts/publish.py")
                    module = importlib.util.module_from_spec(module_spec)
                    module_spec.loader.exec_module(module)
                    module.verify(folder)
                    (folder / "policy.json").write_text("{}")
                    try:
                        module.verify(folder)
                        raise AssertionError("Tampered policy accepted")
                    except ValueError:
                        pass
                    duplicate = subprocess.run(args, capture_output=True, timeout=120)
                    assert duplicate.returncode == 1
                else:
                    assert report["status"] == "failed"
                    assert not (folder / "manifest.json").exists()
                    if scenario == "build-failed":
                        assert not report["buildPassed"] and not report["reviews"]
                print("PASS", scenario)
    finally:
        server.shutdown()
    print("PASS publisher integrity and output overwrite prevention")


if __name__ == "__main__":
    main()
