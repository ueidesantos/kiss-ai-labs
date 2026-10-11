# Executar o Lab 07

Pré-requisitos: SDK .NET 11 e Node.js para teste frontend. Ollama e um modelo local são necessários somente para geração real. Não é necessário instalar Spec Kit CLI ou pacotes NuGet externos.

Execute os comandos abaixo na raiz do repositório:

```powershell
dotnet build 07-sdd-agents-business-rag/src/SddAgents.csproj
dotnet build 07-sdd-agents-business-rag/app/DiscountApp.csproj
dotnet build 07-sdd-agents-business-rag/tests/LabTests.csproj
dotnet run --project 07-sdd-agents-business-rag/tests
node --test 07-sdd-agents-business-rag/tests/frontend.test.cjs
```

O coordenador informou: três builds sem warnings, 66 checks .NET e nove testes JS passaram. As validações usam transporte em memória/simulado; não atestam rede real.

## Replay sem modelo

```powershell
dotnet run --project 07-sdd-agents-business-rag/src -- --offline
```

Leia a pasta de artefatos mostrada no terminal: contexto, SPEC candidata, tarefas, propostas e review. O modo declara `REPLAY OFFLINE`; o conteúdo usa respostas preparadas, não IA. Para escolher saída, acrescente `--out <pasta-vazia>`; o workflow recusa sobrescrever pasta não vazia.

## Aplicação de referência

```powershell
dotnet run --project 07-sdd-agents-business-rag/app
```

Abra `http://127.0.0.1:8087`. Simule R$100 com 10%: total R$90,00. O endpoint também pode ser consultado:

```powershell
Invoke-RestMethod -Uri http://127.0.0.1:8087/api/discount -Method Post -ContentType application/json -Body '{"amount":100,"discount":10}'
```

HTTP real e inspeção visual não foram validados nesta execução por restrições do ambiente. O comando é roteiro para completar essa evidência.

## Geração real

Com Ollama disponível em `http://localhost:11434` e `llama3.2` instalado:

```powershell
dotnet run --project 07-sdd-agents-business-rag/src -- --model llama3.2
```

Use `--cpu` se desejar geração na CPU; `--url`, `--top-k`, `--lab` e `--out` ajustam endpoint, recuperação e caminhos. `--help` lista opções. O fluxo chama TL, Front e Back sequencialmente. Revise citações e propostas antes de incorporá-las. Ollama real não foi validado nesta execução; artefatos candidatos não aprovam mudança de negócio.
