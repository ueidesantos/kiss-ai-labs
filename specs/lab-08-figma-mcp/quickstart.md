# Roteiro de execução

Pré-requisitos do core: SDK .NET 11. Captura real: Figma desktop com arquivo real aberto, Dev Mode/MCP habilitado e compatibilidade de cliente autorizada. SPEC por modelo: Ollama e modelo local. CLI Spec Kit oficial não é pré-requisito.

Na raiz do repositório:

```powershell
dotnet build 08-sdd-figma-mcp/src/FigmaMcp.csproj
dotnet run --project 08-sdd-figma-mcp/tests
```

O coordenador informou 19 checks mock aprovados. São verificações sem sockets; não conexão real.

## Capturar um node real

Crie primeiro o arquivo definido em [FIGMA.md](../../08-sdd-figma-mcp/FIGMA.md), registre URL e node IDs reais e habilite servidor desktop conforme [guia Figma](https://developers.figma.com/docs/figma-mcp-server/local-server-installation/).

O CLI em integração pelo coordenador usa estes comandos; confirme `--help` e os arquivos resultantes na entrega antes de executá-los:

```powershell
dotnet run --project 08-sdd-figma-mcp/src -- capture --node NODE --out ./captura
dotnet run --project 08-sdd-figma-mcp/src -- spec --capture ./captura/capture.json --out ./saida
dotnet run --project 08-sdd-figma-mcp/src -- demo
```

`NODE` deve ser substituído pelo ID real, não literalmente usado como ID. A segunda operação usa captura como fonte de design e Ollama no modo real; opção `--offline`, quando utilizada, significa replay explícito. Demo usa fixture fictícia e não se apresenta como Figma real. Capturar cada estado separadamente evita presumir cobertura por um único frame.

Depois, confira modo, endpoint, node, horário, contexto, metadata e screenshot no capture. Revise SPEC/tarefas/propostas e rejeite informações ausentes inventadas. Não execute propostas automaticamente.

## Aplicação funcional de referência

```powershell
dotnet run --project 07-sdd-agents-business-rag/app
```

Acesse `http://127.0.0.1:8087` quando o ambiente permitir. Esta API/UI serve para demonstrar funcionalidade do lab 07; fidelidade ao Figma do lab 08 permanece pendente. Verifique estados 100/10→90, erro >20 e comportamento mobile após captura real.

Nesta execução, criação/captura real ficaram bloqueadas por ferramentas Figma não expostas e navegação negada. Não há arquivo real a usar neste roteiro nem evidência de sucesso HTTP/Ollama/Figma/GitHub.
