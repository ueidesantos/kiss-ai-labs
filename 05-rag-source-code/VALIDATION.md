# Validation — RAG over Source Code

Run on 2026-10-10, America/Sao_Paulo.

- SDK: `11.0.100-rc.1.26425.128`; six repository projects compiled with zero warnings and zero errors.
- `pwsh -File 05-rag-source-code/tests/validate.ps1`: passed. It checks recursive `.cs`/`.CS` discovery, exclusion of `bin`, `obj`, `.git`, and Markdown, ranking, four embedding calls, one generation call, context and sources with line ranges, K above the total, CPU mode for both APIs, and 12 error cases. The test uses controlled vectors and does not assess model quality.
- Real demo: `nomic-embed-text:latest` (`0a109f422b47`) and `llama3.2:latest` (`a80c4f17acd5`), local endpoint, `--cpu`, K=2.

```bash
dotnet run --no-build --project 05-rag-source-code/src -- --question "Onde a regra de desconto é aplicada?" --top-k 2 --cpu
```

Retrieved `DiscountPolicy.cs` (0.632) and `OrderService.cs` (0.605). Response: “The discount rule is applied before calculating the order total, according to the CalculateTotal method in the OrderService class.” Exit code 0. The full output is in [demo-local.txt](demo-local.txt).

The model identified the call correctly but omitted citation IDs from its response. The application printed the sources separately; always check the files. There is no guarantee the model will follow the requested format.

## Environment Limitation

Attempts using the default run mode with `llama3.2` and `qwen2.5-coder:0.5b` aborted with `prediction aborted, token repeat limit reached`. The failure also occurred in a direct HTTP call outside the lab. A direct call with `num_gpu=0` and the complete demo with `--cpu` passed. This points to a difference between execution modes in this environment without establishing the cause of the failure. CPU mode is configurable; when omitted, Ollama uses its default behavior.

The current cover and all existing assets were preserved. The main README, lab guide, SPEC, and backlog were updated. `git diff --check` passed.
