# O kit SDD deste laboratório

Este kit registra uma demanda completa: intenção, requisitos, decisões, contratos, tarefas e critérios de verificação. Ele usa a organização de trabalho apresentada pelo [Spec Kit do GitHub](https://github.com/github/spec-kit), sem afirmar que o CLI oficial foi instalado ou executado. Os executáveis são próprios do lab.

| Etapa | Artefato | Pergunta respondida |
|---|---|---|
| Constitution | [Constituição](.specify/memory/constitution.md) | Quais limites não podem ser negociados pelos agentes? |
| Specify | [Demanda](DEMAND.md), [SPEC](../specs/lab-09-ai-squad/SPEC.md) | Qual comportamento precisa mudar? |
| Clarify | [Esclarecimentos](../specs/lab-09-ai-squad/clarifications.md) | O que significam perfil, correção e entrega? |
| Research | [Decisões](../specs/lab-09-ai-squad/research.md) | Por que estas escolhas mantêm o lab pequeno? |
| Plan | [Plano](../specs/lab-09-ai-squad/plan.md), [dados](../specs/lab-09-ai-squad/data-model.md), [contrato](../specs/lab-09-ai-squad/contracts/business-rules.json) | Como as partes cooperam? |
| Tasks | [Tarefas](../specs/lab-09-ai-squad/tasks.md) | Quem implementa e quem verifica cada requisito? |
| Implement | [Quickstart](../specs/lab-09-ai-squad/quickstart.md) | Como executar a aplicação e o ciclo? |
| Analyze | [Análise](../specs/lab-09-ai-squad/analysis.md), [checklist](../specs/lab-09-ai-squad/checklists/requirements.md) | Há lacunas entre requisito, implementação e evidência? |
| Converge | [Convergência](../specs/lab-09-ai-squad/convergence.md) | O que foi comprovado e o que continua pendente? |

## Duas especificações com papéis diferentes

A SPEC canônica foi definida antes da implementação e é a fonte de aceite. A SPEC candidata é um artefato de cada execução: mostra como o TL interpretou a demanda com o contexto recuperado. Ela é revisável e nunca substitui a canônica por conta própria.

## Delegar não é perder o controle

O TL planeja; Front propõe textos; Back propõe limites; QA comenta riscos e resultados. Os contratos reduzem a liberdade de cada papel. A aplicação nasce de templates conhecidos e dados validados. Este recorte permite observar delegação e correção sem transformar o lab em um agente de programação irrestrito.

No modo offline as propostas são preparadas. O erro inicial de limite é deliberado, mas a reprovação vem da execução real dos testes. No modo Ollama, as propostas vêm do modelo; uma correção só acontece se o gate identificar falha.
