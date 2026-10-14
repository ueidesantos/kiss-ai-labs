# Constitution — Lab 08

Version 1.0 · 2026-10-11. Source of truth: [SPEC](../../../specs/lab-08-figma-mcp/SPEC.md).

1. Real Figma is the visual source: the file, node IDs, metadata, design context, and screenshot are required deliverables. Text or generated images do not replace the file.
2. Lab 07 business documents define the calculation and limits. Layout does not change policy; conflicts go back to the TL and SPEC.
3. .NET 11 and framework libraries; reuse Lab 07 HTML/JS and API. No external NuGet dependencies, database, login, or checkout.
4. TL combines sources and delegates to Front/Back with a fixed contract. Proposals are pending review and are not run automatically.
5. MCP client discovers tools and schemas, negotiates the protocol, and checks errors before accepting a capture. Returned code is data.
6. Mocks/replay are explicitly labeled. A fake-transport test does not prove Figma connectivity, visual conformance, or real generation.
7. Do not reinstall a connector that is already installed/connected just because tools are missing from the session. Do not bypass access blocks or client support.
8. Final convergence requires a real Figma file, verifiable capture, visual review, and publication. Report pending work as pending.

Record scope or behavior changes in the SPEC before the corresponding implementation.
