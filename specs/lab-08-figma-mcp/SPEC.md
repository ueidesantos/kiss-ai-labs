# SPEC — LAB-08: Figma specification through MCP

## KISS request and sources of truth

Evolve Lab 07's discount simulator from a Figma file with initial, calculating, success, and error states. The visual specification must come from real frames queried through MCP; business documents continue to define limits and calculations. Do not replace Figma with a generated image or design Markdown.

Stack: .NET 11/C#, ASP.NET Core for the reference application, and HTML/JS. Reuse Lab 07's contract and calculation; do not duplicate checkout, login, or database. No external NuGet packages.

The user authorized creation of a new file and confirmed that Figma is installed/connected. In this session, the connector tools were not exposed to the agent and browser navigation was denied by policy; real creation remains pending operational access. Do not invent a file ID or URL.

The user later provided file `jms60WykB5z30uSU0phFqe`, node `0:1`, with Design and Prototype links recorded in `08-sdd-figma-mcp/figma.source.json`. This is the target for lookup and editing when the connector is available. Its contents and node type have not yet been read; consult metadata and select the frames before implementation. This agent did not create the supplied file.

The latest target is the copy `ZUseBZLMJIIEN5HjFBB0fu`, `KissProject4Codex`, preferred in `figma.source.json`; earlier links were preserved for provenance. None of these links has been presented as an MCP capture or as content already implemented.

## Requirements

| ID | Requirement | Verifiable criterion |
|---|---|---|
| F-1 | Connect to the Figma desktop MCP server using Streamable HTTP | initialize, notifications/initialized, tools/list, and tools/call with session and protocol version |
| F-2 | Discover tools and query a real node/frame | get_metadata, get_design_context, and get_screenshot; an error or missing tool is not success |
| F-3 | Persist MCP-returned context, metadata, and screenshot | Evidence file identifies endpoint, node, real/mock mode, time, and each result |
| F-4 | TL combines Figma specification with business RAG and delegates to Front/Back | Front requirement and proposal reference FIGMA; contract and business rules follow Lab 07 |
| F-5 | Specify design details and states without inventing evidence | Dimensions, layout, styles, labels, states, and IDs come from capture; missing information blocks visual approval |
| F-6 | Demo and tests work without credentials and make no claim of real connection | Mock transport reproduces the MCP contract; fixtures labeled fictional; tests run without sockets |

## MCP contract

- Default endpoint: `http://127.0.0.1:3845/mcp`, with the desktop server enabled in Figma.
- JSON-RPC 2.0 transport over HTTP POST, `Accept: application/json, text/event-stream`.
- Negotiate protocol `2025-06-18`, confirm the returned version, send `MCP-Protocol-Version` in subsequent requests, and forward `Mcp-Session-Id` when received.
- Send initialized notification without an id; accept 202 without JSON. Support JSON and SSE request responses; reject a response with a different id and JSON-RPC error messages.
- Call tools listed by tools/list; use arguments compatible with the returned inputSchema. Node is required in real mode. This lab's standalone client does not implement OAuth for the remote server or bypass Figma client controls.
- Limit timeouts and response sizes; record tool errors without saving credentials. Do not execute code received from Figma.

## Figma file to create once connected

Name: `KISS AI Lab 08 — Discount simulator`.

A page `Specification` with four desktop frames (1440×900) and one mobile frame (390×844). Names: `01 Initial`, `02 Calculating`, `03 Success`, `04 Error`, `05 Mobile`.

- Title: Discount simulator; description: simulation without charges or recording.
- Labeled fields: Order amount (R$), Discount (%). Defaults: 100 and 10.
- Help text: Discount allowed from 0 to 20%.
- Button: Simulate; loading state: Calculating… and disabled.
- Success: original R$100.00, discount 10%, total R$90.00.
- Error: percentage 21%, readable limit message, no previous result visible.
- Auto layout: vertical form; responsive layout without horizontal scrolling.
- Proposed design tokens: background `#F1F5F7`, surface `#FFFFFF`, text `#172B36`, action `#165C72`, error `#A01919`; Inter font, spacing 8/16/24/32, radii 6/16.
- These are instructions for creating the file, not evidence of an existing design. After creation, record the URL, IDs, and MCP screenshot before implementing Front.

## Delegation

TL: consult MCP and business sources, issue a traceable candidate SPEC, define the contract, and assign tasks. Front Dev: implement only from real design context and screenshots, adapt to the stack, and validate states/geometry. Back Dev: preserve Lab 07's `POST /api/discount` endpoint and decimal validation. The reference API can run independently; Lab 08 visual conformance remains pending until Figma is connected.

## Gates

G1: scope defined. G2 technical: MCP and API contracts defined. G2 visual: pending a real file and high-fidelity context. G3/G4: integration code tested with mocks, without confusing mocks for a connected Figma. Final delivery requires creation/capture in Figma, visual verification, and publication.

## Delivery adjustment authorized by the user — 2026-10-11

The user confirmed the [community file](https://www.figma.com/community/file/1691245874203095002/kissproject-4-codex?fuid=999015154036575904) as the primary reference and authorized retaining at least the link if access is unavailable. The latest Design copy is `L9IkL9GoyMixBb3bbUXihp`, with supplied node `0:1`, and has not been inspected. Documentation delivery may proceed with this link; MCP capture and visual conformance remain unvalidated and must not be reported as complete. This exception removes real access as a blocker to publishing the documentation. The subscription-limitation hypothesis has not been confirmed.
