# 01 — MCP Spotify Server

## Purpose

A minimal **MCP Server** (Model Context Protocol) example in .NET 11 that exposes tools for interacting with the **Spotify Web API**.

The MCP protocol is implemented **manually** over JSON-RPC 2.0 via stdin/stdout for educational purposes—without third-party MCP libraries.

## Architecture

```
┌─────────────┐  JSON-RPC / stdio  ┌──────────────────┐  HTTPS   ┌──────────┐
│ MCP Client  │ ──────────────────▶│ McpSpotifyServer │ ────────▶│ Spotify  │
│ (terminal)  │  initialize        │ (.NET 11 Console)│ GET/POST │ Web API  │
│             │  tools/list        │                  │          │          │
│             │  tools/call        │                  │          │          │
│             │ ◀──────────────────│ JSON result      │ ◀────────│ JSON     │
└─────────────┘   content[].text   └──────────────────┘          └──────────┘
```

- **MCP Client**: anything that sends JSON-RPC over stdin (in this example, you type it in the terminal).
- **MCP Server**: this console app.
- **Spotify Web API**: `https://api.spotify.com/v1/...` with a bearer token.

## Prerequisites

- .NET 11 SDK
- Conta Spotify (gratuita ou Premium)
- Spotify access token with the following scope:
  - For `search_track` → **no special scope** (any token works)
  - For `get_currently_playing` → `user-read-currently-playing`

To get a token quickly, open the [Spotify Web Console](https://developer.spotify.com/console/get-search-item/) → click **Get Token** → select the scopes → copy the `OAuth Token`.

## How to Run

```bash
# 1. Entre na pasta do projeto
cd 01-mcp-spotify/src/McpSpotifyServer

# 2. Defina o token no terminal
# PowerShell:
$env:SPOTIFY_ACCESS_TOKEN = "seu-token-aqui"
# CMD:
#   set SPOTIFY_ACCESS_TOKEN=seu-token-aqui
# Bash:
#   export SPOTIFY_ACCESS_TOKEN="seu-token-aqui"

# 3. Rode o servidor
dotnet run
```

The server waits for JSON messages on stdin (one per line).

## Example

Paste **one line at a time** into the terminal after running `dotnet run`:

### Handshake + listar tools

```json
{"jsonrpc":"2.0","id":1,"method":"initialize","params":{"protocolVersion":"2024-11-05","capabilities":{},"clientInfo":{"name":"test","version":"1.0"}}}
{"jsonrpc":"2.0","id":2,"method":"tools/list","params":{}}
```

### Chamar `search_track`

```json
{"jsonrpc":"2.0","id":3,"method":"tools/call","params":{"name":"search_track","arguments":{"query":"Daft Punk"}}}
```

Expected output in `content[0].text`:
```json
[
  {"name":"Get Lucky","artist":"Daft Punk","album":"Random Access Memories","uri":"spotify:track:5VnDkUNyX6u5Sk0yZiP8XB"},
  ...
]
```

### Chamar `get_currently_playing`

```json
{"jsonrpc":"2.0","id":4,"method":"tools/call","params":{"name":"get_currently_playing","arguments":{}}}
```

## What You Will Learn

1. **MCP is just JSON-RPC over stdio**: no magic. Messages are JSON objects with `jsonrpc`, `id`, `method`, and `params`.
2. **Required flow**: `initialize` → `tools/list` → `tools/call`.
3. **Each tool has** a name, description, and `inputSchema` (JSON Schema for its arguments).
4. **Tool response**: always `{ content: [{ type: "text", text: "..." }] }` — plain text or JSON serialized as a string.
5. **Separation of responsibilities**: MCP defines only the protocol; Spotify integration uses plain HTTP.
