# Data model

| Artifact | Contents | Constraint |
|---|---|---|
| Sources in report | Path, excerpt, score, and SHA256 | Integrity does not guarantee truth |
| Identities | Developer/reviewer models and digests | Real mode requires two distinct identities |
| Proposals | Role and JSON content | No free-form code execution |
| `policy.json` | Standard/VIP limits | Tests protect 10/20 |
| `front.json` | `title`, `help`, `standardLabel`, `vipLabel` | Text is displayed without interpreting HTML |
| Review | `model`, `summary`, `findings` | Each finding has `id`, `severity`, `evidence`, `fix` |
| Gate | Tests + final review | `high`/`critical` findings or failed tests block |
| `report.json` | Status, policy, reviews, tests, history, proposals, events, sources, and model usage | Offline mode explicitly labeled |
| `manifest.json` | Integrity of run artifacts | Publication rejects mismatches |
| `candidate` | Materialized app and core | Kept separate from canonical source |

The history allows comparison of the initial proposal and the single correction. Token counts reflect usage returned by the provider when available, not invented estimates. Currency uses decimal arithmetic and AwayFromZero rounding; there are no accounts or payments.
