# Lab 09 — KISS AI Squad

**Uma IA desenvolve. Outra tenta encontrar o que passou despercebido.** Uma mudança pequena de negócio percorre SDD, RAG, propostas Front/Back, testes e revisão adversarial por um segundo modelo.

A demanda: Padrão pode simular até 10% de desconto; VIP até 20%. O interesse está na entrega verificável: fontes, SPEC candidata, aplicação, diff, achados e testes. A aprovação de uma IA não supera um teste reprovado nem um achado grave.

## Experimente sem modelo

Na raiz do repositório, com SDK **.NET 11**:

```powershell
dotnet run --project 09-ai-squad/runner -- --offline --out 09-ai-squad/runs/demo
$env:SQUAD_RUN = (Resolve-Path 09-ai-squad/runs/demo).Path
dotnet run --project 09-ai-squad/app
```

Abra **http://127.0.0.1:8089**. Use uma pasta de saída nova por execução. O simulador funciona sem relatório; `SQUAD_RUN` conecta o painel às evidências.

Offline usa **propostas e revisões preparadas, sem IA**. Back começa com Padrão=20%; testes reais reprovam a regra; a correção preparada para 10% passa novamente por verificação. Não é uma descoberta espontânea de um modelo.

## Desenvolvedor e revisor distintos

Com Ollama ativo e dois modelos diferentes instalados, substitua os nomes abaixo pelos seus:

```powershell
dotnet run --project 09-ai-squad/runner -- --model MODELO_DESENVOLVEDOR --review-model MODELO_REVISOR --out 09-ai-squad/runs/real
```

O runner verifica nomes e digests no Ollama: dois aliases da mesma instalação não atendem ao contrato. Modelo ausente, resposta inválida ou falha de conexão encerra a execução, sem fallback para demo.

**“Claude desenvolve, Codex revisa” é o exemplo do conceito.** O código deste lab integra dois modelos via Ollama; não implementa APIs Claude/Codex. Identidades distintas reduzem a repetição da mesma revisão, mas não garantem independência ou correção.

| Papel | Responsabilidade |
|---|---|
| TL · modelo desenvolvedor | Usa contexto recuperado para planejar a mudança |
| Front · modelo desenvolvedor | Propõe textos por contrato JSON |
| Back · modelo desenvolvedor | Propõe limites da política por contrato JSON |
| QA adversarial · modelo revisor | Procura falhas, registra evidência/severidade/correção e revisa a nova candidata |
| Testes determinísticos | Verificam a regra sem depender da opinião dos modelos |

O RAG é lexical, com TF-IDF e cosseno reutilizados do lab 07, e registra caminho, trecho e SHA256. Templates controlados materializam a aplicação: os agentes geram dados estruturados, **não código arbitrário executável**.

## Como uma entrega é liberada

1. Materializar a proposta e executar build/testes.
2. Pedir revisão adversarial ao segundo modelo.
3. Se testes falharem ou houver achado `high`/`critical`, permitir uma correção Back.
4. Reexecutar build/testes e revisão. Persistindo o bloqueio, reprovar.
5. Somente uma execução aprovada pode seguir para publicação opcional.

Os artefatos incluem `report.json`, `policy.json`, `front.json`, `plan.md`, `SPEC.md`, `changes.diff`, `build.log`, `PR.md`, `manifest.json` e a pasta `candidate`. Relatório e revisões preservam o histórico da execução.

## Draft PR opcional

Com Git, Python e GitHub CLI (`gh`) autenticado:

```powershell
python 09-ai-squad/scripts/publish.py --run 09-ai-squad/runs/demo --repo SEU_USUARIO/SEU_REPOSITORIO
```

Revise os artefatos primeiro. O script verifica aprovação e integridade, usa clone temporário, publica branch e abre **draft PR**. Não faz merge. Uma demo é identificada como preparada também na entrega. Publicar este laboratório em `main` não significa que um PR de demonstração foi criado.

## Kit SDD e evidências

- [Demanda](DEMAND.md), [SPEC canônica](../specs/lab-09-ai-squad/SPEC.md) e [kit explicado](SPECKIT.md).
- [Plano](../specs/lab-09-ai-squad/plan.md), [tarefas](../specs/lab-09-ai-squad/tasks.md), [quickstart](../specs/lab-09-ai-squad/quickstart.md) e [checklist](../specs/lab-09-ai-squad/checklists/requirements.md).
- [Regras](business/descontos.md), [cálculo](business/simulacao.md) e [convergência/limitações](../specs/lab-09-ai-squad/convergence.md).
- [Lab 08 — Figma como especificação](../08-sdd-figma-mcp/README.md), referência complementar; não há captura Figma neste lab.

Dados fictícios. Selecionar VIP não autentica clientes, efetua pagamentos ou concede uma autorização comercial.
