# Request clarifications

The [canonical SPEC](SPEC.md) records the approved implementation outcome.

| Topic | Recorded decision |
|---|---|
| User and need | A service representative wants to simulate a discount before presenting a proposal |
| Stack | .NET 11, ASP.NET Core, C#; HTML/CSS/JS without dependencies |
| Domain | Fictional; this lab's documentation is not a real company's policy |
| Limit | Order > 0; discount from 0 to 20%, inclusive |
| Rounding | HALF_UP for positive currency, to two decimal places |
| Responsibility | API applies the rule; Front collects/displays; TL coordinates and verifies |
| Runtime agents | Ollama calls with distinct roles; developers receive only their task |
| Source of truth | SPEC in this folder; model output remains a candidate |
| Commercial action | No charges, order recording, or management approval |
| Publication | Coordinator attempts commit/push; report actual block if any |

There is no product ambiguity that blocks this increment. Operational dependencies remain pending: real HTTP, real Ollama, and GitHub access. They do not change acceptance criteria to make the work appear approved.
