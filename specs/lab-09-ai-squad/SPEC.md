# Lab 09 — KISS AI Squad: one AI builds, another reviews

Approved request: Standard customers can simulate discounts up to 10%; VIP customers up to 20%. The amount must be positive and no more than R$1 billion. Use decimal currency and AwayFromZero rounding. No authentication: the profile is provided for simulation, with no charges or customer persistence.

## Requirements and acceptance

- **REQ-1 — Two models:** A .NET 11 CLI runs TL, Front, and Back with the developer model (`--model`) and adversarial review with the reviewer (`--review-model`), through Ollama. In real mode, models must have distinct names and digests; aliases for the same model are rejected. Offline mode identifies prepared proposals and review, with no AI calls.
- **REQ-2 — Context:** Local lexical RAG retrieves business documentation with path, excerpt, and SHA256. Documents are data, not execution instructions.
- **REQ-3 — Controlled scope:** Bounded JSON proposals feed known templates. Do not execute arbitrary text returned by the LLM. Explain that the developer proposes text and limits, not free-form code.
- **REQ-4 — Independent gate:** Deterministic tests protect limits, invalid inputs, and currency handling. The reviewer cannot change tests or override failures. `high`/`critical` findings block approval. At most one Back correction is allowed, followed by build, tests, and a new review; another failure ends in rejection.
- **REQ-5 — Evidence:** Generate a working ASP.NET Core application, TL plan, canonical SPEC snapshot alongside the candidate, proposals, diff, build, tests, reviews, and events. Record model identities, duration, and token counts when available. The dashboard distinguishes no report, a run in progress, and a result.
- **REQ-6 — Honest demo:** Offline mode prepares an incorrect Standard=20% proposal; real tests detect the violation. A prepared correction to 10% passes the same gates. Offline reviews are labeled as prepared; do not claim autonomous AI discovery.
- **REQ-7 — Reviewable delivery:** An opt-in script checks the approved gate and integrity manifest, creates a branch and draft PR; no automatic merge. Publishing the lab to `main` is separate from creating a PR with a candidate.
- **REQ-8 — Presentation:** Lab 09 becomes the main feature on the cover, preserving the original scene and Lab 08 as the previous project. Figma is an optional reference link; do not simulate a real capture.

## Adversarial review

The reviewer receives the requirements, sources, proposal, and candidate results. It looks for contradictions in rules, validation, and user experience. Each finding includes an identifier, severity, evidence, and suggested fix. Review is a second gate: no severe findings does not replace tests. A transport error, invalid response, or unverifiable model identity fails explicitly; it never triggers offline mode automatically.

“Claude builds, Codex reviews” illustrates the intended separation. This implementation uses **two distinct models available through Ollama**; it does not integrate the commercial Claude or Codex APIs. Different names do not guarantee statistical independence or lack of bias; digest verification only prevents aliases for the same installation.

## Architecture and verification criteria

Console runner + domain + reference app + deterministic tests + static dashboard. Bounded sequential workflow, fixed request, one correction. Proposals are typed data; this is not a general-purpose coding agent.

Verify the build, demo with initial rejection/correction/new review, blocking on a severe finding, rejection of identical models/aliases, invalid responses, publication integrity, and simulator. Record real integration and tests with a prepared server separately: a simulated HTTP response does not prove real model reasoning.

The build compiles the candidate's files. Deterministic tests use the runner's trusted domain with the proposed policy; API/browser evidence is separate. A run's `SPEC.md` is a canonical snapshot, while `plan.md` contains the TL's proposal.
