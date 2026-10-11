# Plano técnico

[SPEC](SPEC.md) e [constituição](../../08-sdd-figma-mcp/.specify/memory/constitution.md) orientam a entrega.

1. Criar arquivo real autorizado; registrar URL e cinco node IDs.
2. Capturar nodes no desktop MCP: initialize, initialized, tools/list, tools/call para metadata/context/screenshot. Validar schemas, IDs, versão, sessão, erro e tamanho.
3. Persistir capture identificado como real, endpoint/node/time/resultados; preservar screenshot retornado.
4. TL combina `FIGMA` com RAG lexical do negócio do lab 07 e produz SPEC candidata/contrato/tarefas.
5. Front recebe contexto real e screenshot; Back mantém cálculo decimal/contrato. Código proposto não é executado automaticamente.
6. Revisar estados/geometry/tokens em desktop/mobile; registrar evidências e convergência.

Componentes: `src/FigmaMcpClient.cs`, CLI de captura/geração/demo, workflow do lab 07, API de referência reutilizada e testes MCP sem sockets.

Dependência crítica: captura não cria arquivo Figma. A criação depende de ferramentas suportadas do conector; a autenticação remote não habilita automaticamente o endpoint desktop.

| Responsável | Requisitos | Entrega |
|---|---|---|
| TL | F-1..F-4, F-6 | Captura, integração de fontes, delegação e evidências |
| Design/Front | F-2, F-5 | Arquivo real, estados e UI fiel após captura |
| Back | F-4 | Preservação API/regra lab 07 |
| Revisão | todos | Checklist e comparação visual |

Se houver discrepância de design/regra, não resolver silenciosamente: atualizar a SPEC com decisão. Descartar propostas não incorporadas é reversível; revisão de código incorporado usa Git.
