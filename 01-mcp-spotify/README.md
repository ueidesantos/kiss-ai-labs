# 01 — MCP Spotify Server

## Objetivo

Exemplo mínimo de um **MCP Server** (Model Context Protocol) em .NET 11 que expõe ferramentas (tools) para interagir com a **Spotify Web API**.

O protocolo MCP é implementado **manualmente** sobre JSON-RPC 2.0 via stdin/stdout para fins didáticos — sem bibliotecas MCP de terceiros.

## Arquitetura

```
┌─────────────┐  JSON-RPC / stdio  ┌──────────────────┐  HTTPS   ┌──────────┐
│ MCP Client  │ ──────────────────▶│ McpSpotifyServer │ ────────▶│ Spotify  │
│ (terminal)  │  initialize        │ (.NET 11 Console)│ GET/POST │ Web API  │
│             │  tools/list        │                  │          │          │
│             │  tools/call        │                  │          │          │
│             │ ◀──────────────────│ JSON result      │ ◀────────│ JSON     │
└─────────────┘   content[].text   └──────────────────┘          └──────────┘
```

- **MCP Client**: qualquer coisa que envie JSON-RPC pelo stdin (no exemplo: você digitando no terminal).
- **MCP Server**: este projeto Console.
- **Spotify Web API**: `https://api.spotify.com/v1/...` com Bearer Token.

## Pré-requisitos

- .NET 11 SDK
- Conta Spotify (gratuita ou Premium)
- Token de acesso Spotify com escopo:
  - Para `search_track` → **nenhum escopo especial** (qualquer token serve)
  - Para `get_currently_playing` → `user-read-currently-playing`

Como obter token rapidamente: acesse o [Spotify Web Console](https://developer.spotify.com/console/get-search-item/) → clique **Get Token** → selecione os escopos → copie o `OAuth Token`.

## Como executar

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

O servidor vai ficar esperando mensagens JSON no stdin (uma por linha).

## Exemplo

Cole **uma linha de cada vez** no terminal após `dotnet run`:

### Handshake + listar tools

```json
{"jsonrpc":"2.0","id":1,"method":"initialize","params":{"protocolVersion":"2024-11-05","capabilities":{},"clientInfo":{"name":"test","version":"1.0"}}}
{"jsonrpc":"2.0","id":2,"method":"tools/list","params":{}}
```

### Chamar `search_track`

```json
{"jsonrpc":"2.0","id":3,"method":"tools/call","params":{"name":"search_track","arguments":{"query":"Daft Punk"}}}
```

Saída esperada no `content[0].text`:
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

## O que aprender

1. **MCP é só JSON-RPC sobre stdio**: nenhuma mágica. Mensagens são JSONs com `jsonrpc`, `id`, `method`, `params`.
2. **Fluxo obrigatório**: `initialize` → `tools/list` → `tools/call`.
3. **Cada tool tem**: nome, descrição e `inputSchema` (JSON Schema dos argumentos).
4. **Resposta de tool**: sempre `{ content: [{ type: "text", text: "..." }] }` — texto ou JSON serializado em string.
5. **Separação de responsabilidades**: MCP só define o protocolo; a lógica de integração com Spotify é HTTP puro.
