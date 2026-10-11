# Lab 09 — KISS AI Squad

Demanda aprovada: clientes Padrão podem simular até 10% de desconto; VIP até 20%. Valor positivo, dinheiro decimal e arredondamento AwayFromZero. Sem autenticação: perfil informado para simulação, sem cobrança ou persistência de clientes.

## Requisitos e aceite

- REQ-1: CLI .NET 11 executa TL, Front, Back e QA com Ollama; modo offline identifica respostas preparadas em cada evento e relatório.
- REQ-2: RAG lexical local recupera documentação de negócio com caminho, trecho e SHA256, preservando origem da mudança.
- REQ-3: agentes geram propostas JSON delimitadas; o código é materializado por templates controlados. Não executar texto arbitrário retornado pelo LLM. Explicar essa limitação na interface e documentação.
- REQ-4: testes determinísticos imutáveis verificam limites Padrão/VIP, entradas inválidas e dinheiro. QA do modelo não pode aprovar testes reprovados. No máximo uma correção Back; parada explícita após nova falha.
- REQ-5: gerar aplicação ASP.NET Core funcional, plano, SPEC candidata, propostas, diff, testes e eventos. Painel local mostra eventos reais do processo, duração, modelo/tokens quando disponíveis e falhas.
- REQ-6: demo offline injeta proposta preparada errada (Padrão20); testes reais falham e correção preparada troca para10. Rotular o cenário como demonstração preparada, sem alegar descoberta autônoma.
- REQ-7: script opt-in cria branch e draft PR com aplicação candidata e evidências aprovadas; nenhum merge automático. Lab publicado em main; criar PR de demonstração é uma operação separada explícita.
- REQ-8: manter lab08 como destaque principal da capa e adicionar09 como novidade secundária. Figma é referência opcional por link; não simular captura real.

## Arquitetura KISS

Console runner + biblioteca de domínio + app de referência + harness console sem NuGet + painel estático servido pelo app. Ollama em localhost, sem credenciais. Fonte textual tratada como dados. Delegação por contratos JSON: TL descreve plano; Front define textos; Back define limites; QA explica riscos. Propostas Back são dados tipados, validados, usados por código de confiança; não é um agente de programação de propósito geral.

Escopo inicial: demanda fixa, quatro papéis, uma rodada de correção. Demo precisa funcionar sem modelos; modo real nunca deve cair silenciosamente em replay. Publicação depende de build, testes e revisão. Datas/empresa/dados do currículo não fazem parte do lab.
