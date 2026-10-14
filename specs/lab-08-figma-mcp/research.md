# Research and decisions

Based on the [SPEC](SPEC.md). Sources consulted 2026-10-11: [Figma desktop MCP](https://developers.figma.com/docs/figma-mcp-server/local-server-installation/), [Figma remote/OAuth](https://developers.figma.com/docs/figma-mcp-server/remote-server-installation/), [MCP lifecycle 2025-06-18](https://modelcontextprotocol.io/specification/2025-06-18/basic/lifecycle).

| Decision | Rationale | Limit |
|---|---|---|
| Real Figma file required | Visual specification needs verifiable nodes and context | Mock/Markdown do not replace design |
| .NET 11 client with HttpClient/JSON | Small protocol example without external NuGet packages | Custom client requires real compatibility validation |
| Local desktop endpoint | Documentation lists 127.0.0.1:3845/mcp | Requires Figma desktop, open file, and enabled server |
| MCP negotiation | Session, protocol, schemas, and errors precede capture | Do not assume protocol version or inputSchema |
| Reuse Lab 07 | Contract/calculation already defined; new focus is the visual source | Lab 07 UI does not prove Lab 08 fidelity |
| Separate remote Codex | OAuth managed by the connector | .NET client does not collect/reuse tokens or implement OAuth |

A session screenshot confirms the connector is installed/connected; the tool inventory did not expose Figma, and navigation was denied. This is an operational limitation, not evidence that the plugin needs reinstalling.
