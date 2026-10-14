# The lab's SDD kit

This kit records a complete request: intent, requirements, decisions, contracts, tasks, and verification criteria. It follows the work organization presented by GitHub's [Spec Kit](https://github.com/github/spec-kit), without claiming that the official CLI was installed or run. The executables are specific to this lab.

| Stage | Artifact | Question answered |
|---|---|---|
| Constitution | [Constitution](.specify/memory/constitution.md) | Which constraints are non-negotiable for the agents? |
| Specify | [Request](DEMAND.md), [SPEC](../specs/lab-09-ai-squad/SPEC.md) | What behavior needs to change? |
| Clarify | [Clarifications](../specs/lab-09-ai-squad/clarifications.md) | What do profile, correction, and delivery mean? |
| Research | [Decisions](../specs/lab-09-ai-squad/research.md) | Why do these choices keep the lab small? |
| Plan | [Plan](../specs/lab-09-ai-squad/plan.md), [data](../specs/lab-09-ai-squad/data-model.md), [contract](../specs/lab-09-ai-squad/contracts/business-rules.json) | How do the parts work together? |
| Tasks | [Tasks](../specs/lab-09-ai-squad/tasks.md) | Who implements and verifies each requirement? |
| Implement | [Quickstart](../specs/lab-09-ai-squad/quickstart.md) | How do I run the application and workflow? |
| Analyze | [Analysis](../specs/lab-09-ai-squad/analysis.md), [checklist](../specs/lab-09-ai-squad/checklists/requirements.md) | Are there gaps between requirements, implementation, and evidence? |
| Converge | [Convergence](../specs/lab-09-ai-squad/convergence.md) | What has been proven and what is still pending? |

## Two specifications with different roles

The canonical SPEC was defined before implementation and is the source of acceptance criteria. The run's `SPEC.md` file is a snapshot of the canonical SPEC, preserved with the candidate. The TL's interpretation appears in `plan.md`; the runner does not present the SPEC copy as model-generated text. None of these artifacts automatically replaces the canonical source.

## Delegation does not mean losing control

The TL plans; Front proposes text; Back proposes limits; another model acts as adversarial QA and records findings with evidence, severity, and a fix. Contracts constrain each role's latitude. The application is built from known templates and validated data. This bounded scope makes delegation and correction visible without turning the lab into an unrestricted coding agent.

In offline mode, proposals and reviews are prepared, with no AI. The initial limit error is deliberate, but the rejection comes from running the real tests. In Ollama mode, proposals come from the model; a correction happens only if the gate identifies a failure.

## Two models, verifiable responsibility

The developer model handles TL/Front/Back; the reviewer has a distinct identity. The gate combines deterministic tests with the absence of `high`/`critical` findings. A correction requires new tests and a new review. The separation illustrates “Claude builds, Codex reviews,” but this kit's adapters use Ollama; it does not claim commercial API integration.

## What the tests prove

The runner compiles the candidate and runs deterministic domain tests against the proposed policy. Those tests use the runner's trusted domain; they do not represent HTTP calls to the candidate. API and browser checks are separate evidence, recorded in [VALIDATION.md](../specs/lab-09-ai-squad/VALIDATION.md).
