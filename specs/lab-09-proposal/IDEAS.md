# Lab 09 — From a business change to a reviewable patch

**Status: proposal for discussion; not implemented, not approved as a SPEC, and not added to the backlog.**

Role in this analysis: discovery subagent, also covering architecture and product. Basis: code and documentation for Labs 07 and 08, their SPECs, and BACKLOG. The goal is to make the lab more useful in a developer's day-to-day work without turning it into a platform.

## Main proposal

**Suggested name: SDD Change Loop — A rule changed. What needs to change in the product?**

Fictional request: the discount simulator now distinguishes **Standard** and **VIP** customers. Standard allows up to 10%; VIP allows up to 20%. The screen adds a profile choice, limit guidance, and corresponding error states. Amount handling, rounding, and the no-charge behavior stay as in Lab 07. The user selects a profile; this is a simulation, with no authentication or eligibility verification.

Observable example: R$ 100 at 15% is rejected for Standard and returns R$ 85 for VIP. R$ 100 at 10% still returns R$ 90 for both. These are proposed rules for the new exercise, not an authorized change to earlier labs.

The teaching value comes from starting with an existing product and a small change. The TL should identify the impact, update a candidate SPEC, delegate only what is needed, receive proposals, review tests, and produce a traceable change package. This is close to the everyday work of maintaining an application.

## How this builds on earlier labs

| Today | Proposed next step |
|---|---|
| Lab 07 generates proposals from an initial request | Compare the old and new rules, find affected requirements/files, and propose a bounded change |
| Lab 08 adds visual context from Figma | Link each screen change to its source frame/state and record what could not be retrieved |
| References point to retrieved documents | Store the path, excerpt, and content hash used to reproduce the analysis |
| Front and Back each receive one task | Also provide the explicit list of allowed files and the changed contract |
| Proposals require review | Deliver a separate review, criteria by requirement, and verifiable gaps |

There is no need to add microservices, queues, a vector database, Kubernetes, or an agent framework. The new challenge is managing the change, contracts, and evidence.

## KISS workflow

1. Receive `change.md`: previous behavior, desired behavior, and examples.
2. Retrieve current business documents and available visual context; record their origin and hash. Keep outdated documents out of the current collection.
3. Have the TL produce `impact.json`: affected requirements, contract changes, expected files, conflicts, and questions.
4. Prepare `SPEC.candidate.md`, acceptance examples, and Front/Back tasks. Update the canonical SPEC only after reviewing the proposal.
5. Have Dev Back propose profile validation; have Dev Front propose a selector, guidance, and error handling based on the available visual spec.
6. Have the reviewer check the contract, sources, scope, and evidence; record `accepted`, `changes-requested`, or `blocked` without confusing an AI opinion with a test that was run.
7. Generate a delivery summary with the observable change, diff, tests run, and limitations.

## Stack and minimum size

- .NET 11 / ASP.NET Core / C#; simple HTML, CSS, and JavaScript.
- In-memory lexical RAG reusing the concept from Lab 07; three or four Markdown documents are enough.
- Ollama is optional for the TL, Front, Back, and reviewer; use explicit calls, with no orchestration framework.
- Use the Lab 08 Figma MCP client when available; preserve the manifest and link when it is not.
- JSON and Markdown files for tasks, sources, proposals, and evidence. No request persistence.
- One application project, one small orchestrator, and one test project. Reuse existing components only when it reduces duplication without coupling the exercise to future changes.

Offline mode uses a prepared scenario and identifies it as such. It supports studying the workflow and running the reference application; it does not prove AI use, Figma access, or visual fidelity.

## Proposed contract for the new application

`POST /api/discount` accepts `amount`, `discount`, and `customerType` (`standard` or `vip`). The profile is required in this new lab; missing or unknown values return a validation error. Lab 07 keeps its original contract.

Success preserves monetary values as decimal text and may add `customerType` and `maxDiscount` to explain the calculation. The backend is authoritative for the limit; frontend guidance does not replace validation. The final contract must be in the SPEC before implementation.

## Proposed acceptance criteria

| ID | Expected evidence |
|---|---|
| AC-01 | For 100/10%, Standard and VIP return 90.00; Standard/15% fails; VIP/15% returns 85.00; VIP/21% fails |
| AC-02 | Missing/unknown profile, non-positive amount, and negative discount are rejected; decimal rounding preserves Lab 07 rules |
| AC-03 | The screen has a labeled profile, matching limit, loading state, result, and error; changing the profile clears stale results |
| AC-04 | The impact report links rule → requirement → contract → task → files → test, with inspectable source excerpts |
| AC-05 | Front cannot change the calculation; Back cannot change the design. Changes outside the file list are flagged for review |
| AC-06 | Conflicting limits in two current sources cause an explicit block; search similarity does not decide which rule is correct |
| AC-07 | Evidence distinguishes a test that ran, a suggested scenario, and pending review; a test failure cannot be turned into model approval |
| AC-08 | When Figma MCP is unavailable, show the link and keep visual review pending; do not invent a frame, capture, or validation |
| AC-09 | Offline demo works without Figma/Ollama and identifies prepared responses; real mode records which roles actually called the model |

## Three complexity levels

| Level | Deliverable | Guardrail to keep it KISS |
|---|---|---|
| **1 — Impact and reference application** | Working profile-based change; source retrieval; impact report; candidate SPEC; tasks; replay and domain/contract tests | One request, one contract, and one screen; proposals are not applied automatically |
| **2 — Agents with review and bounded correction** | Real TL/Front/Back/reviewer; proposals as diffs; scope validation; one correction round after review; compare evidence before and after | At most one correction per run; explicit timeout and call limits; persistent failure ends with a diagnosis |
| **3 — Change detection and regression** | Compare business and Figma-capture hashes across two runs; invalidate evidence tied to changed sources; select affected scenarios and run the full suite before completion | Compare local snapshots with no continuous monitoring; visual changes require an appropriate review, not just matching hashes |

**Recommendation:** implement Level 1 first, plus a simple real path from Level 2. Level 3 can come later. This lets the lab demonstrate a working application and real collaboration when a model is available, while keeping offline replay clear.

Automatically executing proposed code adds an important boundary: local tests do not isolate arbitrary code. At Level 1, implement and review the reference separately; agents generate proposals. Before applying and running proposals at Level 2, define an isolated execution environment and explicit authorization in the workflow contract. A separate checkout alone is not execution isolation.

## Figma and Spec Kit in a future delivery

The proposed design calls for frames for Standard, VIP, loading, result, limit error, and mobile. The file should represent the chosen exercise; the current Lab 08 link may provide context, but does not prove that these new states exist.

The delivery includes a constitution, research, clarifications, canonical SPEC, plan, data model, contract, tasks, scenarios, quickstart, and convergence report. The kit should explain the change between versions and the agent roles. State separately whether the official Spec Kit CLI was used or the documents were authored by following the process.

## Two alternatives

### A. Find discrepancies between Figma, business rules, and the application

Request: the screen says “up to 20%,” but the current policy allows 10% for Standard. RAG retrieves the rule, MCP gets the screen, the TL identifies the conflict, and delegates a copy/validation fix. The main result is a reproducible discrepancy report with links and test cases. This has a smaller product scope and greater dependence on real design access; extracted text alone must not be treated as proof of visual fidelity.

### B. Request a discount exception

Request: discounts above 10% and up to 20% require a justification before simulation. Front shows a conditional field; Back validates the justification; RAG explains the reason with a source; the TL keeps the contract and tests in sync. Human approval and pending/approved/rejected states can come later. This offers a richer product flow, but persistence, concurrency, and identity should stay out of the first version.

## Next decision

Choose the request and level before creating `specs/lab-09-.../SPEC.md`. This analysis does not start another lab, change existing labs, or claim that the pending Lab 07/08 integrations are complete.
