# Runbook

Core prerequisites: .NET 11 SDK. Real capture: Figma desktop with the real file open, Dev Mode/MCP enabled, and authorized client compatibility. Model-generated SPEC: Ollama and a local model. The official Spec Kit CLI is not required.

From the repository root:

```powershell
dotnet build 08-sdd-figma-mcp/src/FigmaMcp.csproj
dotnet run --project 08-sdd-figma-mcp/tests
```

The coordinator reported 19 passing mock checks. These run without sockets; they do not verify a real connection.

## Capture a real node

First create the file described in [FIGMA.md](../../08-sdd-figma-mcp/FIGMA.md), record the real URL and node IDs, and enable the desktop server per the [Figma guide](https://developers.figma.com/docs/figma-mcp-server/local-server-installation/).

The CLI integrated by the coordinator uses these commands; confirm `--help` and the resulting files before running them:

```powershell
dotnet run --project 08-sdd-figma-mcp/src -- capture --node NODE --out ./capture
dotnet run --project 08-sdd-figma-mcp/src -- spec --capture ./capture/capture.json --out ./output
dotnet run --project 08-sdd-figma-mcp/src -- demo
```

Replace `NODE` with a real ID; do not use the literal word as the ID. The second operation uses the capture as the design source and Ollama in real mode; `--offline`, if used, explicitly means replay. The demo uses a fictional fixture and does not present itself as real Figma. Capture each state separately instead of assuming one frame covers them all.

Then check the mode, endpoint, node, time, context, metadata, and screenshot in the capture. Review the SPEC/tasks/proposals and reject invented details that were missing. Do not run proposals automatically.

## Functional reference application

```powershell
dotnet run --project 07-sdd-agents-business-rag/app
```

Open `http://127.0.0.1:8087` when the environment allows it. This API/UI demonstrates Lab 07 functionality; fidelity to Lab 08's Figma remains pending. After real capture, verify 100/10→90, error above 20, and mobile behavior.

In this run, real creation/capture was blocked because Figma tools were not exposed and browser navigation was denied. There is no real file to use in this runbook and no evidence of successful HTTP/Ollama/Figma/GitHub access.
