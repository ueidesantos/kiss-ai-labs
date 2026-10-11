# Validação — LAB08

2026-10-11, America/Sao_Paulo. **Entrega no Figma e publicação pendentes.**

- Cliente e harness .NET11 compilados sem avisos/erros.
- 19 testes passaram com transporte fictício: lifecycle JSON/SSE, sessão, versão, ID correlacionado, JSON-RPC error, tools/schemas, screenshot exigido, isError, vazio, limite de tamanho, node obrigatório, recusa de fixture como fonte real e assinatura de screenshot. Não são chamadas reais ao Figma.
- CLI `demo` executado e integrou contexto `FIGMA-FIXTURE`, RAG de negócio, TL/Front/Back, SPEC candidata e fonte visual. `figma-review.json` informa `visualMatchVerified=false`, `figmaFileCreatedByThisRun=false` e necessidade de revisão.
- O usuário comprovou plugin instalado/conectado com Allowall e forneceu links Design/Prototype/Community. O alvo mais recente consta de `figma.source.json`.
- Ferramentas do conector não foram expostas nesta sessão; navegador recusou acesso a Figma e a página de Plugins. Web também não conseguiu ler os URLs fornecidos. Nenhum frame, token, componente ou screenshot real foi extraído; nenhuma alteração de canvas foi confirmada.
- Imagem de arquitetura gerada com image_gen, identificada como diagrama no README. Ela não é design/screenshot do Figma nem evidencia conformidade visual.
- Fetch/push GitHub bloqueados por DNS. Commit local não equivale a publicação.

## Obrigatório para concluir

1. Acessar o arquivo real pelo conector; criar/editar os frames definidos no briefing e registrar URL/IDs.
2. Capturar metadados, contexto de alta fidelidade e screenshots; registrar a especificação derivada dessas fontes.
3. Implementar/revisar Front contra os frames reais e executar integração visual/funcional.
4. Conferir gates e publicar os commits com hash remoto confirmado.

Veja [convergence.md](../specs/lab-08-figma-mcp/convergence.md). Não tratar fixture como substituto desses requisitos.
