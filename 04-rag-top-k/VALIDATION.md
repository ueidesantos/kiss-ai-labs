# Validation — RAG with Configurable Top-K

Run on 2026-10-09, on Windows, with .NET SDK `11.0.100-rc.1.26425.128` and Ollama `0.40.2`.

## Build and Integration

- All five repository projects compiled with zero warnings and zero errors.
- `pwsh -File 04-rag-top-k/tests/validate.ps1` passed.
- The test checked cosine ranking, source IDs, and that the printed context matches the context sent for generation.
- Three documents and the question produced four embedding calls, reused across rounds; three distinct K values produced three generations.
- The list `3,1,5,1` preserved order, removed duplicates, and retrieved 3, 1, and 3 documents.
- Twelve error cases passed: invalid values, incompatible or unknown options, missing values, repeated options, invalid URLs, missing folders, and empty folders.
- The cover and other files in `assets/` were not changed.

## Demo with Local Ollama

```bash
dotnet run --no-build --project 04-rag-top-k/src -- --compare 1,3,5 --question "Como protegemos credenciais?"
```

Embedding: `nomic-embed-text`; generation: `llama3.2`; temperature 0, seed 42, 384-token output limit. Full output from this run is in [demo-local.txt](demo-local.txt).

| K solicitado | Recuperados | Contexto em caracteres UTF-16 |
|---:|---:|---:|
| 1 | 1 | 411 |
| 3 | 3 | 1244 |
| 5 | 3 | 1244 |

The observed ranking was `seguranca.md` (0.695), `implantacao.md` (0.589), `retencao.md` (0.583). K=3 and K=5 sent the same context; the two additional documents do not directly address credential security. Sizes include the files’ line endings in the Windows checkout.

With the default model, K=1 answered that credentials should be kept in environment variables or a secrets manager. K=3 and K=5 produced the same answer, citing `[S1]`; K=1 did not include the ID, underscoring the need to check the source list.

An earlier attempt with `qwen2.5:0.5b` was aborted by Ollama due to token repetition. With `llama3.2:1b`, the run completed, but the model said it could not find the information even though the correct source was retrieved. This shows that correct retrieval does not guarantee a correct answer. Evaluate quality by checking the documents; a completed run is not proof of accuracy.
