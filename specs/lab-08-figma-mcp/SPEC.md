# SPEC — LAB-08: especificação pelo Figma via MCP

## Demanda KISS e fontes de verdade

Evoluir o simulador de descontos do lab 07 a partir de um arquivo Figma com estados inicial, calculando, sucesso e erro. A especificação visual nasce dos frames reais consultados por MCP; documentos de negócio continuam definindo limites e cálculo. Não substituir Figma por uma imagem gerada ou um Markdown de design.

Stack: .NET 11/C#, ASP.NET Core para a aplicação de referência e HTML/JS. Reutilizar contrato e cálculo do lab 07; não duplicar checkout, login ou banco. Sem pacotes NuGet externos.

O usuário autorizou criar um arquivo novo e comprovou que o Figma está instalado/conectado. Nesta sessão, ferramentas do conector não foram expostas ao agent e a navegação foi recusada pela política do navegador; criação real permanece pendente de acesso operacional. Nenhum ID/URL de arquivo deve ser inventado.

Depois o usuário forneceu o arquivo `jms60WykB5z30uSU0phFqe`, nó `0:1`, com links Design e Prototype registrados em `08-sdd-figma-mcp/figma.source.json`. Esse é o alvo para consulta e edição quando o conector estiver disponível. Conteúdo e tipo do nó ainda não foram lidos; consultar metadados e selecionar os frames antes de implementar. O arquivo fornecido não foi criado por este agent.

O alvo mais recente é a cópia `ZUseBZLMJIIEN5HjFBB0fu`, `KissProject4Codex`, também fornecida pelo usuário. Ela é a referência preferida em `figma.source.json`; os links anteriores foram preservados para procedência. Nenhum desses links foi apresentado como uma captura MCP ou como conteúdo já implementado.

## Requisitos

| ID | Requisito | Critério verificável |
|---|---|---|
| F-1 | Conectar ao servidor MCP desktop do Figma por Streamable HTTP | initialize, notifications/initialized, tools/list e tools/call com sessão e versão de protocolo |
| F-2 | Descobrir ferramentas e consultar um node/frame real | get_metadata, get_design_context e get_screenshot; resposta de erro ou tool ausente não é sucesso |
| F-3 | Persistir contexto, metadados e screenshot retornados pelo MCP | arquivo de evidência identifica endpoint, node, modo real/mock, horário e cada resultado |
| F-4 | TL combina especificação do Figma com RAG de negócio e delega Front/Back | requisito e proposta de Front referenciam FIGMA; contrato e regras de negócio seguem lab 07 |
| F-5 | Explicitar detalhe de design e estados sem inventar evidências | dimensões, layout, estilos, labels, estados e IDs derivam do capture; ausências bloqueiam aprovação visual |
| F-6 | Demo e testes independem de credenciais, sem alegar conexão real | mock transport reproduz contrato MCP; fixtures rotuladas como fictícias; testes sem sockets |

## Contrato MCP

- Endpoint padrão: `http://127.0.0.1:3845/mcp`, servidor desktop habilitado no Figma.
- Transporte JSON-RPC 2.0 com HTTP POST, `Accept: application/json, text/event-stream`.
- Negociar protocolo `2025-06-18`, confirmar versão retornada, enviar `MCP-Protocol-Version` nos pedidos seguintes e propagar `Mcp-Session-Id` quando recebido.
- Notificação initialized sem id; aceitar 202 sem JSON. Suportar respostas JSON e SSE para requests; recusar resposta de outro id e mensagens JSON-RPC error.
- Chamar ferramentas presentes em tools/list; usar argumentos compatíveis com o inputSchema retornado. Node obrigatório no modo real. Cliente standalone deste lab não implementa OAuth do servidor remoto nem contorna controle de clientes do Figma.
- Limitar timeout, tamanho de respostas e registrar tool errors sem salvar credenciais. Não executar código recebido do Figma.

## Arquivo Figma a criar quando conectado

Nome: `KISS AI Lab 08 — Simulador de descontos`.

Uma página `Especificação` com quatro frames desktop (1440×900) e um frame mobile (390×844). Nomes: `01 Inicial`, `02 Calculando`, `03 Sucesso`, `04 Erro`, `05 Mobile`.

- Título: Simulador de desconto; descrição: simulação sem cobrança ou gravação.
- Campos rotulados: Valor do pedido (R$), Desconto (%). Defaults 100 e 10.
- Ajuda: Desconto permitido de 0 a 20%.
- Botão: Simular; carregando: Calculando… e desabilitado.
- Sucesso: valor original R$100,00, desconto10%, total R$90,00.
- Erro: percentual21%, mensagem de limite legível, sem resultado antigo visível.
- Auto-layout: formulário vertical; layout responsivo sem rolagem horizontal.
- Tokens propostos para o desenho: fundo#F1F5F7, superfície#FFFFFF, texto#172B36, ação#165C72, erro#A01919; Inter, espaçamentos8/16/24/32, raios6/16.
- Essas são instruções para criar o arquivo, ainda não evidências de um design existente. Depois da criação, registrar URL, IDs e screenshot do MCP antes de implementar Front.

## Delegação

TL: consultar MCP e negócio, emitir SPEC candidata rastreável, definir contrato e distribuir tarefas. Dev Front: implementar exclusivamente a partir do contexto de design e screenshot reais, adaptar à stack e validar estados/geometry. Dev Back: preservar endpoint POST/api/discount e validação decimal do lab07. A API de referência pode ser executada independentemente; conformidade visual do lab08 permanece pendente até Figma conectado.

## Gates

G1: escopo definido. G2 técnico: contrato MCP e API definidos. G2 visual: pendente de arquivo real e contexto de alta fidelidade. G3/G4: código de integração testado com mock, sem confundir mock com Figma conectado. Entrega final requer criação/captura no Figma, verificação visual e publicação.
