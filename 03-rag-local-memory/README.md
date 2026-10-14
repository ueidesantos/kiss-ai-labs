# 03 — Local RAG Without a Vector Database

![KISS AI Labs: LOCAL RAG with Markdown, embeddings, in-memory cosine similarity, and sources](../assets/kiss_ai_lab.png)

An educational RAG example: the program reads Markdown, creates embeddings with Ollama, compares vectors using in-memory cosine similarity, and passes the most relevant documents to a chat model. The prompt requests citation IDs, and the terminal lists the file and excerpt for each retrieved source.

## Prerequisites

- .NET 11 SDK
- [Ollama](https://ollama.com/) running locally
- Embedding and generation models downloaded:

```bash
ollama pull nomic-embed-text
ollama pull llama3.2
```

## Run

From the repository root:

```bash
dotnet run --project 03-rag-local-memory/src -- --question "Como protegemos credenciais?"
```

The `docs/` directory contains sample notes. To query another folder and choose different models:

```bash
dotnet run --project 03-rag-local-memory/src -- \
  --docs ./meus-documentos --question "Qual é a política de retenção?" \
  --embedding-model nomic-embed-text --model llama3.2 --top-k 2
```

Options: `--docs`, `--question`, `--url`, `--embedding-model`, `--model`, and `--top-k`. Use `--help` to see the default values.

## Flow

```text
Markdown local ──▶ Ollama /embed ──▶ embeddings em memória
                                             │
Pergunta ──▶ Ollama /embed ──▶ cosseno ──▶ Top-K documentos
                                             │
Terminal ◀── resposta + fontes [S1] ◀── Ollama /generate ◀─┘
```

This example makes the ranking step explicit, without a vector database or RAG framework. Each response is followed by the retrieved sources, their IDs, file paths, and a preview of each excerpt. Because the ID is included in the context sent to the model, verify LLM-generated citations against the source list displayed by the program. Documents stay in the local process; the question text, selected documents, and generation request go through the configured Ollama endpoint.

See [`SPEC.md`](SPEC.md) for scope and acceptance criteria.
