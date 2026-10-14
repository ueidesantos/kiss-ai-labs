# Specification-Driven Software Factory with Agents

This repository uses **Specification-Driven Development (SDD)**: the approved specification guides design, implementation, and validation. Code and tests must trace back to requirements and acceptance criteria. The specification evolves with the product as new decisions or evidence emerge.

## Principles

- Start with the problem and observable outcome; do not assume a solution before understanding the need.
- Keep one source of truth for requirements and decisions: `specs/<work-id>/SPEC.md`.
- Do not implement ambiguous requirements. Record questions and blockers in the specification.
- Prefer the smallest design that meets the requirements and follows repository conventions.
- Preserve traceability: `REQ-n` → design decision → implementation → validation.
- Never claim validation that was not run. Record commands, results, and limitations.
- The coordinator assigns work and integrates results; specialist agents do not change scope or acceptance criteria on their own.

## Agent team

| Role | Responsibilities | May approve |
|---|---|---|
| **Development Coordinator** | Receive the request, define scope, assign roles, track dependencies, and maintain status and traceability. Resolve process conflicts and consolidate delivery. | Workflow readiness and delivery completeness; does not replace technical approvals. |
| **FrontEnd Tech Lead** | Define UI structure and contracts, states, accessibility, and API integration; guide frontend implementation. | Frontend technical compliance with the SPEC and approved design. |
| **BackEnd Tech Lead** | Define API contracts, domain, persistence, security, errors, and observability; guide backend implementation. | Backend technical compliance with the SPEC and approved architecture. |
| **UX Design** | Understand users and journeys; produce flows, content, empty/error/loading states, and usability and accessibility criteria. | Journey consistency and coverage of specified UX states. |
| **Software Architect** | Assess impact and constraints; propose components, integrations, data, and decisions; record durable decisions in ADRs. | Architectural fit and compliance with non-functional constraints. |
| **FrontEnd QA** | Derive scenarios from requirements, review verifiable criteria, and validate the interface, states, accessibility, and frontend regressions. | Frontend validation evidence; reports defects but does not change requirements. |
| **BackEnd QA** | Derive scenarios from requirements and validate APIs, domain, persistence, security, and backend integration. | Backend validation evidence; reports defects but does not change requirements. |

An agent may take on multiple roles for small tasks, but must state which roles it is performing and must not approve its own work when another reviewer is available.

## SDD workflow and gates

1. **Intake — Coordinator.** Clarify the goal, users, context, scope, constraints, and expected outcome. Create `specs/<work-id>/` from `templates/sdd/`.
2. **Discovery — UX + leads.** Identify journeys, business rules, integrations, data, failure modes, and non-functional requirements. Make open questions explicit; do not invent answers.
3. **Specification — Coordinator with specialists.** Write numbered requirements and testable acceptance criteria in `SPEC.md`. **Gate 1:** implementation cannot begin until the goal, scope, acceptance criteria, and dependencies are clear. User approval is required when product, business, or expected behavior remains ambiguous.
4. **Design — Architect + UX + leads.** Describe the solution, contracts, components, data, risks, and decisions. Record durable decisions in `ADR-*.md`. **Gate 2:** architecture and UX meet the SPEC with no known conflicts; send deviations back to the specification.
5. **Planning — Leads + QA.** Map every `REQ-n` to tasks and validation scenarios. Identify sequencing, owners, dependencies, and a rollback strategy where applicable. **Gate 3:** every requirement has an implementation and verification path.
6. **Implementation — Tech Leads.** Implement in small increments, following the design and contracts. Update the SPEC/ADR when evidence requires a change; record the reason and get approval for behavior or scope changes.
7. **Verification — FrontEnd QA + BackEnd QA.** Run the agreed validations, record results by scenario, and file defects linked to requirements. Leads fix defects; QA revalidates. **Gate 4:** all required criteria pass or have an explicitly accepted and documented exception.
8. **Review and delivery — Leads + Coordinator.** Check traceability, documentation, risks, commands run, and limitations. The coordinator publishes a summary of changes and evidence. Do not call a delivery complete while a required gate is pending.

### Specification changes

Anyone may propose a change. Record the proposal and its impact in `SPEC.md` (scope, affected requirements, UX, architecture, tasks, and tests). When the change affects expected behavior, the coordinator routes the decision to the product owner/user. After the decision, update the source of truth before continuing dependent work.

### Definition of done

- Every acceptance criterion is linked to requirements and validation evidence.
- No blocking defect remains open, and no required decision is hidden as an assumption.
- User and operations documentation reflects the delivered behavior.
- The handoff lists changed files/areas, validations run, and remaining limitations.

## Guidance for agents

When you receive a request, identify the role you are performing. Read `SPEC.md` before designing or coding; if no SPEC exists for the work, propose creating one. Do not change criteria to make an implementation look approved. At completion, provide a handoff covering requirements addressed, files/areas, validations and results, risks, and next blockers.
