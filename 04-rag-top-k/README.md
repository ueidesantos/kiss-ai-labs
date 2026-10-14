# 04 — RAG with Configurable Top-K

![Capa original KISS AI Labs](../assets/kiss_ai_lab.png)

How much context should you send to the model? Compare different K values using the same question, embeddings, and ranking. This lab builds on `--top-k` in [Local RAG](../03-rag-local-memory/README.md) by comparing values in a single run.

## Run

Prerequisites: .NET 11 SDK (including a preview SDK with `net11.0` support), Ollama running, and models downloaded. Run from the repository root:

```bash
ollama pull nomic-embed-text
ollama pull llama3.2
dotnet build 04-rag-top-k/src/RagTopK.csproj
dotnet run --project 04-rag-top-k/src -- --question "Como protegemos credenciais?" --compare 1,3,5
```

For a single run or your own documents:

```bash
dotnet run --project 04-rag-top-k/src -- --top-k 2
dotnet run --project 04-rag-top-k/src -- --docs ./meus-documentos --question "Qual é a política de retenção?" --compare 1,2,3
```

Options: `--docs`, `--question`, `--url`, `--embedding-model`, `--model`, `--top-k`, `--compare`, and `--help`. Defaults are K=3, `nomic-embed-text`, `llama3.2`, and `http://localhost:11434`. Use `--top-k` and `--compare` separately. K values must be positive integers; duplicates are ignored while preserving order.

## Experiment

```text
Markdown → embeddings uma vez → ranking único por cosseno
                                     │
                        ┌────────────┼────────────┐
                       K=1          K=3          K=5
                        │            │            │
                 contexto + pergunta → geração por rodada
                        │            │            │
                 arquivos, scores, contexto, resposta e fontes
```

The three sample files cover security, retention, and deployment. Each entire file is one chunk. The program prints the full context sent in each round and its size in UTF-16 characters; it does not estimate token counts.

1. Check that `seguranca.md` ranks first for the question about credentials.
2. Compare K=1 and K=3: did the additional information help or add noise? Check whether the sources support the answer.
3. K=5 retrieves the same three documents as K=3 because only three exist. The console distinguishes the requested K from the number retrieved.
4. Try a question that requires more than one source: “How do we protect credentials, and how long do we keep logs?”

More context can help with broad questions and hurt with specific ones. Answers and scores depend on the models. A temperature of zero and a fixed seed reduce variation but do not guarantee determinism or accuracy. Output is limited to 384 tokens per round and may end before the response is complete. There is no automatic quality evaluation or similarity threshold.

## Data and Common Failures

Vectors stay in memory and are recalculated on the next run. File contents go to the Ollama endpoint for embeddings; the question and retrieved documents go for generation. With a remote endpoint, the text leaves your machine. The console also displays retrieved content, so use data appropriate for this demo.

- Missing folder or no `.md` files: check `--docs`; run examples from the repository root.
- Missing model or refused connection: download the models and check the service and `--url`.
- Document too large: embeddings reject truncation; reduce file size. Generation is subject to the model context window.
- Invalid K, option without a value, or incompatible options: see `--help`; the run returns exit code 1.
- Inaccurate citation: compare `[S1]` and other IDs with the list and files. The program does not validate LLM claims.

## Validation

Integration test without models, using PowerShell 7 from the repository root:

```powershell
pwsh -File 04-rag-top-k/tests/validate.ps1
```

The test uses a temporary loopback HTTP server to check ranking, context, call counts, K above the total, and input errors. It does not assess semantic quality. See [VALIDATION.md](VALIDATION.md) for results with real Ollama and [SPEC.md](SPEC.md) for the criteria.
