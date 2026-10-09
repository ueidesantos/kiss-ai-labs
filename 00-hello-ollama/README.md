# 00 — Hello Ollama

## Objetivo

O exemplo mais simples possível de uma aplicação .NET 11 conversando com um LLM executado localmente via **Ollama** usando apenas `HttpClient` nativo.

## Arquitetura

```
┌────────────────┐     HTTP      ┌──────────────┐     local      ┌───────────┐
│ Console .NET 11│ ─────────────▶│   Ollama     │ ──────────────▶│  LLM      │
│ (Program.cs)   │◀──────────────│ (localhost)  │◀──────────────│ (qwen etc)│
└────────────────┘               └──────────────┘                └───────────┘
```

Nenhuma biblioteca extra. Apenas uma requisição POST para `/api/generate` com streaming.

## Pré-requisitos

- [.NET 11 SDK](https://dotnet.microsoft.com/)
- [Ollama](https://ollama.com/) instalado e rodando
- Pelo menos 1 modelo baixado (ex: `ollama pull llama3.2`)

## Como executar

```bash
cd 00-hello-ollama/src

# Com valores default (modelo llama3.2, prompt "Olá!")
dotnet run

# Com modelo e prompt customizados
dotnet run -- --model qwen3 --prompt "Hello from .NET"

# Endpoint alternativo
dotnet run -- --url http://192.168.0.10:11434
```

## Exemplo

```
> Modelo  : qwen3
> Prompt  : Hello from .NET
> Endpoint: http://localhost:11434
--------------------------------------------------
Resposta: Hello! It's great to hear from .NET developers. How can I help you today?
--------------------------------------------------
Concluído com sucesso.
```

## O que aprender

1. **Ollama expõe uma API REST simples** na porta 11434 — qualquer cliente HTTP serve.
2. **Não precisa de SDK**: `HttpClient` do .NET é suficiente para integrar com LLMs locais.
3. **Streaming de tokens**: o endpoint `/api/generate` entrega a resposta pedaço por pedaço via NDJSON.
4. **Mensagens de erro didáticas** fazem toda diferença em um exemplo educacional.
