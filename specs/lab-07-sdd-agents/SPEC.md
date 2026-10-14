# SPEC — LAB-07: SDD + agents + business RAG

- Status: defined for implementation of the requested educational example.
- Coordinator/TL: primary agent; Front Dev and Back Dev: delegated agents.
- Request: simulate the final amount of an order with a discount, using rules retrieved from business documentation.
- Fictional domain: the amounts and rules below apply only to this example.

## Goal and scope

A page with an order amount, discount, and Simulate button; an API that validates and calculates; and an agent workflow that consults documents, prepares a SPEC, and delegates Front and Back proposals. No login, database, checkout, management approval, or automatic deployment.

## Business sources

- B1: `07-sdd-agents-business-rag/business/descontos.md`: order greater than zero, discount from 0 to 20%, currency rounded to two decimal places using HALF_UP.
- B2: `07-sdd-agents-business-rag/business/glossario.md`: original amount, percentage, and final amount.
- B3: `07-sdd-agents-business-rag/business/simulacao.md`: simulation only; no recording or charging.

## Requirements and acceptance criteria

| ID | Requirement | Source | Acceptance criterion |
|---|---|---|---|
| REQ-1 | Consult business documentation before specifying | B1, B2, B3 | AC-1: local TF-IDF/cosine retrieval returns excerpts with IDs and paths |
| REQ-2 | TL produces a SPEC and delegates traceable tasks to Front and Back | B1, B3 | AC-2: JSON plan contains exactly one task per role, requirements, and criteria; references outside the context are rejected |
| REQ-3 | Front collects amount and percentage, sends them to the API, and displays result/error | B2, B3 | AC-3: labeled form, waiting/success/error states, amount in reais, and shared contract |
| REQ-4 | Back validates and calculates using .NET decimal | B1 | AC-4: 100 with 10% returns 90; limits 0/20 accepted; >20, negatives, booleans, non-finite values, and invalid fields rejected |
| REQ-5 | Developers receive the SPEC, contract, and only their assigned task | SPEC | AC-5: two development calls with separate roles; deliverables include requirement and source references |
| REQ-6 | Record evidence and distinguish real AI from offline replay | SPEC | AC-6: run saves sources, SPEC, tasks, and proposals; offline mode states that it used prepared responses and did not call a model |

## Approved contract for the example

- `POST /api/discount`, JSON `{ "amount": 100, "discount": 10 }`.
- HTTP 200: `{ "original": "100.00", "discount": "10", "total": "90.00" }`. Currency values are decimal strings.
- HTTP 400: `{ "error": "readable message" }` for invalid data.
- `GET /` serves the page. No input executes commands or records orders.

## KISS design and delegation

- .NET 11, ASP.NET Core, C#, and framework libraries; HTML/CSS/JS without a frontend build. No external NuGet packages.
- Local lexical RAG with TF-IDF and cosine similarity across three whole Markdown files, without embeddings or a database.
- Agents: explicit HTTP calls to Ollama with TL, Front Dev, and Back Dev roles; sequential calls with a fixed contract.
- TL generates a structured SPEC and two tasks; developers generate code proposals and checks. Proposals are stored in their own directory and are not run automatically.
- The reference application in `app/` was implemented by the delegated agents for this delivery, then reviewed and tested separately. Replay uses proposals from that application; it must not be presented as AI generation.
- The workflow's JSON SPEC is a candidate artifact; this file remains the lab's source of truth until human review.

## Plan

| Owner | Area | Requirements | Validation |
|---|---|---|---|
| TL | Documentation, RAG, workflow, and evidence recording | REQ-1, 2, 5, 6 | Ranking tests, delegation contract, rejections, and replay |
| Front Dev | `app/index.html`, UX/contract guide | REQ-3 | HTML/JS review and payload/result integration |
| Back Dev | `app/DiscountApp.csproj`, calculation, and C# endpoint | REQ-4 | Calculation/validation/contract tests without sockets |
| Reviewer | Integration and evidence | All | Review diff, check tests, and identify blockers |

## Gates and limits

Scope and contract are defined before implementation. A valid reference shows the retrieved source, but does not prove that a generated statement is faithful. Human review remains necessary before incorporating proposals. Real Ollama execution and the HTTP server depend on network permissions; if blocked, record the limitation without claiming those scenarios passed. Do not block the offline demo because models are unavailable.
