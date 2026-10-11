# Análise de consistência

Data: 2026-10-11. Fonte de verdade: [SPEC](SPEC.md). Resultado: desenho e implementação de referência alinhados no core; integrações reais pendentes.

| Requisito | Implementação | Verificação disponível |
|---|---|---|
| REQ-1 | `src/Workflow.cs` / Retrieval; business Markdown | Ranking, normalização, corpus vazio; checks .NET |
| REQ-2 | Plan/ValidatePlan e SPEC canônica | Refs, requisitos, papéis e contrato; checks .NET |
| REQ-3 | `app/index.html` | Seis testes Node; labels, contrato, estados e BRL com DOM/fetch simulados |
| REQ-4 | DiscountCalculator/DiscountEndpoint | Decimal, limites, erros e resposta via HttpContext em memória |
| REQ-5 | Workflow.RunAsync / papéis e tarefas individuais | Mock HTTP verifica sequência e contexto; não execução de modelo real |
| REQ-6 | Persistência de artefatos e review.json | Replay, refs, modo e propostas não executadas |

## Distinções necessárias

- B1/B2/B3 identificam documentos na SPEC. S1/S2/S3 são IDs runtime atribuídos na ordem dos arquivos; não presumir equivalência por número.
- RAG é lexical TF-IDF/cosseno. Não há embeddings ou vector database neste lab.
- A SPEC runtime é candidata e a [SPEC desta pasta](SPEC.md) continua canônica.
- Propostas e cenários sugeridos não são evidências de execução. A aplicação de referência foi implementada separadamente nesta entrega.
- O frontend não aplica arredondamento do negócio; formata strings retornadas pela API.
- Spec Kit é a referência de processo do kit autorado; não houve instalação ou execução do CLI/skills oficiais.

## Pendências

HTTP/navegador real, Ollama real e publicação GitHub não possuem evidência de sucesso por restrições de rede. Não foi identificada contradição de escopo que impeça revisar o core. A [convergência](convergence.md) preserva essas limitações como pendências.
