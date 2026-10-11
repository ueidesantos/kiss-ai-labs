# Spec Kit da demanda Figma + MCP

O lab aplica um ciclo inspirado no [Spec Kit oficial](https://github.com/github/spec-kit): constitution → specify → plan → tasks → implement → converge. O kit foi autorado para esta demanda; CLI e skills oficiais não foram instalados ou executados. As skills `speckit-*` são invocações no chat de agents, não comandos de terminal.

| Etapa | Arquivo | Resultado |
|---|---|---|
| Constitution | [Princípios](.specify/memory/constitution.md) | Figma real obrigatório, limites KISS e evidência honesta |
| Specify | [Demanda](DEMAND.md), [SPEC canônica](../specs/lab-08-figma-mcp/SPEC.md) | F-1..F-6 e gates técnico/visual |
| Clarify/research | [Decisões](../specs/lab-08-figma-mcp/clarifications.md), [pesquisa](../specs/lab-08-figma-mcp/research.md) | Separa OAuth remoto de cliente desktop |
| Plan | [Plano](../specs/lab-08-figma-mcp/plan.md), [dados](../specs/lab-08-figma-mcp/data-model.md), [contrato](../specs/lab-08-figma-mcp/contracts/design-source.json) | Captura → fonte FIGMA → SPEC candidata → devs |
| Tasks | [Tarefas](../specs/lab-08-figma-mcp/tasks.md) | Criação visual pendente preservada explicitamente |
| Implement | [Cliente .NET](src/FigmaMcpClient.cs) e CLI do lab | Integração protocolar, sem executar código recebido |
| Analyze/checklist | [Análise](../specs/lab-08-figma-mcp/analysis.md), [checklist](../specs/lab-08-figma-mcp/checklists/requirements.md) | Verificar fontes e evidências por requisito |
| Converge | [Status](../specs/lab-08-figma-mcp/convergence.md) | Core MCP testado; entrega visual ainda bloqueada |

[FIGMA.md](FIGMA.md) detalha briefing dos frames e caminho de captura. [Quickstart](../specs/lab-08-figma-mcp/quickstart.md) contém comandos .NET. A API de referência é reutilizada do lab 07 e não deve ser apresentada como implementação fiel a um Figma ainda inexistente.

A etapa visual não foi concluída: conector instalado/conectado, tools ausentes na sessão e acesso de navegador negado. Demo/replay demonstram fluxo com conteúdo fictício, não execução Figma real. Nenhuma aprovação final é inferida dos 19 checks de transporte mock.
