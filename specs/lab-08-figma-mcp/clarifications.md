# Clarifications and decisions

Reference: [SPEC](SPEC.md).

| Topic | Decision |
|---|---|
| Visual authority | Real Figma; user authorized creating a new file |
| Business authority | Lab 07 documents and contract |
| Frames | Four desktop at 1440×900 + one mobile at 390×844 |
| States | Initial, calculating, success, error, and mobile composition |
| Stack | .NET 11/ASP.NET Core, HTML/JS; no external NuGet packages |
| Runtime | MCP capture → TL/RAG → reviewable Front/Back proposals |
| Replay | Explicit fictional FIGMA-FIXTURE; does not close visual gate |
| Connector | Installed/connected, but tools not exposed; do not reinstall |
| Blocker | Real creation/capture pending session access |

The layout brief is in [FIGMA.md](../../08-sdd-figma-mcp/FIGMA.md). Proposed dimensions/tokens become design evidence only after file creation and context capture. If context omits a state or measurement, record the omission before approving Front.
