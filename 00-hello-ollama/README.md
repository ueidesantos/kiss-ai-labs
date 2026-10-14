# 00 — Hello Ollama

## Purpose

The simplest possible example of a .NET 11 application talking to an LLM running locally through **Ollama**, using only the built-in `HttpClient`.

## Architecture

```
┌────────────────┐     HTTP      ┌──────────────┐     local      ┌───────────┐
│ Console .NET 11│ ─────────────▶│   Ollama     │ ──────────────▶│  LLM      │
│ (Program.cs)   │◀──────────────│ (localhost)  │◀──────────────│ (qwen etc)│
└────────────────┘               └──────────────┘                └───────────┘
```

No extra libraries. Just a streaming POST request to `/api/generate`.

## Prerequisites

- [.NET 11 SDK](https://dotnet.microsoft.com/)
- [Ollama](https://ollama.com/) installed and running
- At least one model downloaded (for example, `ollama pull llama3.2`)

## How to Run

```bash
cd 00-hello-ollama/src

# Com valores default (modelo llama3.2, prompt "Olá!")
dotnet run

# Com modelo e prompt customizados
dotnet run -- --model qwen3 --prompt "Hello from .NET"

# Endpoint alternativo
dotnet run -- --url http://192.168.0.10:11434
```

## Example

```
> Modelo  : qwen3
> Prompt  : Hello from .NET
> Endpoint: http://localhost:11434
--------------------------------------------------
Resposta: Hello! It's great to hear from .NET developers. How can I help you today?
--------------------------------------------------
Concluído com sucesso.
```

## What You Will Learn

1. **Ollama exposes a simple REST API** on port 11434—any HTTP client will work.
2. **No SDK is needed**: .NET `HttpClient` is enough to integrate with local LLMs.
3. **Token streaming**: the `/api/generate` endpoint returns the response in chunks via NDJSON.
4. **Helpful error messages** make a big difference in an educational example.
