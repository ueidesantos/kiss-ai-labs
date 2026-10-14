# Constitution — Lab 07

Version 1.0 · 2026-10-11 · Scope: discount simulator and educational workflow.

1. **Business before code.** Rules are grounded in business Markdown; conflicts and gaps are recorded, never filled in as facts by the model.
2. **One canonical SPEC.** The [request SPEC](../../../specs/lab-07-sdd-agents/SPEC.md) is the source of truth. A workflow-generated `SPEC.candidate.md` requires human review.
3. **KISS.** .NET 11, ASP.NET Core, and vanilla HTML/JS; no external NuGet packages, database, login, or charges. Lexical TF-IDF/cosine retrieval is enough for three documents.
4. **Bounded delegation.** TL assigns one Front task and one Back task, with requirements, acceptance criteria, and contract. Developers do not expand scope. Proposals are not run automatically.
5. **Verifiable evidence.** REQ → source → task → code → test. Offline replay and mock tests do not prove real generation or HTTP operation.
6. **Strict values and contract.** Currency calculation uses `decimal`; API validates data, Front presents it. Behavior changes update the SPEC before code.
7. **Honest convergence.** Pending integrations prevent claims of complete validation. The official Spec Kit CLI/skills were not installed or run for this delivery.

Changes to this constitution must identify the reason, affected requirements, and review evidence in the request kit.
