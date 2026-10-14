# 05 — RAG over Source Code

![Capa original KISS AI Labs](../assets/kiss_ai_lab.png)

Find rules and classes in C# files using local embeddings and cosine similarity. The flow remains explicit: `.cs` files → embeddings → in-memory ranking → Top-K → answer with sources.

## Run

Prerequisites: .NET 11 SDK (preview with `net11.0` support), Ollama running, and models downloaded. Run from the repository root:

```bash
ollama pull nomic-embed-text
ollama pull llama3.2
dotnet build 05-rag-source-code/src/RagSourceCode.csproj
dotnet run --project 05-rag-source-code/src -- --question "Qual classe valida o limite de desconto de um pedido?" --top-k 2
dotnet run --project 05-rag-source-code/src -- --question "Onde a regra de desconto é aplicada?" --top-k 2
```

Review `DiscountPolicy.cs` (validates discounts between 0 and 20%) and `OrderService.cs` (calls the validation before calculating). `ShippingPolicy.cs` provides another topic for observing ranking noise. These files are sample data; RAG does not execute them.

To query your code:

```bash
dotnet run --project 05-rag-source-code/src -- --code ./meu-projeto --question "Qual classe faz essa validação?" --top-k 3
```

Options: `--code`, `--question`, `--top-k`, `--url`, `--embedding-model`, `--model`, `--cpu`, `--help`. Defaults: folder `05-rag-source-code/samples`, K=2, endpoint `http://localhost:11434`, models `nomic-embed-text` and `llama3.2`. K must be positive; values above the total retrieve all available files.

If `prediction aborted, token repeat limit reached` occurs, try `--cpu` to run embeddings and generation on the CPU. This mode resolved the failure observed in this environment; it may be slower. Example:

```bash
dotnet run --project 05-rag-source-code/src -- --question "Onde a regra de desconto é aplicada?" --top-k 2 --cpu
```

## What to Observe

The console prints retrieved files, cosine scores, full context, the answer, and `[S1]` sources with relative paths and ranges such as `file.cs:1-N`. Each entire file is a retrieval unit; the range identifies the full source, not the exact line containing the rule. Verify the call and validation in the original code. LLM-cited IDs are not validated automatically.

Search is recursive, accepts `.cs` and `.CS`, ignores `bin`, `obj`, `.git`, `.vs`, and `node_modules`, and does not follow file or directory links. It does not use Roslyn, a call graph, a vector database, persistence, chunking, or a similarity threshold. It may therefore retrieve irrelevant files or miss a call: similarity does not prove a relationship between classes.

## Data and Limitations

The full file text and question are sent to the endpoint for embeddings; retrieved files and the question are sent for generation. A local endpoint and local models keep this processing on the machine. With a remote endpoint, these texts go to the configured server. The context is also displayed in the terminal.

Embeddings are recalculated on every run. Large files may exceed the embedding window (truncation is disabled); choose a small folder. Generation is limited to 384 tokens and the model context window. A temperature of zero and a fixed seed do not guarantee accuracy or determinism.

A missing or empty folder, invalid options, and read failures return exit code 1 with a message. Connection refused or missing model: check Ollama, `--url`, and downloaded models. Use `--help` to see the options.

## Validation

```powershell
pwsh -File 05-rag-source-code/tests/validate.ps1
```

The HTTP test runs without models and checks recursive discovery, generated-folder exclusions, ranking, sources with line ranges, context, the K limit, and input errors. It does not measure semantic quality. See [VALIDATION.md](VALIDATION.md) and [SPEC.md](SPEC.md).
