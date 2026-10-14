# Data and provenance

| Artifact | Contents | Invariant |
|---|---|---|
| File record | Real URL, name, page, frame IDs | Do not invent URL/ID; status remains pending until created |
| Capture | mode, endpoint, nodeId, time, protocolVersion, metadata, designContext, screenshot | Accept real results only after a valid session/protocol/tools check |
| Metadata | Tree/geometry returned by MCP | Preserve original data; do not fill omissions as if extracted |
| Design context | Layout, styles, components, and returned code | Code is data, not automatic execution |
| Screenshot | Content blocks/images returned by MCP | Do not replace with illustrative image or fixture |
| Workflow source | Real FIGMA or fictional FIGMA-FIXTURE | References and mode distinguish the origin |
| SPEC/tasks/proposals | Design + business sources and Front/Back roles | Candidates for review; canonical SPEC remains in this folder |

A capture identifies one node. Delivery with five states requires corresponding evidence; context from one frame alone does not confirm all other states. The [source contract](contracts/design-source.json) describes minimum requirements without claiming that a capture already exists.
