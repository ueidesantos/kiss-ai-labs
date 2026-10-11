# Executar e verificar

Todos os comandos partem da raiz do repositório. Instale o SDK .NET 11. O offline não precisa de Ollama, credenciais ou banco de dados.

## 1. Execute a demonstração

```powershell
dotnet run --project 09-ai-squad/runner -- --offline --out 09-ai-squad/runs/demo
```

Confira os artefatos em `09-ai-squad/runs/demo`. O erro Padrão=20% é preparado; os testes devem reprovar a proposta inicial. A correção preparada para 10% deve ser verificada novamente. Use um novo caminho de saída ao repetir.

## 2. Abra o simulador e o painel

```powershell
$env:SQUAD_RUN = (Resolve-Path 09-ai-squad/runs/demo).Path
dotnet run --project 09-ai-squad/app
```

Abra http://127.0.0.1:8089. `SQUAD_RUN` aceita o diretório de execução ou o caminho de `report.json`. Sem relatório, a interface deve explicar a ausência de uma execução. O simulador de referência continua disponível.

Experimente Padrão com 10%, Padrão com 15% e VIP com 15%. Os resultados esperados são aceitação, rejeição e aceitação. Para R$ 100 com VIP e 15%, o total deve ser R$ 85,00.

## 3. Use propostas de um modelo real

Com Ollama ativo e `llama3.2` já instalado:

```powershell
dotnet run --project 09-ai-squad/runner -- --model llama3.2 --out 09-ai-squad/runs/ollama
```

Este comando exercita uma integração externa local. Falha de conexão ou resposta inválida não pode gerar uma alegação de sucesso nem acionar replay. A evidência deve identificar o modelo e o modo.

## 4. Revise antes de publicar

Leia fontes, plano, SPEC candidata, diff e testes. Uma rodada de correção pode ocorrer; depois disso, falha encerra o ciclo. A aprovação de um modelo não substitui os testes.

Para solicitar a publicação de uma execução aprovada, com GitHub CLI autenticado, Git e Python:

```powershell
python 09-ai-squad/scripts/publish.py --run 09-ai-squad/runs/demo
```

A publicação opcional cria draft PR, sem merge. Falta de autenticação ou falha de rede deve ser reportada; não presumir publicação. O [registro de convergência](convergence.md) contém o estado de verificação desta entrega.
