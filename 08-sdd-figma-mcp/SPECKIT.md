# Spec Kit for the Figma + MCP request

The lab applies a cycle inspired by the [official Spec Kit](https://github.com/github/spec-kit): constitution → specify → plan → tasks → implement → converge. The kit was authored for this request; the official CLI and skills were not installed or run. The `speckit-*` skills are invocations in an agent chat, not terminal commands.

| Stage | File | Outcome |
|---|---|---|
| Constitution | [Principles](.specify/memory/constitution.md) | Real Figma required, KISS limits, and honest evidence |
| Specify | [Request](DEMAND.md), [canonical SPEC](../specs/lab-08-figma-mcp/SPEC.md) | F-1..F-6 and technical/visual gates |
| Clarify/research | [Decisions](../specs/lab-08-figma-mcp/clarifications.md), [research](../specs/lab-08-figma-mcp/research.md) | Separate remote OAuth from desktop client |
| Plan | [Plan](../specs/lab-08-figma-mcp/plan.md), [data](../specs/lab-08-figma-mcp/data-model.md), [contract](../specs/lab-08-figma-mcp/contracts/design-source.json) | Capture → FIGMA source → candidate SPEC → developers |
| Tasks | [Tasks](../specs/lab-08-figma-mcp/tasks.md) | Pending visual creation made explicit |
| Implement | [.NET client](src/FigmaMcpClient.cs) and lab CLI | Protocol integration without running received code |
| Analyze/checklist | [Analysis](../specs/lab-08-figma-mcp/analysis.md), [checklist](../specs/lab-08-figma-mcp/checklists/requirements.md) | Verify sources and evidence by requirement |
| Converge | [Status](../specs/lab-08-figma-mcp/convergence.md) | MCP core tested; visual delivery still blocked |

[FIGMA.md](FIGMA.md) details the frame brief and capture workflow. [Quickstart](../specs/lab-08-figma-mcp/quickstart.md) contains the .NET commands. The reference API is reused from Lab 07 and must not be presented as faithful to a Figma file that does not yet exist.

The visual stage is incomplete: connector installed/connected, tools missing from the session, and browser access denied. Demo/replay show a workflow with fictional content, not real Figma execution. The 19 mock transport checks do not imply final approval.
