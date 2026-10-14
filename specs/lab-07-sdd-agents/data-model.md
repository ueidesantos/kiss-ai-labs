# Data and invariants

Reference: [SPEC](SPEC.md). No tables or order persistence.

| Object | Fields | Invariants |
|---|---|---|
| Simulation request | `amount`, `discount` | Exactly two numeric JSON fields; representable as decimal; amount > 0; discount ∈ [0,20] |
| Result | `original`, `discount`, `total` | Invariant decimal strings; currency uses two decimal places |
| Source | `Id`, `Path`, `Text`, `Score` | Stable ID by file order; source path and cosine score |
| Requirement | `Id`, `Text`, `Sources` | Requirement cites IDs from retrieved context |
| Acceptance | `Id`, `Requirements`, `Scenario` | Criterion references existing requirements |
| DevTask | `Role`, `Requirements`, `Acceptance`, `Instruction` | Exactly one Front task and one Back task |
| Contract | `Method`, `Path`, `Request`, `Success`, `Error` | Preserves the approved API contract |
| Proposal | `Role`, `Requirements`, `Sources`, `Code`, `Checks` | Role and requirements match the task; checks are suggestions, not execution |

Page state flow: waiting for input → calculating → success or error → new simulation. The previous result is hidden when a new attempt starts. The button is disabled to prevent duplicate requests.

Run artifacts: `sources.json`, `plan.json`, `handoffs.json`, `SPEC.candidate.md`, `tasks.md`, `front.proposal.html`, `back.proposal.cs`, `review.json`. `review.json` records the mode, pending review, and that generated code was not run.
