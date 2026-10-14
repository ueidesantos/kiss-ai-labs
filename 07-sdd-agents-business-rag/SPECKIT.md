# Applying Spec Kit to the KISS request

This lab brings together SDD artifacts for the discount request and its .NET 11 code. The process was inspired by GitHub's [official Spec Kit](https://github.com/github/spec-kit), whose documentation describes a constitution and the specify → plan → tasks → implement → converge cycle. The `speckit-*` invocations belong in an agent chat, not the terminal.

The kit for this request was written in this repository. It is neither a complete copy of upstream nor an installation of the official CLI; the official CLI and skills were not run here. The application and workflow are executables specific to this lab.

| Stage | Artifact in this delivery | How to use it |
|---|---|---|
| Constitution | [Principles](.specify/memory/constitution.md) | Constrain architecture, delegation, and evidence claims |
| Specify | [Request](DEMAND.md) and [canonical SPEC](../specs/lab-07-sdd-agents/SPEC.md) | Understand outcome, REQ-1..6, and AC-1..6 |
| Clarify | [Decisions](../specs/lab-07-sdd-agents/clarifications.md) | Resolve stack, domain, and limits before implementation |
| Research | [Research](../specs/lab-07-sdd-agents/research.md) | Understand choices and discarded alternatives |
| Plan | [Plan](../specs/lab-07-sdd-agents/plan.md), [data](../specs/lab-07-sdd-agents/data-model.md), [contract](../specs/lab-07-sdd-agents/contracts/discount-api.json) | Share the design across TL, Front, and Back |
| Tasks | [Tasks](../specs/lab-07-sdd-agents/tasks.md) | Delegate by requirement and verify completion |
| Implement | [Workflow](src/Workflow.cs), [API](app/Program.cs), [Front](app/index.html) | Deliver the increment bounded by the SPEC |
| Analyze/checklist | [Analysis](../specs/lab-07-sdd-agents/analysis.md), [checklist](../specs/lab-07-sdd-agents/checklists/requirements.md) | Check consistency and traceability |
| Converge | [Convergence](../specs/lab-07-sdd-agents/convergence.md) | Distinguish validated core from pending integrations |

## Two complementary workflows

The kit records development of the reference application: TL coordinated, Front created the interface, Back implemented the calculation and contract, and validations provide evidence. At runtime, the workflow also demonstrates TL → candidate SPEC → two tasks → developer proposals. Runtime uses lexical RAG over Markdown and makes three sequential calls to Ollama in real mode; it does not start autonomous agents in parallel.

`--offline` uses prepared responses and reference files. It is replay for inspecting the workflow without a model, not AI generation. Proposals from either mode remain for review; they do not replace the canonical SPEC and are not executed.

## Try it

Follow the [quickstart](../specs/lab-07-sdd-agents/quickstart.md). The coordinator reported 66 passing .NET checks and nine passing JS tests in the core. Three .NET projects built without warnings. Real HTTP, real Ollama, and GitHub publication remain pending due to network restrictions in this run.
