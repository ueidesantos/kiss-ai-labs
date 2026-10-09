# Backlog — KISS AI Labs

Sequência sugerida: começar com o menor fluxo que torne a recuperação semântica observável e adicionar persistência, fontes e filtros depois.

| Ordem | Laboratório | Stack | Resultado esperado | Dependência |
|---:|---|---|---|---|
| 1 | **RAG Local sem Vector Database** | C#, .NET 11, Ollama, embeddings | Perguntar sobre Markdown; gerar embeddings, ranquear por cosseno em memória e responder com contexto recuperado. | — |
| 2 | **RAG com Citação da Fonte** | C#, .NET 11, Ollama, embeddings | Exibir arquivo e trecho usados para responder. | Incorporado ao `03-rag-local-memory/` |
| 3 | **RAG com Top-K Configurável** | C#, .NET 11, Ollama | Variar a quantidade de trechos recuperados e observar contexto, precisão e ruído. | 1 |
| 4 | **RAG sobre Código-Fonte** | C#, .NET 11, Ollama, embeddings | Indexar `.cs` e localizar regras, classes e validações. | 1, 2 |
| 5 | **RAG Local com SQLite-vec** | Python, Ollama, SQLite-vec | Persistir embeddings localmente para indexação reproduzível sem serviço externo. | Conceitos do 1 |
| 6 | **RAG com Chunking KISS** | Python, Ollama, NumPy | Comparar documento inteiro e divisão por tamanho na recuperação. | Conceitos do 1 |
| 7 | **RAG com Filtro de Similaridade** | Python, Ollama, embeddings | Descartar contexto abaixo de um limiar antes da geração. | Conceitos do 1 |
| 8 | **RAG Local sobre PDFs** | Python, Ollama, PyMuPDF | Extrair texto local, criar chunks e consultar sem enviar arquivos a serviços externos. | 6, 7 |
| 9 | **RAG Local com .NET** | C#, .NET 11, Ollama, Microsoft.Extensions.AI | Reimplementar consultas a Markdown usando as abstrações de IA do ecossistema .NET. | 1, 2 |

## Critérios comuns de conclusão

- Cada laboratório tem `SPEC.md`, instruções para executar e uma pergunta de demonstração.
- Endpoint e modelos podem ser configurados; os pré-requisitos e falhas comuns são descritos.
- O exemplo explica o que fica local e quais dados são enviados ao modelo configurado.
- O código mantém o fluxo explícito e evita abstrações não necessárias ao conceito ensinado.
