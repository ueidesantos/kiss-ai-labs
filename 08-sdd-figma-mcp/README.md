# 08 — Figma como especificação via MCP

![Arquitetura do lab08](assets/project-flow.png)

**Figma faz parte obrigatória da entrega.** Ele define a especificação visual; documentos de negócio definem as regras; o TL combina as duas fontes e delega Front/Back. Stack .NET11/C#, sem pacotes externos.

O design de referência está no [Figma — KissProject 4 Codex](https://www.figma.com/community/file/1691245874203095002/kissproject-4-codex?fuid=999015154036575904). Este é o link principal confirmado pelo usuário; a [cópia editável](https://www.figma.com/design/L9IkL9GoyMixBb3bbUXihp/KissProject-4-Codex--Community-?node-id=0-1) e a procedência estão em [figma.source.json](figma.source.json). O usuário aceitou a entrega com o link quando o acesso não estiver disponível. **Conteúdo e integração MCP real ainda não validados.** A imagem acima explica a arquitetura e não é uma captura do Figma. Não foi confirmado que a falta de assinatura seja a causa da limitação.

## Demanda KISS

Evoluir o simulador de descontos do lab07 com uma tela especificada no Figma: inicial, calculando, sucesso, erro e mobile. O contrato/cálculo continuam no [lab07](../07-sdd-agents-business-rag/README.md); não há novo checkout, login ou banco.

O kit detalha layout, tokens, labels, estados, responsividade, fontes e critérios de revisão. O briefing para criar/editar o arquivo é uma instrução de trabalho; só o contexto extraído do Figma poderá aprovar a especificação visual.

## Integração real implementada

O cliente .NET conecta ao servidor MCP desktop e executa:

```text
initialize → notifications/initialized → tools/list
           → get_metadata → get_design_context → get_screenshot
```

Descobre schemas, transmite `nodeId`, aceita JSON/SSE, propaga sessão/versão e rejeita erros, ferramentas ausentes, argumentos desconhecidos e respostas grandes. Não executa código recebido.

Pré-requisitos reais: Figma desktop com MCP habilitado e o arquivo correto aberto. Consulte [FIGMA.md](FIGMA.md) e o [guia oficial](https://developers.figma.com/docs/figma-mcp-server/local-server-installation/). Conectar o plugin Figma ao Codex não inicia automaticamente o servidor desktop deste cliente.

Na raiz do repositório:

```bash
dotnet build 08-sdd-figma-mcp/src/FigmaMcp.csproj
dotnet run --project 08-sdd-figma-mcp/src -- capture --node 0:1 --out 08-sdd-figma-mcp/runs/capture
```

O nó0:1 veio do link; seu tipo ainda precisa ser confirmado. Se for página, use metadados para selecionar os frames relevantes, sem presumir que a página inteira seja um design de alta fidelidade.

`capture` exige contexto textual e screenshot real com dados de imagem válidos; grava `capture.json`, `SPEC.visual-source.md` e screenshots. Não cria o arquivo Figma. Criação/edição do canvas deve ocorrer pelo conector autorizado; essa parte da entrega está pendente.

## Figma + RAG → SPEC → agents

Depois de capturar os frames:

```bash
dotnet run --project 08-sdd-figma-mcp/src -- spec --capture 08-sdd-figma-mcp/runs/capture/capture.json --cpu
```

O comando reutiliza o workflow do lab07, acrescenta a fonte `FIGMA`, consulta negócio, chama TL/Front/Back e grava propostas. Exige que o requisito de Front e a proposta referenciem o design; não aprova fidelidade visual automaticamente. `SPEC.visual-source.md` preserva contexto/metadados do MCP e checklist de revisão. A fonte canônica continua na [SPEC do kit](../specs/lab-08-figma-mcp/SPEC.md).

Opções: `capture`, `spec`, `demo`; `--node`, `--mcp-url`, `--capture`, `--out`, `--lab`, `--offline`, `--cpu`, `--model`, `--url`, `--help`. Saídas padrão em `runs/<horário>/`, ignoradas pelo Git; pastas não vazias são recusadas.

O cliente standalone usa `http://127.0.0.1:3845/mcp`. Não implementa OAuth do servidor remoto. O plugin Codex usa outra conexão, com autenticação e suporte a ferramentas de canvas, conforme o [guia remoto](https://developers.figma.com/docs/figma-mcp-server/remote-server-installation/).

## Demonstração offline e testes

```bash
dotnet run --project 08-sdd-figma-mcp/src -- demo
dotnet run --project 08-sdd-figma-mcp/tests
```

`demo` combina **fixture fictícia, identificada como `FIGMA-FIXTURE`, e replay de agents**. Não acessa Figma/Ollama, não contém IDs reais nem screenshot e não substitui a entrega no Figma. Testes MCP usam um transporte fictício e conferem protocolo, schemas, erros e limites sem sockets. O cálculo e frontend de referência podem ser executados no lab07 enquanto a integração visual real está pendente.

## Kit e dados

Leia [SPECKIT.md](SPECKIT.md), [SPEC](SPEC.md), [guia Figma](FIGMA.md) e [VALIDATION.md](VALIDATION.md). Kit completo da demanda: constituição, pesquisa, esclarecimentos, plano, dados, contrato de design, tarefas, checklist, análise/convergência e quickstart. CLI oficial Spec Kit não foi instalado/executado.

Contexto e screenshot do Figma ficam em disco. No modo de geração real, o contexto textual do design, documentos de negócio e demanda seguem ao endpoint Ollama configurado; screenshots não são enviados ao modelo por este lab. Endpoint remoto recebe esses textos. Propostas não são executadas automaticamente. Timeout MCP30s por solicitação, limite4MiB por resposta; captura inválida ou divergência mantém o gate visual bloqueado.
