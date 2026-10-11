# Figma como especificação visual

**Arquivo Figma obrigatório ainda não criado nesta execução.** O usuário autorizou a criação. Screenshot fornecido comprova conector instalado e conectado, mas as ferramentas Figma não estão expostas no inventário callable desta sessão; navegação Figma foi explicitamente negada. Isso bloqueia criação/captura real e aprovação visual. Nenhuma URL ou ID de arquivo foi inventado.

## Duas integrações distintas

| Caminho | Função | Estado/limite |
|---|---|---|
| Conector Figma do Codex | Criação/edição/leitura via ferramentas suportadas e autenticação OAuth da plataforma | Instalado/conectado segundo screenshot; ferramentas não disponíveis nesta sessão |
| Cliente .NET deste lab | Leitura de nodes via MCP desktop em `http://127.0.0.1:3845/mcp` | Implementado e testado com mocks; servidor real não comprovado |

O conector identificado nesta sessão é `connector_68df038e0ba48191908c8434991bbac2`. Não é necessário pedir reinstalação como resposta à ausência de ferramentas. O cliente .NET não reaproveita tokens OAuth do conector, não implementa OAuth remoto e não contorna restrições de clientes. A documentação oficial descreve o [servidor remoto e OAuth](https://developers.figma.com/docs/figma-mcp-server/remote-server-installation/) e o [servidor desktop](https://developers.figma.com/docs/figma-mcp-server/local-server-installation/). A compatibilidade do cliente próprio com o ambiente real precisa ser validada respeitando os clientes suportados pelo Figma.

## Arquivo a criar quando as ferramentas estiverem disponíveis

Nome: `KISS AI Lab 08 — Simulador de descontos`. Página: `Especificação`. Este é um briefing de criação, não evidência de um design existente.

| Frame proposto | Dimensão | Conteúdo/estado |
|---|---|---|
| 01 Inicial | 1440 × 900 | Valor 100, desconto 10, ajuda de limite, Simular habilitado |
| 02 Calculando | 1440 × 900 | Botão desabilitado, texto Calculando…, resultado anterior oculto |
| 03 Sucesso | 1440 × 900 | Original R$100,00; desconto 10%; total R$90,00 |
| 04 Erro | 1440 × 900 | Desconto 21%; mensagem de limite; sem resultado anterior |
| 05 Mobile | 390 × 844 | Mesmo formulário, layout vertical sem rolagem horizontal |

Conteúdo obrigatório: título `Simulador de desconto`, descrição de simulação sem cobrança/gravação, labels `Valor do pedido (R$)` e `Desconto (%)`, ajuda `Desconto permitido de 0 a 20%`. Usar Auto Layout vertical e hierarquia explícita de título, descrição, campos, ajuda, botão, status e resultado.

Tokens propostos: fundo `#F1F5F7`, superfície `#FFFFFF`, texto `#172B36`, ação `#165C72`, erro `#A01919`; fonte Inter; espaçamento 8/16/24/32; raios 6/16. Estes valores são instruções para criar, não valores extraídos do MCP. Depois da criação, derivar tokens/geometry realmente usados do capture e registrar desvios.

## Da criação à delegação

1. Criar o arquivo através do conector suportado; registrar URL real e IDs dos cinco frames. Confirmar acesso e conteúdo sem usar IDs fictícios.
2. No Figma desktop, abrir o arquivo e habilitar servidor MCP em Dev Mode conforme [guia oficial](https://developers.figma.com/docs/figma-mcp-server/local-server-installation/).
3. Copiar o link de cada frame para obter node ID; capturar com o CLI do lab. O cliente chama initialize → initialized → tools/list → metadata/design context/screenshot, conforme [ciclo MCP 2025-06-18](https://modelcontextprotocol.io/specification/2025-06-18/basic/lifecycle).
4. Guardar captura e screenshot originais. Captura de um frame comprova apenas aquele frame; repetir para cobrir os estados obrigatórios.
5. TL incorpora a fonte `FIGMA` real junto aos documentos de negócio na SPEC candidata e delega. `FIGMA-FIXTURE` de demo continua fictícia e não fecha o gate visual.
6. Dev Front implementa somente após contexto real suficiente; Dev Back preserva contrato e regra decimal do lab 07. Revisar UI versus screenshots em desktop/mobile e estados.

## Evidências e gates

- Entrega obrigatória: URL do arquivo, node IDs, capturas, contexto de design, screenshots e revisão visual ligada a F-2..F-5.
- Disponível: 19 checks MCP com transporte simulado, informados pelo coordenador. Isso valida protocolo e recusas, não criação ou acesso real.
- Pendente: arquivo real, captura real, conformidade Front e publicação GitHub.
- Não aprovar visual por screenshot ilustrativo, fixture ou apenas pelo sucesso de mocks. Seguir [convergência](../specs/lab-08-figma-mcp/convergence.md).
