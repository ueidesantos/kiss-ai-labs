# Análise de consistência

Base: [SPEC](SPEC.md). Data: 2026-10-11.

| Requisito | Implementação/evidência | Limite |
|---|---|---|
| F-1 | FigmaMcpClient initialize/notification/sessão/protocolo/JSON/SSE | 19 checks mock, não servidor real |
| F-2 | tools/list, schemas e chamadas obrigatórias | Nó real ainda indisponível |
| F-3 | Capture inclui metadata/context/screenshot/origem/time | Captura real e gravação CLI precisam confirmação |
| F-4 | Coordenador integra fonte FIGMA com workflow lab 07 | Não atestar geração/delegação real sem capture |
| F-5 | Briefing dos frames, tokens e estados | É instrução de criação; gate visual pendente |
| F-6 | Transporte fake e modo demo/fixture explícito | Mock não comprova conformidade ao Figma |

As fontes têm autoridade distinta: negócio define regra; Figma define apresentação; SPEC registra decisões; testes dão evidência de execução. Um texto de design não é Figma real. Uma imagem no README não é captura MCP por si só.

Situação do conector: screenshot comprova instalado/conectado; tools não expostas no inventário e navegação Figma negada impediram ação real. Não transformar esse bloqueio em pedido de reinstalação. O [contrato](contracts/design-source.json) mantém URL/node nulos porque nenhum arquivo foi criado.

Referências FIGMA válidas comprovam procedência identificada, não que toda descrição gerada seja fiel. Comparação visual e revisão humana continuam obrigatórias. [Convergência](convergence.md) registra a consequência: entrega integral ainda não aprovada.
