# Consistency analysis

Date: 2026-10-11. Source of truth: [SPEC](SPEC.md). Result: reference design and implementation align in the core; real integrations remain pending.

| Requirement | Implementation | Available verification |
|---|---|---|
| REQ-1 | `src/Workflow.cs` / Retrieval; business Markdown | Ranking, normalization, empty corpus; .NET checks |
| REQ-2 | Plan/ValidatePlan and canonical SPEC | Refs, requirements, roles, and contract; .NET checks |
| REQ-3 | `app/index.html` | Six Node tests; labels, contract, states, and BRL with simulated DOM/fetch |
| REQ-4 | DiscountCalculator/DiscountEndpoint | Decimal, limits, errors, and response through in-memory HttpContext |
| REQ-5 | Workflow.RunAsync / roles and individual tasks | Mock HTTP verifies sequence and context; no real model execution |
| REQ-6 | Artifact persistence and review.json | Replay, refs, mode, and proposals not run |

## Necessary distinctions

- B1/B2/B3 identify documents in the SPEC. S1/S2/S3 are runtime IDs assigned by file order; do not assume number-based equivalence.
- RAG uses lexical TF-IDF/cosine similarity. This lab has neither embeddings nor a vector database.
- Runtime SPEC is a candidate; the [SPEC in this folder](SPEC.md) remains canonical.
- Proposals and suggested scenarios are not execution evidence. The reference app was implemented separately for this delivery.
- The frontend does not apply business rounding; it formats strings returned by the API.
- Spec Kit is the process reference for this authored kit; its official CLI/skills were not installed or run.

## Pending

Real HTTP/browser, real Ollama, and GitHub publication have no successful evidence due to network restrictions. No scope contradiction was found that blocks review of the core. [Convergence](convergence.md) retains these as pending limitations.
