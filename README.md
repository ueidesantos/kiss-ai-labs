# KISS AI Labs

![Diagrama dos exemplos KISS AI Labs: Ollama e MCP Spotify](assets/ChatGPT Image Oct 9, 2026, 03_16_57 AM.png)

Pequenos experimentos em **.NET 11** para estudar:

- LLMs locais rodando via **Ollama**
- **Model Context Protocol (MCP)** com integrações externas (Spotify)
- Integração de IA com .NET sem overengineering
- **Spec-Driven Development** (cada exercício tem sua `SPEC.md`)

> **Princípio KISS**: Keep It Simple, Stupid.
> Nada de Clean Architecture, CQRS, DDD, MediatR, interfaces desnecessárias,
> bibliotecas que mascaram o que está acontecendo. O objetivo é **entender** o
> protocolo e o fluxo, não aprender um framework.

## O que tem neste repositório

| Pasta                    | Tecnologias envolvidas        | Descrição curta                              |
|--------------------------|-------------------------------|----------------------------------------------|
| `00-hello-ollama/`       | .NET 11, Ollama, `HttpClient` | App Console conversando com LLM local via HTTP direto |
| `01-mcp-spotify/`        | .NET 11, MCP, Spotify Web API | Servidor MCP mínimo com 2 tools (search + now playing) |
| `02-ai-code-reviewer/`  | .NET 11, Ollama, Git          | Revisor local de diffs que aponta possíveis bugs sem alterar arquivos |

## Como usar

Cada exercício é independente e possui:

- **`SPEC.md`** → o problema, requisitos e critérios de aceite (escrito *antes* da implementação).
- **`README.md`** → como rodar, exemplo de saída e o que aprender.
- **`src/`** → o código-fonte (.NET 11 Console).

Basta entrar na pasta do exercício, ler o SPEC (5 min), ler o README (2 min) e rodar `dotnet run`.

## Rodando os builds

```bash
cd 00-hello-ollama/src
dotnet build

cd ../../01-mcp-spotify/src/McpSpotifyServer
dotnet build

cd ../../../02-ai-code-reviewer/src
dotnet build
```

## Publicando no GitHub

```bash
git init
git add .
git commit -m "feat: add KISS Ollama and MCP Spotify labs"
git branch -M main
git remote add origin https://github.com/<SEU_USUARIO>/kiss-ai-labs.git
git push -u origin main
```

## Licença

Use como quiser para estudar.
