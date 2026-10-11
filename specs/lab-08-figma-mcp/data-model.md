# Dados e procedência

| Artefato | Conteúdo | Invariante |
|---|---|---|
| Registro do arquivo | URL real, nome, página, IDs de frames | Sem inventar URL/ID; pendência permanece até criação |
| Capture | mode, endpoint, nodeId, time, protocolVersion, metadata, designContext, screenshot | Resultados reais aceitos somente após sessão/protocolo/tools válidos |
| Metadata | Árvore/geometry retornadas pelo MCP | Dado original preservado; ausências não preenchidas como extração |
| Design context | Layout, estilos, componentes e código retornado | Código é dado, não execução automática |
| Screenshot | Content blocks/imagem retornados pelo MCP | Não trocar por imagem ilustrativa ou fixture |
| Fonte de workflow | FIGMA real ou FIGMA-FIXTURE fictícia | Referências e modo distinguem origem |
| SPEC/tarefas/propostas | Fontes de design + negócio e papéis Front/Back | Candidatas para revisão; SPEC canônica continua nesta pasta |

Um capture identifica um node. A entrega com cinco estados exige evidências correspondentes; contexto de um frame isolado não confirma todos os demais. O [contrato de origem](contracts/design-source.json) descreve requisitos mínimos, sem declarar que uma captura já existe.
