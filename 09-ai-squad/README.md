# Lab 09 — KISS AI Squad

**Uma mudança de negócio, quatro papéis de IA e evidências para revisar.** Evolua o simulador de desconto: Padrão até 10%, VIP até 20%. Acompanhe o TL, as propostas Front/Back, os testes e a revisão QA — incluindo uma correção quando necessária.

O diferencial é verificar a entrega: fontes de negócio, SPEC candidata, aplicação, diff e resultados de teste ficam disponíveis. Um agente dizer “aprovado” não supera um teste reprovado.

## Experimente sem modelo

Pré-requisito: SDK **.NET 11**. Na raiz do repositório:

```powershell
dotnet run --project 09-ai-squad/runner -- --offline --out 09-ai-squad/runs/demo
$env:SQUAD_RUN = (Resolve-Path 09-ai-squad/runs/demo).Path
dotnet run --project 09-ai-squad/app
```

Abra **http://127.0.0.1:8089**. O simulador funciona sem um relatório; `SQUAD_RUN` conecta o painel à execução. Use outro diretório de saída para cada experimento.

No offline, as propostas são **preparadas**: Back começa com Padrão=20%, os testes reais reprovam a regra e uma correção preparada troca o limite para 10%. É uma demonstração reproduzível de verificação, não uma descoberta autônoma de IA.

## Execute com Ollama

Com o Ollama em execução e o modelo instalado:

```powershell
dotnet run --project 09-ai-squad/runner -- --model llama3.2 --out 09-ai-squad/runs/ollama
```

O modo real pede propostas ao modelo. Ele não garante uma falha inicial e não cai silenciosamente em offline. Modelo ausente, resposta inválida ou erro de conexão é falha da execução. Consulte o registro de [convergência](../specs/lab-09-ai-squad/convergence.md) para distinguir integrações exercitadas das disponíveis no código.

## Quem faz o quê

| Papel | Trabalho delimitado |
|---|---|
| TL | Consulta regras via RAG, organiza o plano e a SPEC candidata |
| Dev Front | Propõe textos da interface por contrato JSON |
| Dev Back | Propõe os limites da política por contrato JSON |
| QA | Revisa evidências e explica riscos; não altera os testes |

O runner coordena etapas sequenciais. O RAG é lexical: TF-IDF e similaridade de cosseno sobre Markdown, reutilizando o lab 07. As fontes registram caminho, trecho e SHA256.

**Limite intencional:** os agentes fornecem dados estruturados; templates controlados materializam a candidata. Este lab não executa código arbitrário retornado pelo LLM. Uma rodada de correção Back é o máximo; uma nova reprovação encerra o fluxo.

## Da execução ao PR

Plano, propostas, fontes, diff, testes e eventos formam a evidência da execução. O painel apresenta o modo, os resultados e informações de modelo quando disponíveis. Não inventa métricas de uso para o modo offline.

A publicação da candidata é separada e opt-in, com Git, GitHub CLI autenticado e Python disponíveis:

```powershell
python 09-ai-squad/scripts/publish.py --run 09-ai-squad/runs/demo
```

O script publica em branch e abre **draft PR** somente para uma execução aprovada. Revise os artefatos antes de executar. Nenhum merge é automático. Publicar o código deste laboratório em `main` não significa que um PR de demonstração já foi criado.

## Entenda e evolua

- [Demanda KISS](DEMAND.md) e [SPEC canônica](../specs/lab-09-ai-squad/SPEC.md).
- [Kit SDD explicado](SPECKIT.md), [plano](../specs/lab-09-ai-squad/plan.md), [tarefas](../specs/lab-09-ai-squad/tasks.md) e [quickstart](../specs/lab-09-ai-squad/quickstart.md).
- [Regras de negócio](business/descontos.md), [cálculo](business/simulacao.md) e [evidências](business/entrega.md).
- [Checklist](../specs/lab-09-ai-squad/checklists/requirements.md) e [convergência](../specs/lab-09-ai-squad/convergence.md).
- [Lab 08 — Figma como especificação](../08-sdd-figma-mcp/README.md): referência visual complementar. O lab 09 não simula captura ou acesso ao Figma.

Dados e regras são fictícios. Selecionar VIP é uma opção do simulador, sem autenticação, pagamento ou dados de clientes.
