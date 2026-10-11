# Lab 09 — Da mudança de negócio ao patch revisável

**Estado: proposta para discussão; não implementada, não aprovada como SPEC e não adicionada ao backlog.**

Papel desta análise: subagent de descoberta, acumulando arquitetura e produto. Base: código/documentação dos labs 07 e 08, suas SPECs e o BACKLOG. O objetivo é evoluir a utilidade no trabalho do desenvolvedor sem transformar o laboratório em uma plataforma.

## Proposta principal

**Nome sugerido: SDD Change Loop — uma regra mudou, o que precisa mudar no produto?**

Demanda fictícia: o simulador de descontos passa a distinguir cliente **Padrão** e **VIP**. Padrão aceita até 10%; VIP aceita até 20%. A tela ganha a escolha de perfil, a ajuda do limite e os estados de erro correspondentes. Valor, arredondamento e ausência de cobrança continuam como no lab 07. O usuário informa o perfil; isso é uma simulação, sem autenticação ou comprovação de elegibilidade.

Exemplo observável: R$100 com 15% é recusado para Padrão e resulta em R$85 para VIP. R$100 com 10% continua resultando em R$90 para ambos. Essas regras são propostas para o novo exercício, não uma alteração autorizada dos labs anteriores.

O diferencial didático é partir de um produto existente e de uma mudança pequena. O TL deve identificar o impacto, atualizar a SPEC candidata, delegar apenas o necessário, receber propostas, revisar testes e produzir um pacote de mudança rastreável. Isso se aproxima do trabalho diário de manutenção de uma aplicação.

## Evolução em relação aos labs anteriores

| Hoje | Próximo passo proposto |
|---|---|
| Lab 07 gera propostas a partir de uma demanda inicial | Comparar regra anterior e nova, localizar requisitos/arquivos afetados e propor uma alteração delimitada |
| Lab 08 acrescenta contexto visual do Figma | Vincular cada alteração de tela ao frame/estado de origem e registrar o que não foi obtido |
| Referências apontam para documentos recuperados | Guardar caminho, trecho e hash do conteúdo usado para reproduzir a análise |
| Front e Back recebem uma tarefa cada | Recebem também a lista explícita de arquivos permitidos e o contrato alterado |
| Propostas exigem revisão | Entrega inclui parecer separado, critérios por requisito e lacunas verificáveis |

Não é necessário acrescentar microserviços, filas, banco vetorial, Kubernetes ou um framework de agents. A dificuldade nova fica na gestão da mudança, nos contratos e na evidência.

## Fluxo KISS

1. Receber `change.md`: comportamento anterior, comportamento desejado e exemplos.
2. Recuperar os documentos vigentes de negócio e o contexto visual disponível; registrar origem e hash. Documentos antigos ficam fora da coleção vigente.
3. TL emitir `impact.json`: requisitos afetados, alterações de contrato, arquivos previstos, conflitos e perguntas.
4. Preparar `SPEC.candidate.md`, exemplos de aceite e tarefas Front/Back. A SPEC canônica só muda depois da revisão da proposta.
5. Dev Back propor validação por perfil; Dev Front propor seletor, ajuda e tratamento de erros conforme a especificação visual disponível.
6. Revisor conferir contrato, fontes, escopo e evidências; registrar `accepted`, `changes-requested` ou `blocked`, sem confundir parecer de IA com teste executado.
7. Gerar um resumo de entrega com mudança observável, diff, testes executados e limitações.

## Stack e tamanho mínimo

- .NET 11 / ASP.NET Core / C#; HTML, CSS e JavaScript simples.
- RAG lexical em memória reutilizando o conceito do lab 07; três ou quatro documentos Markdown bastam.
- Ollama opcional para TL, Front, Back e revisor; chamadas explícitas, sem framework de orquestração.
- Cliente Figma MCP do lab 08 quando houver acesso; manifesto e link preservados quando não houver.
- Arquivos JSON e Markdown para tarefas, fontes, propostas e evidências. Sem persistência de pedidos.
- Um projeto de aplicação, um pequeno orquestrador e um projeto de testes. Reutilizar componentes existentes somente onde isso reduzir duplicação sem acoplar o exercício a mudanças futuras.

O modo offline usa um cenário preparado e declara essa condição. Ele permite estudar o fluxo e executar a aplicação de referência; não comprova uso de IA, acesso ao Figma ou fidelidade visual.

## Contrato proposto para a nova aplicação

`POST /api/discount` recebe `amount`, `discount` e `customerType` (`standard` ou `vip`). O perfil é obrigatório neste novo lab; ausência e valores desconhecidos retornam erro de validação. O lab 07 mantém seu contrato original.

O sucesso mantém os valores monetários como texto decimal e pode acrescentar `customerType` e `maxDiscount` para explicar o cálculo. O backend é a autoridade sobre o limite; a ajuda no frontend não substitui a validação. O contrato definitivo deve constar da SPEC antes de implementar.

## Critérios de aceite propostos

| ID | Evidência esperada |
|---|---|
| AC-01 | Para 100/10%, Padrão e VIP retornam 90.00; Padrão/15% falha; VIP/15% retorna 85.00; VIP/21% falha |
| AC-02 | Perfil ausente/desconhecido, valor não positivo e desconto negativo são recusados; arredondamento decimal preserva as regras do lab 07 |
| AC-03 | A tela tem perfil rotulado, limite correspondente, estado carregando, resultado e erro; troca de perfil remove resultado desatualizado |
| AC-04 | O relatório de impacto relaciona regra → requisito → contrato → tarefa → arquivos → teste, com trechos de origem inspecionáveis |
| AC-05 | Front não recebe permissão para mudar o cálculo; Back não muda o design. Alteração fora da lista de arquivos fica sinalizada para revisão |
| AC-06 | Duas fontes vigentes com limites contraditórios geram bloqueio explícito; a similaridade de busca não escolhe qual regra é verdadeira |
| AC-07 | Evidências distinguem teste executado, cenário sugerido e revisão pendente; falha de teste não pode ser convertida em aprovação pelo modelo |
| AC-08 | Quando Figma MCP não estiver acessível, o link aparece e a revisão visual permanece pendente; nenhum frame, captura ou validação é inventado |
| AC-09 | A demonstração offline funciona sem Figma/Ollama e identifica respostas preparadas; no modo real, registra quais papéis realmente chamaram o modelo |

## Três níveis de complexidade

| Nível | Entrega | Limite para continuar KISS |
|---|---|---|
| **1 — Impacto e aplicação de referência** | Mudança por perfil funcionando; recuperação de fontes; relatório de impacto; SPEC candidata; tarefas; replay e testes de domínio/contrato | Uma única demanda, um contrato e uma tela; propostas não são aplicadas automaticamente |
| **2 — Agents com revisão e correção limitada** | TL/Front/Back/revisor reais; propostas como diffs; validação do escopo; uma rodada de correção após parecer; comparação de evidências antes/depois | No máximo uma correção por execução; timeout e limite de chamadas explícitos; falha persistente termina com diagnóstico |
| **3 — Detecção de mudanças e regressão** | Comparar hashes de negócio e captura Figma entre duas execuções; invalidar evidências ligadas às fontes alteradas; selecionar cenários afetados e executar a suíte completa antes de concluir | Comparação de snapshots locais, sem monitoramento contínuo; mudanças visuais exigem revisão apropriada, não apenas hashes iguais |

**Recomendação:** implementar primeiro o nível 1 e um caminho real simples do nível 2. O nível 3 é uma evolução posterior. Assim o lab demonstra aplicação funcionando e colaboração real quando o modelo estiver disponível, mantendo uma reprodução offline clara.

Execução automática de código proposto acrescenta uma fronteira importante: testes locais não isolam código arbitrário. No nível 1, a referência é implementada e revisada separadamente; os agents geram propostas. Para aplicar e executar propostas no nível 2, definir ambiente de execução isolado e autorização explícita no contrato do fluxo antes de implementar essa parte. Um checkout separado, sozinho, não é isolamento de execução.

## Figma e Spec Kit na entrega futura

O design proposto pede frames para Padrão, VIP, carregamento, resultado, erro de limite e mobile. O arquivo deve representar o exercício escolhido; o link atual do lab 08 pode ser referência de contexto, mas não prova que esses novos estados existam.

A entrega inclui constituição, pesquisa, esclarecimentos, SPEC canônica, plano, modelo de dados, contrato, tarefas, cenários, quickstart e relatório de convergência. O kit deve explicar a mudança entre versões e os papéis dos agents. Declarar separadamente se o CLI oficial Spec Kit foi utilizado ou se os documentos foram autorados seguindo o processo.

## Duas alternativas

### A. Corretor de divergências entre Figma, regra e aplicação

Demanda: a tela diz "até 20%", mas a política vigente permite 10% para Padrão. RAG recupera a regra, MCP obtém a tela, TL aponta o conflito e delega correção de mensagem/validação. O resultado central é um relatório reproduzível de divergências com links e casos de teste. Menor escopo de produto e maior dependência de acesso real ao design; não deve presumir que texto extraído prova fidelidade visual.

### B. Solicitação de exceção de desconto

Demanda: descontos acima de 10% e até 20% exigem uma justificativa antes de simular. Front mostra um campo condicional; Back valida justificativa; RAG explica o motivo com fonte; TL mantém contrato e testes sincronizados. Evolui para aprovação humana e estados pendente/aprovado/rejeitado em uma etapa posterior. É mais rica em fluxo de produto, mas persistência, concorrência e identidade devem ficar fora da primeira versão.

## Próxima decisão

Escolher a demanda e o nível antes de criar `specs/lab-09-.../SPEC.md`. Esta análise não inicia outro laboratório, não modifica os labs existentes e não declara cumpridas as integrações pendentes dos labs 07/08.
