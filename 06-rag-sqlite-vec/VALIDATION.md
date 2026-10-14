# Validation — Local RAG with SQLite-vec

Run on 2026-10-11, America/Sao_Paulo. **Full validation and publication are pending.**

## Completed

- `py_compile` succeeded for the application and test suite. Local links, UTF-8, PNG signature/dimensions (1671x941), and `git diff --check` were validated; the cover was visually reviewed.
- Ten input and HTTP contract tests passed: float32 vectors, dimensions, invalid values, arguments, missing database, empty folder, UTF-8 payload, HTTP error, and embedding/CPU options.
- Eight tests requiring real SQLite-vec were skipped because the extension is not installed. The suite explicitly returns exit code 1 in this case; it is not considered fully passed.
- Implementation checked against the official documentation: [Python and extension loading](https://alexgarcia.xyz/sqlite-vec/python.html), [KNN with scalar functions and cosine distance](https://alexgarcia.xyz/sqlite-vec/features/knn.html).

## Environment Blockers

- `pip install sqlite-vec==0.1.9` could not retrieve the distribution in this network-restricted environment. The version is listed on the [official PyPI page](https://pypi.org/project/sqlite-vec/0.1.9/), but it was not installed or validated here.
- `ollama list` could not access `127.0.0.1:11434`: the socket was blocked by permissions. The real semantic demo was not run; no model output was fabricated.
- GitHub fetch failed with `Could not resolve host: github.com`. The work will be committed locally; pushing depends on GitHub access and full validation.

## Resume Validation

```bash
python -m pip install -r 06-rag-sqlite-vec/requirements.txt
python -m py_compile 06-rag-sqlite-vec/src/rag.py 06-rag-sqlite-vec/tests/test_rag.py
python 06-rag-sqlite-vec/tests/test_rag.py
python 06-rag-sqlite-vec/src/rag.py index --cpu
python 06-rag-sqlite-vec/src/rag.py ask --question "Como protegemos credenciais?" --top-k 1 --cpu
```

Record the actual output and update this file and the backlog status after the full suite passes. Before pushing, sync `main` and check the daily history to preserve the limit of one new lab per São Paulo date.
