# Consistency analysis

Based on the [SPEC](SPEC.md). Date: 2026-10-11.

| Requirement | Implementation/evidence | Limit |
|---|---|---|
| F-1 | FigmaMcpClient initialize/notification/session/protocol/JSON/SSE | 19 mock checks, not a real server |
| F-2 | tools/list, schemas, and required calls | Real node still unavailable |
| F-3 | Capture includes metadata/context/screenshot/source/time | Real capture and CLI recording need confirmation |
| F-4 | Coordinator integrates FIGMA source with Lab 07 workflow | Do not claim real generation/delegation without capture |
| F-5 | Frame, token, and state brief | Creation instructions; visual gate pending |
| F-6 | Fake transport and explicit demo/fixture mode | Mock does not prove Figma conformance |

Sources have different authority: business documentation defines rules; Figma defines presentation; the SPEC records decisions; tests provide execution evidence. A design text is not real Figma. An image in the README is not by itself an MCP capture.

Connector status: screenshot confirms installed/connected; tools were not exposed in the inventory and Figma navigation was denied, preventing real action. Do not turn this block into a reinstall request. The [contract](contracts/design-source.json) keeps URL/node null because no file was created.

Valid FIGMA references prove identified provenance, not that all generated descriptions are faithful. Visual comparison and human review remain mandatory. [Convergence](convergence.md) records the result: full delivery is not yet approved.
