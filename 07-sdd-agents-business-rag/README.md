# 07 — SDD + agents + business RAG

![Lab 07 flow](assets/project-flow.png)

**KISS request:** simulate the final order amount with a discount from 0 to 20%. Stack: .NET 11, ASP.NET Core, C#, HTML/CSS/JS, and optional Ollama. No external packages.

The TL retrieves business documentation with lexical RAG (TF-IDF + cosine similarity), prepares a candidate SPEC, and delegates bounded tasks to Front Dev and Back Dev. Each developer receives the SPEC, contract, sources, and only their assigned task. Proposals are saved for review; they are not run automatically.

The reference application was implemented by separate Front/Back agents for this delivery and independently reviewed. It runs without Ollama.

## Run the application

From the repository root, with the .NET 11 SDK:

```bash
dotnet run --project 07-sdd-agents-business-rag/app
```

Open `http://127.0.0.1:8087`. Enter 100 and 10; the expected result is R$90.00. A 21% discount should show an error. The simulation does not save orders or charge amounts.

Contract: `POST /api/discount`, JSON `{ "amount": 100, "discount": 10 }`; success `{ "original": "100.00", "discount": "10", "total": "90.00" }`; HTTP 400 error `{ "error": "message" }`.

The backend uses decimal arithmetic and HALF_UP rounding (`AwayFromZero`). The frontend preserves money as text when displaying the response; if converting an input to a JSON number would change its decimal value, it rejects the input before calling the API. The API JSON body is limited to 4096 bytes.

## Run TL + Front + Back

Without models, to study the workflow:

```bash
dotnet run --project 07-sdd-agents-business-rag/src -- --offline
```

**Offline mode replays prepared responses; it is not AI generation.** It uses the reference code as a proposal. With Ollama running:

```bash
ollama pull llama3.2
dotnet run --project 07-sdd-agents-business-rag/src -- --model llama3.2 --cpu
```

Real mode makes three `/api/generate` calls: TL, Front, and Back, with JSON responses and validation of contracts, roles, and references. `--cpu` sends `num_gpu=0`. Zero temperature and a fixed seed do not guarantee accuracy; rule fidelity and citations require review.

Options: `--offline`, `--cpu`, `--lab`, `--out`, `--model`, `--url`, `--top-k`, `--help`. Default K is 3. The lab folder is discovered from the executable. Output goes to `runs/<UTC timestamp>/`, ignored by Git. An existing nonempty `--out` folder is rejected.

## Complete request kit

Read [SPECKIT.md](SPECKIT.md) for an explanation of the cycle. The delivery includes the constitution, request, business sources, SPEC, clarifications, research, plan, data model, contract, tasks, checklist, analysis, and convergence. This kit was authored for this request and follows the process of the [official Spec Kit](https://github.com/github/spec-kit); its CLI was not installed or run in this session.

Source of truth: [canonical SPEC](../specs/lab-07-sdd-agents/SPEC.md). The model-emitted SPEC is a candidate and does not replace the canonical one without review. Sources → requirements → tasks → code → evidence remain distinct.

Each run writes `sources.json`, `plan.json`, `SPEC.candidate.md`, `tasks.md`, `handoffs.json`, proposals `.html`/`.cs`, and `review.json`. The latter declares `generatedCodeExecuted=false` and `needs-human-review`.

## Validate

```bash
dotnet build 07-sdd-agents-business-rag/tests/LabTests.csproj
dotnet run --project 07-sdd-agents-business-rag/tests
node --test 07-sdd-agents-business-rag/tests/frontend.test.cjs
```

The .NET tests check calculation/native in-memory HTTP handler, ranking, sources, delegation, rejections, and replay. JS tests use simulated DOM/fetch; Node is only required for that validation. [VALIDATION.md](VALIDATION.md) records what ran and what still depends on network/browser access.

## Data and limitations

RAG runs in memory, with whole Markdown files and lexical search; it uses neither embeddings nor a database. In Ollama mode, the request, retrieved documents, and specification are sent to the configured endpoint. Proposals/artifacts are also written to disk. A remote endpoint receives this text.

The lab does not run generated code or resolve business conflicts on its own. Valid references do not prove semantic fidelity. The application server and visual review require local access; Ollama requires models. Generation timeout is 3 minutes and the limit is 4096 tokens per call. [Request](DEMAND.md) · [SPEC](SPEC.md) · [Kit](SPECKIT.md).
