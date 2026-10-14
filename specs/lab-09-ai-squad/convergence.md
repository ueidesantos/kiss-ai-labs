# Convergence and evidence

Scope updated on 2026-10-12: two distinct Ollama models, blocking adversarial review, and Lab 09 featured on the main cover. Detailed evidence is in [VALIDATION.md](VALIDATION.md).

| Layer | Proven status |
|---|---|
| Domain | Demo: 13/17 initially → 17/17 after one correction |
| Offline runner | One correction, two prepared reviews, candidate compiled twice |
| Integration contracts | Prepared HTTP server: approval, severe blocker, alias, invalid JSON, and build failure verified |
| Local publication | Tampered manifest and overwrite rejected; local gate verified |
| Interface | Standard/VIP simulations, 390px mobile, and finding rendered as literal text verified |
| Real models | Not exercised: local Ollama unavailable |
| Draft PR | Not created: GitHub CLI unavailable; do not confuse local gate with publication |
| Cover/site/commit | Final confirmation by the coordinator, outside execution tests |

The run's SPEC is a canonical snapshot; the plan is the TL's proposal. Deterministic tests exercise the trusted domain with the policy; HTTP/browser tests are separate. An approved demo does not prove real models, Figma, or draft PR publication were exercised.
