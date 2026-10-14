# Validation — 2026-10-12

Evidence consolidated by the coordinator and backend/frontend agents. The results below distinguish deterministic execution, prepared transport, and real integration.

| Check | Observed result | Scope |
|---|---|---|
| Runner and application build | Passed | .NET 11 compilation |
| Offline demo | 13/17 initial tests; 17/17 after one correction | Prepared policy 20/20 → 10/20; real domain tests |
| Offline review | Two prepared reviews | No AI calls; initial finding and new review after correction |
| Candidate build | Two successful compilations | Application materialized before each review |
| Prepared Ollama transport | All script scenarios passed | HTTP/JSON contracts and gates; no real inference |
| Adversarial gate | Persistent severe finding ends in rejection | Cannot release after the one allowed correction |
| Identities | Alias with same digest rejected | Two names are not enough |
| Failures | Invalid JSON and failed build block | No silent fallback |
| Integrity/publication | Artifact modification rejected | Local manifest verification |
| Existing output | Overwrite rejected | Isolation between runs |
| Browser/API | Standard 10%=90, Standard 15% rejected, VIP 15%=85 for amount 100 | Functional app flow |
| Mobile | No overflow or JavaScript errors observed at 390px | Playwright interface verification |
| Review content | Injection payload displayed as literal text | Interface does not interpret findings as HTML |

## Reproduce the main checks

From the repository root, with .NET 11, Git, and Python available:

```powershell
dotnet build 09-ai-squad/runner --nologo -m:1 -p:UseSharedCompilation=false
dotnet build 09-ai-squad/app --nologo -m:1 -p:UseSharedCompilation=false
dotnet run --project 09-ai-squad/runner -- --offline --out 09-ai-squad/runs/verification
python 09-ai-squad/scripts/test_runner.py
python 09-ai-squad/scripts/publish.py --run 09-ai-squad/runs/verification --repo YOUR_USER/YOUR_REPOSITORY --check-only
```

Use a new run folder. `test_runner.py` starts a prepared HTTP server and covers correction and new review, adversarial blocking, alias, invalid JSON, failed build, hash tampering, and existing-folder rejection. Runner tests verify policy using the trusted domain; candidate compilation is real. HTTP/browser validation was performed separately.

## Verified limitations

- Ollama on localhost:11434 was unavailable. No inference with two real models was performed; no Claude/Codex validation is claimed.
- GitHub CLI was unavailable. No demo draft PR was created; local publication gates were checked without publishing.
- Deterministic approval and the prepared server do not demonstrate the quality of a real LLM review.
- Publishing the lab to `main` is a coordinator step, separate from this evidence. This record does not claim that a commit, push, or deployment was completed.
- Inspection of the new cover and site publication must be confirmed separately by the coordinator.
