# Research and technical decisions

Reference: [SPEC](SPEC.md). Process research: [official Spec Kit](https://github.com/github/spec-kit), consulted 2026-10-11. The artifacts below were authored for this request.

| Question | Decision | Reason and limit |
|---|---|---|
| Stack | .NET 11 / ASP.NET Core / vanilla HTML | Meets requested stack; three projects without external NuGet packages |
| Retrieval | TF-IDF and cosine similarity over whole Markdown files | Three-document corpus; simple, local, inspectable. Not equivalent to semantic embeddings |
| Coordination | TL first, then one Front call and one Back call | Shared contract before proposals; three sequential calls |
| Currency | `decimal`, two places, `AwayFromZero` | For positive amounts, implements source HALF_UP rule |
| Persistence | Workflow artifacts only | Simulation creates no order or charge; no database needed |
| Proposal execution | Human review | Generated code is a proposal file, not automatically executable |
| Model-free demo | Explicit replay | Exercises artifacts and validation; does not measure AI quality |

Not adopted for this request: vector database, microservices, Front framework, authentication, MCP, or automatic deployment. These are scope limits, not promises of future features.

Next evidence needed: run the HTTP application and three Ollama calls in an environment with authorized access; review fidelity to cited rules in addition to ID validity.
