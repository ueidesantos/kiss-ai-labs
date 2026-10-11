# Spec Kit aplicado à demanda KISS

Este lab reúne os artefatos de SDD da demanda de desconto e seu código .NET 11. O processo foi inspirado no [Spec Kit oficial do GitHub](https://github.com/github/spec-kit), cuja documentação descreve constituição e o ciclo specify → plan → tasks → implement → converge. As invocações `speckit-*` pertencem ao chat do agent, não ao terminal.

O kit desta demanda foi escrito neste repositório. Não é uma cópia integral do upstream nem uma instalação do CLI oficial; o CLI e as skills oficiais não foram executados aqui. A aplicação e o workflow são executáveis próprios do lab.

| Etapa | Artefato desta entrega | Como usar |
|---|---|---|
| Constitution | [Princípios](.specify/memory/constitution.md) | Limitar arquitetura, delegação e alegações de evidência |
| Specify | [Demanda](DEMAND.md) e [SPEC canônica](../specs/lab-07-sdd-agents/SPEC.md) | Entender resultado, REQ-1..6 e AC-1..6 |
| Clarify | [Decisões](../specs/lab-07-sdd-agents/clarifications.md) | Resolver stack, domínio e limites antes de implementar |
| Research | [Pesquisa](../specs/lab-07-sdd-agents/research.md) | Entender escolhas e alternativas descartadas |
| Plan | [Plano](../specs/lab-07-sdd-agents/plan.md), [dados](../specs/lab-07-sdd-agents/data-model.md), [contrato](../specs/lab-07-sdd-agents/contracts/discount-api.json) | Compartilhar desenho entre TL, Front e Back |
| Tasks | [Tarefas](../specs/lab-07-sdd-agents/tasks.md) | Delegar por requisito e verificar conclusão |
| Implement | [Workflow](src/Workflow.cs), [API](app/Program.cs), [Front](app/index.html) | Entregar o incremento delimitado pela SPEC |
| Analyze/checklist | [Análise](../specs/lab-07-sdd-agents/analysis.md), [checklist](../specs/lab-07-sdd-agents/checklists/requirements.md) | Conferir coerência e rastreabilidade |
| Converge | [Convergência](../specs/lab-07-sdd-agents/convergence.md) | Distinguir core validado de integrações pendentes |

## Dois fluxos que se complementam

O kit registra o desenvolvimento da aplicação de referência: TL coordenou, Front criou interface, Back implementou cálculo e contrato, e validações fornecem evidências. No runtime, o workflow também demonstra TL → SPEC candidata → duas tarefas → propostas dos devs. O runtime usa RAG lexical sobre Markdown e faz três chamadas sequenciais ao Ollama no modo real; não inicia agentes autônomos em paralelo.

`--offline` usa respostas preparadas e arquivos de referência. É replay para inspecionar o fluxo sem modelo, não geração de IA. As propostas de ambos os modos ficam para revisão; elas não substituem a SPEC canônica nem são executadas.

## Experimentar

Siga o [quickstart](../specs/lab-07-sdd-agents/quickstart.md). O core tem 66 checks .NET e nove testes JS aprovados, informados pelo coordenador. Três projetos .NET compilaram sem warnings. HTTP real, Ollama real e publicação GitHub permanecem pendentes devido às restrições de rede desta execução.
