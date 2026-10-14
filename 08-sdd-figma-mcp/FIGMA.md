# Figma as a visual specification

**The required Figma file has not yet been created in this run.** The user authorized its creation. The supplied screenshot confirms the connector is installed and connected, but Figma tools are not exposed in this session's callable inventory; Figma navigation was explicitly denied. This blocks real creation/capture and visual approval. No URL or file ID has been invented.

## Two separate integrations

| Path | Purpose | Status/limit |
|---|---|---|
| Codex Figma connector | Create/edit/read through supported tools and platform OAuth | Installed/connected per screenshot; tools unavailable in this session |
| This lab's .NET client | Read nodes through desktop MCP at `http://127.0.0.1:3845/mcp` | Implemented and tested with mocks; real server unverified |

The connector identified in this session is `connector_68df038e0ba48191908c8434991bbac2`. There is no need to request reinstallation because tools are unavailable. The .NET client does not reuse the connector's OAuth tokens, implement remote OAuth, or bypass client restrictions. Official documentation describes the [remote server and OAuth](https://developers.figma.com/docs/figma-mcp-server/remote-server-installation/) and the [desktop server](https://developers.figma.com/docs/figma-mcp-server/local-server-installation/). The custom client's compatibility with the real environment must be validated while respecting Figma-supported clients.

## File to create when tools are available

Name: `KISS AI Lab 08 — Discount simulator`. Page: `Specification`. This is a creation brief, not evidence of an existing design.

| Proposed frame | Size | Content/state |
|---|---|---|
| 01 Initial | 1440 × 900 | Amount 100, discount 10, limit help text, Simulate enabled |
| 02 Calculating | 1440 × 900 | Button disabled, “Calculating…” text, previous result hidden |
| 03 Success | 1440 × 900 | Original R$100.00; discount 10%; total R$90.00 |
| 04 Error | 1440 × 900 | Discount 21%; limit message; no previous result |
| 05 Mobile | 390 × 844 | Same form, vertical layout without horizontal scrolling |

Required content: title `Discount simulator`, description that it simulates without charging/saving, labels `Order amount (R$)` and `Discount (%)`, help text `Discount allowed from 0 to 20%`. Use vertical Auto Layout and an explicit hierarchy of title, description, fields, help, button, status, and result.

Proposed tokens: background `#F1F5F7`, surface `#FFFFFF`, text `#172B36`, action `#165C72`, error `#A01919`; Inter font; spacing 8/16/24/32; radii 6/16. These are creation instructions, not values extracted from MCP. After creation, derive the tokens/geometry actually used from the capture and record deviations.

## From creation to delegation

1. Create the file through the supported connector; record the real URL and IDs for all five frames. Confirm access and content without using fake IDs.
2. In Figma desktop, open the file and enable the MCP server in Dev Mode per the [official guide](https://developers.figma.com/docs/figma-mcp-server/local-server-installation/).
3. Copy each frame link to get its node ID; capture it with the lab CLI. The client calls initialize → initialized → tools/list → metadata/design context/screenshot, per the [MCP 2025-06-18 lifecycle](https://modelcontextprotocol.io/specification/2025-06-18/basic/lifecycle).
4. Keep the original capture and screenshot. A capture proves only that frame; repeat to cover all required states.
5. TL adds the real `FIGMA` source alongside business documents to the candidate SPEC and delegates. The demo's `FIGMA-FIXTURE` remains fictional and does not close the visual gate.
6. Front Dev implements only after sufficient real context is available; Back Dev preserves Lab 07's contract and decimal rule. Review the UI against desktop/mobile screenshots and states.

## Evidence and gates

- Required delivery: file URL, node IDs, captures, design context, screenshots, and visual review linked to F-2..F-5.
- Available: 19 MCP checks with simulated transport, reported by the coordinator. They validate protocol and rejections, not real creation or access.
- Pending: real file, real capture, Front conformance, and GitHub publication.
- Do not approve visuals based on an illustrative screenshot, fixture, or mock success alone. Follow [convergence](../specs/lab-08-figma-mcp/convergence.md).
