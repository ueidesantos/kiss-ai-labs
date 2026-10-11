# Pesquisa e decisões

Base: [SPEC](SPEC.md). Fontes consultadas em 2026-10-11: [Figma desktop MCP](https://developers.figma.com/docs/figma-mcp-server/local-server-installation/), [Figma remoto/OAuth](https://developers.figma.com/docs/figma-mcp-server/remote-server-installation/), [MCP lifecycle 2025-06-18](https://modelcontextprotocol.io/specification/2025-06-18/basic/lifecycle).

| Decisão | Justificativa | Limite |
|---|---|---|
| Arquivo Figma real obrigatório | Especificação visual precisa de nodes e contexto verificáveis | Mock/Markdown não substituem design |
| Cliente .NET 11 com HttpClient/JSON | Exemplo protocolar pequeno, sem NuGet externo | Cliente próprio exige validação de compatibilidade real |
| Endpoint desktop local | Documentação informa 127.0.0.1:3845/mcp | Requer Figma desktop, arquivo aberto e servidor habilitado |
| Negociação MCP | Sessão, protocolo, schemas e erros precedem captura | Versionamento e inputSchema não podem ser presumidos |
| Reuso lab 07 | Contrato/cálculo já definidos; foco novo é fonte visual | UI do lab 07 não prova fidelidade ao lab 08 |
| Remote Codex separado | OAuth gerenciado pelo conector | Cliente .NET não coleta/reutiliza tokens nem implementa OAuth |

Screenshot de sessão comprova conector instalado/conectado; inventário de tools não expôs Figma e navegação foi negada. Esta é uma limitação operacional, não uma indicação de que o plugin precisa ser reinstalado.
