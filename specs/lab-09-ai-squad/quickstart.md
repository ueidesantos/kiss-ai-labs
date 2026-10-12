# Executar e verificar

Pré-requisito: SDK .NET 11. Comandos na raiz do repositório. Consulte [README](../../09-ai-squad/README.md) para visão do fluxo.

## Demonstração reproduzível

```powershell
dotnet run --project 09-ai-squad/runner -- --offline --out 09-ai-squad/runs/demo
$env:SQUAD_RUN = (Resolve-Path 09-ai-squad/runs/demo).Path
dotnet run --project 09-ai-squad/app
```

Abra http://127.0.0.1:8089. `SQUAD_RUN` aceita diretório ou `report.json`. Use nova pasta de saída por execução. Padrão 100/10% resulta 90; Padrão 100/15% é recusado; VIP 100/15% resulta 85.

Propostas e revisões offline são preparadas. A proposta inicial 20% para Padrão é reprovada por testes reais; a correção 10% é testada e revisada novamente. Sem `SQUAD_RUN`, o simulador de referência funciona e o painel informa ausência de evidência.

## Dois modelos locais

Com Ollama em localhost:11434 e dois modelos instalados:

```powershell
dotnet run --project 09-ai-squad/runner -- --model MODELO_DESENVOLVEDOR --review-model MODELO_REVISOR --out 09-ai-squad/runs/real
```

Substitua os identificadores. Nomes e digests precisam ser diferentes; aliases de um mesmo modelo são recusados. O modo real não faz replay offline. Ele é uma integração Ollama, não um adaptador das APIs Claude ou Codex. O parâmetro opcional `--lab` identifica o diretório do laboratório.

## Inspeção e publicação opcional

Leia `report.json`, `changes.diff`, `build.log`, `PR.md` e `candidate`. Verifique fontes, modelos, revisões, testes e estado final. Sem testes aprovados e resolução de achados high/critical, o gate permanece fechado. A correção única sempre exige nova revisão.

Com Git, Python e `gh` autenticado:

```powershell
python 09-ai-squad/scripts/publish.py --run 09-ai-squad/runs/demo --repo SEU_USUARIO/SEU_REPOSITORIO
```

O script confere gate/manifesto, cria clone temporário, publica branch e abre draft PR. Nenhum merge automático. URL de PR somente é evidência quando retornada por uma publicação concluída.

[Convergência](convergence.md) separa testes de integração preparados e chamadas reais aos modelos.
