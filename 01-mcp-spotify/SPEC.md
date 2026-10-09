# SPEC — MCP Spotify Server

## Objetivo

Demonstrar de forma mínima o **Model Context Protocol (MCP)** com um servidor MCP em .NET 11 que integra com a **Spotify Web API**.

O foco é mostrar os conceitos MCP (tools via JSON-RPC sobre stdio) sem camadas de abstração.

## Papéis (MCP)

| Ator                   | Implementação                        |
|------------------------|--------------------------------------|
| **MCP Client**         | Qualquer cliente MCP (ex: o usuário via entrada manual / stdin) |
| **MCP Server**         | Aplicação .NET 11 Console (`McpSpotifyServer`) |
| **Spotify Web API**    | `https://api.spotify.com/v1/...`     |

## Tools MCP Expostas

Apenas **2 tools** para manter o exemplo didático:

### 1. `search_track`
Busca faixas (tracks) no Spotify por palavra-chave.

**Entrada:**
```json
{ "query": "nome da música ou artista" }
```

**Saída (JSON array resumido):**
```json
[
  { "name": "Nome", "artist": "Artista", "album": "Álbum", "uri": "spotify:track:..." },
  ...
]
```

### 2. `get_currently_playing`
Retorna a música que está tocando agora na conta do usuário (requer escopo `user-read-currently-playing`).

**Entrada:** `{}` (vazio)

**Saída (JSON):**
```json
{ "is_playing": true, "name": "...", "artist": "...", "progress_ms": 12345, "duration_ms": 234000 }
```

## Autenticação Spotify

- **Fluxo**: Token de acesso obtido **fora** do servidor (usuário provê via env var).
- **Escopos necessários**:
  - `search_track` → **nenhum** (funciona com Client Credentials).
  - `get_currently_playing` → `user-read-currently-playing` (precisa de Authorization Code Flow).
- **Configuração** via variáveis de ambiente:
  - `SPOTIFY_ACCESS_TOKEN` (obrigatório)
  - `SPOTIFY_CLIENT_ID` + `SPOTIFY_CLIENT_SECRET` (opcional: se quiser renovar token automaticamente via Client Credentials)

Credenciais nunca são versionadas.

## Fluxo de Execução

```
┌─────────────┐  JSON-RPC / stdio  ┌─────────────────┐  HTTPS  ┌────────────┐
│ MCP Client  │ ──────────────────▶│  McpSpotify     │ ───────▶│ Spotify    │
│ (prompt)    │  tools/call        │  Server (.NET)  │ REST    │ Web API    │
│             │ ◀──────────────────│                 │ ◀───────│            │
└─────────────┘   JSON result      └─────────────────┘         └────────────┘
```

1. Client envia mensagem JSON-RPC `initialize` → Server responde com `serverInfo` + `capabilities.tools`.
2. Client envia `tools/list` → Server lista `search_track` e `get_currently_playing`.
3. Client envia `tools/call` com nome da tool e argumentos → Server chama Spotify → retorna JSON.

## Critérios de Aceite

1. ✅ Compila com `dotnet build` sem warnings.
2. ✅ Lê `SPOTIFY_ACCESS_TOKEN` de variável de ambiente ou User Secrets.
3. ✅ Expõe as 2 tools via `tools/list`.
4. ✅ `search_track` retorna pelo menos 3 resultados formatados como JSON.
5. ✅ Trata erros:
   - Token ausente → mensagem clara.
   - Token expirado/inválido → informa erro HTTP 401.
   - Spotify offline → mensagem de rede.
6. ✅ Comunicação MCP mínima válida via stdin/stdout (JSON-RPC 2.0).
7. ✅ Nenhuma biblioteca MCP de terceiros (protocolo implementado manualmente para didática).

## Como Executar

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
