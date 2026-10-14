# Run and verify

Prerequisites: .NET 11 SDK and Git (for generating the diff). Run commands from the repository root. See the [README](../../09-ai-squad/README.md) for an overview of the workflow.

## Reproducible demo

```powershell
dotnet run --project 09-ai-squad/runner -- --offline --out 09-ai-squad/runs/demo
$env:SQUAD_RUN = (Resolve-Path 09-ai-squad/runs/demo).Path
dotnet run --project 09-ai-squad/app
```

Open http://127.0.0.1:8089. `SQUAD_RUN` accepts a directory or `report.json`. Use a new output folder for each run. Standard 100/10% returns 90; Standard 100/15% is rejected; VIP 100/15% returns 85.

Offline proposals and reviews are prepared. The initial 20% proposal for Standard is rejected by real tests; the 10% correction is tested and reviewed again. Without `SQUAD_RUN`, the reference simulator works and the dashboard reports that evidence is missing.

## Two local models

With Ollama on localhost:11434 and two models installed:

```powershell
dotnet run --project 09-ai-squad/runner -- --model DEVELOPER_MODEL --review-model REVIEWER_MODEL --out 09-ai-squad/runs/real
```

Replace the identifiers. Names and digests must differ; aliases for the same model are rejected. Real mode does not replay offline results. It is an Ollama integration, not an adapter for the Claude or Codex APIs. The optional `--lab` parameter identifies the lab directory.

## Inspection and optional publication

Read `report.json`, `changes.diff`, `build.log`, `PR.md`, and `candidate`. Check sources, models, reviews, tests, and final status. The gate stays closed unless tests pass and `high`/`critical` findings are resolved. The one allowed correction always requires a new review.

With Git, Python, and authenticated `gh`:

```powershell
python 09-ai-squad/scripts/publish.py --run 09-ai-squad/runs/demo --repo YOUR_USER/YOUR_REPOSITORY
```

The script checks the gate/manifest, creates a temporary clone, publishes a branch, and opens a draft PR. It never merges automatically. A PR URL counts as evidence only when returned by a completed publication.

[Convergence](convergence.md) distinguishes prepared integration tests from real calls to the models.
