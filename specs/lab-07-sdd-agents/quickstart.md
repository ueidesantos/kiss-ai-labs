# Run Lab 07

Prerequisites: .NET 11 SDK and Node.js for frontend tests. Ollama and a local model are needed only for real generation. No Spec Kit CLI or external NuGet packages need to be installed.

Run these commands from the repository root:

```powershell
dotnet build 07-sdd-agents-business-rag/src/SddAgents.csproj
dotnet build 07-sdd-agents-business-rag/app/DiscountApp.csproj
dotnet build 07-sdd-agents-business-rag/tests/LabTests.csproj
dotnet run --project 07-sdd-agents-business-rag/tests
node --test 07-sdd-agents-business-rag/tests/frontend.test.cjs
```

The coordinator reported: three builds without warnings, 66 .NET checks, and nine JS tests passed. Validation uses in-memory/simulated transport; it does not verify a real network.

## Replay without a model

```powershell
dotnet run --project 07-sdd-agents-business-rag/src -- --offline
```

Read the artifact folder shown in the terminal: context, candidate SPEC, tasks, proposals, and review. The mode declares `REPLAY OFFLINE`; content uses prepared responses, not AI. To choose an output folder, add `--out <empty-folder>`; the workflow rejects overwriting a nonempty folder.

## Reference application

```powershell
dotnet run --project 07-sdd-agents-business-rag/app
```

Open `http://127.0.0.1:8087`. Simulate R$100 at 10%: total R$90.00. You can also call the endpoint:

```powershell
Invoke-RestMethod -Uri http://127.0.0.1:8087/api/discount -Method Post -ContentType application/json -Body '{"amount":100,"discount":10}'
```

Real HTTP and visual inspection were not validated in this run due to environment restrictions. The command is a recipe for completing that evidence.

## Real generation

With Ollama available at `http://localhost:11434` and `llama3.2` installed:

```powershell
dotnet run --project 07-sdd-agents-business-rag/src -- --model llama3.2
```

Use `--cpu` to generate on the CPU; `--url`, `--top-k`, `--lab`, and `--out` adjust the endpoint, retrieval, and paths. `--help` lists options. The workflow calls TL, Front, and Back sequentially. Review citations and proposals before incorporating them. Real Ollama was not validated in this run; candidate artifacts do not approve business-rule changes.
