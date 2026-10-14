# Technical plan

The [SPEC](SPEC.md) and [constitution](../../08-sdd-figma-mcp/.specify/memory/constitution.md) guide this delivery.

1. Create the authorized real file; record the URL and five node IDs.
2. Capture nodes through desktop MCP: initialize, initialized, tools/list, tools/call for metadata/context/screenshot. Validate schemas, IDs, version, session, errors, and size.
3. Persist the capture as real, identifying endpoint/node/time/results; preserve the returned screenshot.
4. TL combines `FIGMA` with Lab 07's lexical business RAG and produces a candidate SPEC/contract/tasks.
5. Front receives real context and screenshot; Back preserves decimal calculation/contract. Proposed code is not run automatically.
6. Review states/geometry/tokens on desktop/mobile; record evidence and convergence.

Components: `src/FigmaMcpClient.cs`, capture/generation/demo CLI, Lab 07 workflow, reused reference API, and MCP tests without sockets.

Critical dependency: capture does not create a Figma file. Creation depends on supported connector tools; remote authentication does not automatically enable the desktop endpoint.

| Owner | Requirements | Deliverable |
|---|---|---|
| TL | F-1..F-4, F-6 | Capture, source integration, delegation, and evidence |
| Design/Front | F-2, F-5 | Real file, states, and faithful UI after capture |
| Back | F-4 | Preserve Lab 07 API/rule |
| Review | All | Checklist and visual comparison |

If design and business rules conflict, do not resolve silently: update the SPEC with the decision. Discarding proposals that were not incorporated is reversible; code already incorporated should be reverted through Git.
