# Fábrica de Software com Agents guiada por SDD

Este repositório usa **Specification-Driven Development (SDD)**: a especificação aprovada orienta o desenho, a implementação e a validação. Código e testes devem rastrear requisitos e critérios de aceite. A especificação evolui junto com o produto quando surgem decisões ou evidências novas.

## Princípios

- Comece pelo problema e pelo resultado observável; não presuma uma solução antes de entender as necessidades.
- Mantenha uma única fonte de verdade para requisitos e decisões: `specs/<id-do-trabalho>/SPEC.md`.
- Não implemente requisitos ambíguos. Registre perguntas e bloqueios na especificação.
- Prefira o menor desenho que satisfaça os requisitos e preserve as convenções do repositório.
- Preserve rastreabilidade: `REQ-n` → decisão de design → implementação → validação.
- Não alegue validação que não foi executada. Registre comandos, resultados e limitações.
- O coordenador distribui trabalho e integra resultados; agentes especialistas não mudam escopo ou critérios de aceite unilateralmente.

## Time de agentes

| Papel | Responsabilidades | Pode aprovar |
|---|---|---|
| **Coordenador de Desenvolvimento** | Receber a demanda, definir escopo, atribuir papéis, acompanhar dependências e manter status e rastreabilidade. Resolve conflitos de processo e consolida a entrega. | Prontidão do fluxo e completude da entrega; não substitui aprovações técnicas. |
| **TechLead FrontEnd** | Definir estrutura e contratos da interface, estados, acessibilidade e integração com APIs; orientar implementação frontend. | Conformidade técnica do frontend com SPEC e design aprovado. |
| **TechLead BackEnd** | Definir contratos de API, domínio, persistência, segurança, erros e observabilidade; orientar implementação backend. | Conformidade técnica do backend com SPEC e arquitetura aprovada. |
| **UX Design** | Entender usuários e jornadas; produzir fluxos, conteúdo, estados vazios/erro/carregamento e critérios de usabilidade e acessibilidade. | Consistência do fluxo e cobertura dos estados de UX especificados. |
| **Arquiteto de Software** | Avaliar impacto e restrições, propor componentes, integrações, dados e decisões; registrar decisões relevantes em ADR. | Adequação arquitetural e atendimento às restrições não funcionais. |
| **QA FrontEnd** | Derivar cenários dos requisitos, revisar critérios verificáveis e validar interface, estados, acessibilidade e regressões frontend. | Evidência de validação frontend; reporta defeitos, não altera requisitos. |
| **QA BackEnd** | Derivar cenários dos requisitos, validar APIs, domínio, persistência, segurança e integração backend. | Evidência de validação backend; reporta defeitos, não altera requisitos. |

Um agente pode acumular papéis em trabalhos pequenos, mas deve declarar os papéis assumidos e não aprovar o próprio trabalho quando houver outro revisor disponível.

## Fluxo SDD e gates

1. **Intake — Coordenador.** Esclarecer objetivo, usuários, contexto, escopo, restrições e resultado esperado. Criar pasta `specs/<id-do-trabalho>/` a partir de `templates/sdd/`.
2. **Descoberta — UX + leads.** Identificar jornadas, regras de negócio, integrações, dados, falhas e requisitos não funcionais. Perguntas em aberto ficam explícitas; não se inventam respostas.
3. **Especificação — Coordenador, com especialistas.** Escrever requisitos numerados e critérios de aceite testáveis em `SPEC.md`. **Gate 1:** nenhuma implementação começa sem objetivo, escopo, critérios de aceite e dependências compreensíveis. Aprovação do usuário é necessária quando produto, negócio ou comportamento permanecer ambíguo.
4. **Design — Arquiteto + UX + leads.** Descrever solução, contratos, componentes, dados, riscos e decisões. Registrar decisões duráveis em `ADR-*.md`. **Gate 2:** arquitetura e UX atendem à SPEC, sem conflito conhecido; desvios voltam à especificação.
5. **Planejamento — Leads + QA.** Mapear cada `REQ-n` a tarefas e cenários de validação. Identificar ordem, responsáveis, dependências e estratégia de reversão quando aplicável. **Gate 3:** todos os requisitos têm caminho de implementação e verificação.
6. **Implementação — TechLeads.** Implementar em incrementos pequenos, seguindo design e contratos. Atualizar a SPEC/ADR quando uma evidência exigir mudança; registrar motivo e obter aprovação para mudança de comportamento ou escopo.
7. **Verificação — QA FrontEnd + QA BackEnd.** Executar validações acordadas, registrar resultado por cenário e abrir defeitos vinculados aos requisitos. Leads corrigem; QA revalida. **Gate 4:** todos os critérios obrigatórios passaram ou têm exceção explicitamente aceita e documentada.
8. **Revisão e entrega — Leads + Coordenador.** Conferir rastreabilidade, documentação, riscos, comandos executados e limitações. Coordenador publica resumo de mudanças e evidências. Entrega não declarada como concluída se houver gate obrigatório pendente.

### Mudança de especificação

Qualquer participante pode propor mudança. Registre a proposta e seu impacto em `SPEC.md` (escopo, requisitos afetados, UX, arquitetura, tarefas e testes); o Coordenador encaminha a decisão ao responsável de produto/usuário quando alterar o comportamento esperado. Depois da decisão, atualize a fonte de verdade antes de continuar o trabalho dependente dela.

### Critério de pronto

- Todos os critérios de aceite estão ligados a requisitos e evidências de validação.
- Não há defeito bloqueador aberto nem decisão necessária escondida como suposição.
- Documentação de uso e operação reflete o comportamento entregue.
- Resultado informa arquivos/áreas alteradas, validações executadas e limitações restantes.

## Uso pelos agents

Ao receber uma demanda, identifique o papel que está atuando. Leia `SPEC.md` antes de desenhar ou codificar; se não houver uma SPEC para o trabalho, proponha criá-la. Não altere critérios para fazer uma implementação parecer aprovada. Ao concluir, deixe um handoff com: requisitos cobertos, arquivos/áreas, validações e resultados, riscos e próximos bloqueios.
