# Lab 09 — KISS AI Squad

**One AI builds. Another looks for what slipped through.** A small business-rule change goes through SDD, RAG, Front/Back proposals, tests, and adversarial review by a second model.

The request: Standard customers can simulate discounts up to 10%; VIP customers up to 20%. The focus is on a verifiable delivery: sources, a candidate SPEC, an application, a diff, findings, and tests. An AI's approval cannot override a failed test or a critical finding.

## Try it without a model

From the repository root, with the **.NET 11 SDK** and **Git** (required to generate the diff):

```powershell
dotnet run --project 09-ai-squad/runner -- --offline --out 09-ai-squad/runs/demo
$env:SQUAD_RUN = (Resolve-Path 09-ai-squad/runs/demo).Path
dotnet run --project 09-ai-squad/app
```

Open **http://127.0.0.1:8089**. Use a new output folder for each run. The simulator works without a report; `SQUAD_RUN` connects the dashboard to the evidence.

Offline mode uses **prepared proposals and reviews, with no AI**. Back starts with Standard=20%; real tests reject the rule; the prepared 10% correction goes through verification again. This is not a model's spontaneous discovery.

## Separate developer and reviewer

With Ollama running and two different models installed, replace the names below with yours:

```powershell
dotnet run --project 09-ai-squad/runner -- --model DEVELOPER_MODEL --review-model REVIEWER_MODEL --out 09-ai-squad/runs/real
```

The runner checks model names and digests in Ollama: two aliases for the same installation do not satisfy the contract. A missing model, invalid response, or connection failure ends the run without falling back to the demo.

**“Claude builds, Codex reviews” is a conceptual example.** This lab's code integrates two models through Ollama; it does not implement the Claude or Codex APIs. Distinct identities reduce the chance of repeating the same review, but do not guarantee independence or correctness.

| Role | Responsibility |
|---|---|
| TL · developer model | Uses retrieved context to plan the change |
| Front · developer model | Proposes text through a JSON contract |
| Back · developer model | Proposes policy limits through a JSON contract |
| Adversarial QA · reviewer model | Looks for defects, records evidence/severity/fix, and reviews the new candidate |
| Deterministic tests | Verify the rule without relying on the models' opinions |

RAG is lexical, using TF-IDF and cosine similarity reused from Lab 07, and records paths, excerpts, and SHA256. Controlled templates materialize the application: agents generate structured data, **not arbitrary executable code**.

## How a delivery is approved

1. Materialize the proposal and run the build/tests.
2. Request adversarial review from the second model.
3. If tests fail or a `high`/`critical` finding appears, allow one Back correction.
4. Run the build/tests and review again. If the blocker remains, reject the change.
5. Only an approved run can proceed to optional publication.

Artifacts include `report.json`, `policy.json`, `front.json`, `plan.md`, `SPEC.md`, `changes.diff`, `build.log`, `PR.md`, `manifest.json`, and the `candidate` folder. The report and reviews preserve the run history. `SPEC.md` is a snapshot of the canonical specification; the TL's plan is in `plan.md`. The runner compiles the candidate and tests the policy against the trusted domain; HTTP and browser tests are recorded separately.

## Optional draft PR

With Git, Python, and an authenticated GitHub CLI (`gh`):

```powershell
python 09-ai-squad/scripts/publish.py --run 09-ai-squad/runs/demo --repo YOUR_USER/YOUR_REPOSITORY
```

To check the gate and hashes without publishing, add `--check-only` to the command. This mode does not require GitHub authentication.

Review the artifacts first. The script checks approval and integrity, uses a temporary clone, publishes a branch, and opens a **draft PR**. It does not merge. A demo is also labeled as prepared in the delivery. Publishing this lab to `main` does not mean a demo PR was created.

## SDD kit and evidence

- [Request](DEMAND.md), [canonical SPEC](../specs/lab-09-ai-squad/SPEC.md), and [kit overview](SPECKIT.md).
- [Plan](../specs/lab-09-ai-squad/plan.md), [tasks](../specs/lab-09-ai-squad/tasks.md), [quickstart](../specs/lab-09-ai-squad/quickstart.md), and [checklist](../specs/lab-09-ai-squad/checklists/requirements.md).
- [Validation performed](../specs/lab-09-ai-squad/VALIDATION.md), distinguishing the demo, prepared transport, and real integration.
- [Rules](business/descontos.md), [calculation](business/simulacao.md), and [convergence/limitations](../specs/lab-09-ai-squad/convergence.md).
- [Lab 08 — Figma as a visual specification](../08-sdd-figma-mcp/README.md), a complementary reference; this lab has no Figma capture.

Fictional data. Selecting VIP does not authenticate customers, process payments, or grant commercial authorization.
