# Technical plan and delegation

Based on the [SPEC](SPEC.md) and [constitution](../../07-sdd-agents-business-rag/.specify/memory/constitution.md).

## Components

- `src/SddAgents.csproj`: .NET 11 CLI; retrieves documents, calls TL, validates the plan, calls Front and Back, validates proposals, and saves artifacts.
- `app/DiscountApp.csproj`: ASP.NET Core 11; GET `/` and POST `/api/discount`; isolated `decimal` calculation.
- `tests/LabTests.csproj`: harness without external packages for rules, contract, ranking, delegation, and replay.
- `app/index.html`: vanilla interface, fixed JSON contract, loading/success/error states.

## Sequence

1. Read the request and B1/B2/B3; use lexical RAG to retrieve context with IDs `S<n>`.
2. TL produces a plan/candidate SPEC without changing the [canonical SPEC](SPEC.md).
3. Validate refs, criteria, exactly two roles, and contract.
4. Delegate one Front task and one Back task, each with the SPEC/contract/context.
5. Validate proposals and save files for review without running generated code.
6. Review the reference app and run .NET checks and JS tests.
7. Record convergence and any missing integrations.

| Role | Area | IDs |
|---|---|---|
| TL | RAG, workflow, evidence, integration, and kit | REQ-1, 2, 5, 6 |
| Front Dev | Interface and reproducible JS test | REQ-3 |
| Back Dev | API, calculation, and backend scenarios | REQ-4 |

## Contracts and scope protection

The [JSON contract](contracts/discount-api.json) details input/output. There is no database. Business data is context, not commands; invalid references and incompatible contracts are rejected. Validating a reference does not prove semantic fidelity; human review remains necessary.

## Validation and rollback

[Quickstart](quickstart.md) lists commands. The executable core does not depend on Ollama during replay. Real integrations require network access. Proposals stay in `runs/` and do not affect the reference code. To discard a proposal, do not incorporate it; to revert accepted changes, use a reviewed revert commit.
