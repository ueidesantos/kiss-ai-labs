# 03 — RAG Local sem Vector Database

Um RAG didático: o programa lê Markdown, cria embeddings com Ollama, compara vetores usando similaridade de cosseno em memória e passa os documentos mais relevantes a um modelo de chat. A resposta pede IDs de citação e o terminal lista arquivo e trecho de cada fonte recuperada.

## Pré-requisitos

- .NET 11 SDK
- [Ollama](https://ollama.com/) rodando localmente
- Modelos de embedding e geração baixados:

```bash
ollama pull nomic-embed-text
ollama pull llama3.2
```

## Executar

Na raiz do repositório:

```bash
dotnet run --project 03-rag-local-memory/src -- --question "Como protegemos credenciais?"
```

O diretório `docs/` contém notas de exemplo. Para consultar outra pasta e ajustar modelos:

```bash
dotnet run --project 03-rag-local-memory/src -- \
  --docs ./meus-documentos --question "Qual é a política de retenção?" \
  --embedding-model nomic-embed-text --model llama3.2 --top-k 2
```

Opções: `--docs`, `--question`, `--url`, `--embedding-model`, `--model` e `--top-k`. Use `--help` para consultar os valores padrão.

## Fluxo

```text
Markdown local ──▶ Ollama /embed ──▶ embeddings em memória
                                             │
Pergunta ──▶ Ollama /embed ──▶ cosseno ──▶ Top-K documentos
                                             │
Terminal ◀── resposta + fontes [S1] ◀── Ollama /generate ◀─┘
```

O exemplo mantém explícita a etapa de ranking, sem banco vetorial ou framework de RAG. Cada resposta termina com as fontes recuperadas, seus IDs, o caminho do arquivo e uma prévia do trecho. Como o ID é incluído no contexto enviado ao modelo, a citação textual gerada pelo LLM deve ser conferida com a lista de fontes exibida pelo programa. Os documentos ficam no processo local; texto da pergunta, documentos selecionados e geração passam pelo endpoint Ollama configurado.

Veja [`SPEC.md`](SPEC.md) para o escopo e os critérios de aceite.
