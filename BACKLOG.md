# Backlog — KISS AI Labs

Trilha atual: recuperação semântica em memória, fontes, comparação de Top-K, consulta a código-fonte e persistência local.

| Ordem | Laboratório | Stack | Resultado esperado | Dependência | Estado |
|---:|---|---|---|---|---|
| 1 | **RAG Local sem Vector Database** | C#, .NET 11, Ollama, embeddings | Perguntar sobre Markdown; gerar embeddings, ranquear por cosseno em memória e responder com contexto recuperado. | — | Concluído em `03-rag-local-memory/` |
| 2 | **RAG com Citação da Fonte** | C#, .NET 11, Ollama, embeddings | Exibir arquivo e trecho usados para responder. | 1 | Incorporado ao `03-rag-local-memory/` |
| 3 | **RAG com Top-K Configurável** | C#, .NET 11, Ollama | Variar a quantidade de trechos recuperados e observar contexto, precisão e ruído. | 1 | Concluído em `04-rag-top-k/` |
| 4 | **RAG sobre Código-Fonte** | C#, .NET 11, Ollama, embeddings | Indexar `.cs` e localizar regras, classes e validações. | 1, 2 | Concluído em `05-rag-source-code/` |
| 5 | **RAG Local com SQLite-vec** | Python, Ollama, SQLite-vec | Persistir embeddings localmente para indexação reproduzível sem serviço externo. | Conceitos do 1 | Implementado em `06-rag-sqlite-vec/`; validação nativa e publicação pendentes |

## SDD, agents e design

Adicionados por solicitação direta do usuário. Os itens retirados do roadmap permanecem fora da trilha.

| Lab | Entrega | Estado |
|---|---|---|
| 07 | SDD + agents + RAG de negócio; kit completo e simulador .NET 11 | Núcleo/testes aprovados; integração real e publicação pendentes |
| 08 | Especificação pelo Figma via MCP + TL/Front/Back + RAG | Cliente e replay implementados; link Figma entregue como alternativa aceita; captura MCP e revisão visual não validadas |

| 09 | KISS AI Squad: SDD + RAG + desenvolvimento e revisão por modelos distintos | Implementado; evidências e limites em [VALIDATION.md](09-ai-squad/VALIDATION.md) |

## Critérios comuns de conclusão

- Cada laboratório tem `SPEC.md`, instruções para executar e uma pergunta de demonstração.
- Endpoint e modelos podem ser configurados; os pré-requisitos e falhas comuns são descritos.
- O exemplo explica o que fica local e quais dados são enviados ao modelo configurado.
- O código mantém o fluxo explícito e evita abstrações não necessárias ao conceito ensinado.

## Entrega pendente de validação/publicação

O lab 06 já tem código, documentação e testes preparados. Conclua os testes com SQLite-vec instalado e a demo com Ollama antes de declarar esse lab concluído. Consulte `06-rag-sqlite-vec/VALIDATION.md`.
