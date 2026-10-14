# SPEC — MCP Spotify Server

## Purpose

Demonstrate the **Model Context Protocol (MCP)** in a minimal way with a .NET 11 MCP server that integrates with the **Spotify Web API**.

The focus is on MCP concepts (tools over JSON-RPC via stdio) without abstraction layers.

## Roles (MCP)

| Actor                  | Implementation                      |
|------------------------|--------------------------------------|
| **MCP Client**         | Any MCP client (for example, a user entering input manually via stdin) |
| **MCP Server**         | .NET 11 Console application (`McpSpotifyServer`) |
| **Spotify Web API**    | `https://api.spotify.com/v1/...`     |

## Exposed MCP Tools

Only **2 tools**, to keep the example educational:

### 1. `search_track`
Searches Spotify for tracks by keyword.

**Input:**
```json
{ "query": "nome da música ou artista" }
```

**Output (summarized JSON array):**
```json
[
  { "name": "Nome", "artist": "Artista", "album": "Álbum", "uri": "spotify:track:..." },
  ...
]
```

### 2. `get_currently_playing`
Returns the track currently playing in the user's account (requires the `user-read-currently-playing` scope).

**Input:** `{}` (empty)

**Output (JSON):**
```json
{ "is_playing": true, "name": "...", "artist": "...", "progress_ms": 12345, "duration_ms": 234000 }
```

## Spotify Authentication

- **Flow**: The access token is obtained **outside** the server (the user supplies it through an environment variable).
- **Required scopes**:
  - `search_track` → **none** (works with Client Credentials).
  - `get_currently_playing` → `user-read-currently-playing` (requires the Authorization Code flow).
- **Configuration** through environment variables:
  - `SPOTIFY_ACCESS_TOKEN` (required)
  - `SPOTIFY_CLIENT_ID` + `SPOTIFY_CLIENT_SECRET` (optional: for automatic token renewal through Client Credentials)

Credentials are never committed to version control.

## Execution Flow

```
┌─────────────┐  JSON-RPC / stdio  ┌─────────────────┐  HTTPS  ┌────────────┐
│ MCP Client  │ ──────────────────▶│  McpSpotify     │ ───────▶│ Spotify    │
│ (prompt)    │  tools/call        │  Server (.NET)  │ REST    │ Web API    │
│             │ ◀──────────────────│                 │ ◀───────│            │
└─────────────┘   JSON result      └─────────────────┘         └────────────┘
```

1. The client sends the JSON-RPC `initialize` message → the server responds with `serverInfo` + `capabilities.tools`.
2. The client sends `tools/list` → the server lists `search_track` and `get_currently_playing`.
3. The client sends `tools/call` with the tool name and arguments → the server calls Spotify → returns JSON.

## Acceptance Criteria

1. ✅ Builds with `dotnet build` with no warnings.
2. ✅ Reads `SPOTIFY_ACCESS_TOKEN` from an environment variable or User Secrets.
3. ✅ Exposes the two tools via `tools/list`.
4. ✅ `search_track` returns at least three results formatted as JSON.
5. ✅ Handles errors:
   - Missing token → clear message.
   - Expired/invalid token → reports HTTP 401 error.
   - Spotify offline → network error message.
6. ✅ Valid minimal MCP communication via stdin/stdout (JSON-RPC 2.0).
7. ✅ No third-party MCP libraries (the protocol is implemented manually for educational purposes).

## How to Run

```bash
# 1. Configurar token Spotify (PowerShell):
$env:SPOTIFY_ACCESS_TOKEN = "seu-token-aqui"

# 2. Rodar o servidor:
cd src/McpSpotifyServer
dotnet run

# 3. O servidor escuta stdin. Cole os exemplos abaixo (um JSON por linha):

# --- Exemplo 1: initialize ---
{"jsonrpc":"2.0","id":1,"method":"initialize","params":{"protocolVersion":"2024-11-05","capabilities":{},"clientInfo":{"name":"test-cli","version":"1.0"}}}

# --- Exemplo 2: listar tools ---
{"jsonrpc":"2.0","id":2,"method":"tools/list","params":{}}

# --- Exemplo 3: chamar search_track ---
{"jsonrpc":"2.0","id":3,"method":"tools/call","params":{"name":"search_track","arguments":{"query":"The Beatles"}}}

# --- Exemplo 4: música tocando agora ---
{"jsonrpc":"2.0","id":4,"method":"tools/call","params":{"name":"get_currently_playing","arguments":{}}}
```
