# SPEC — RAG Local com SQLite-vec

## Problema

Consultar pequenos documentos locais sem recalcular seus embeddings em toda pergunta e sem manter um serviço de banco vetorial.

## Requisitos

- Python, Ollama e extensão SQLite-vec; somente biblioteca padrão além da extensão.
- Comandos separados `index` e `ask`, com pasta, banco, endpoint, modelos e CPU configuráveis.
- Indexar arquivos Markdown inteiros; persistir textos, vetores float32 e configuração do embedding.
- Reconstruir o índice numa transação; uma falha preserva o conjunto anterior e arquivos removidos deixam de aparecer após reindexação.
- Recusar bancos de outro projeto, índices ausentes e embeddings vazios, não finitos, de norma zero ou dimensão incompatível.
- Consultar distância de cosseno em SQL, recuperar Top-K e mostrar fontes/contexto antes de gerar a resposta.
- Reutilizar documentos persistidos mesmo sem os arquivos originais, sem reindexar em `ask`.
- Documentar trânsito de dados e limitações.

## Critérios de aceite

- Compilação Python e testes de entrada/contrato HTTP passam.
- Testes com SQLite-vec real passam: persistência entre conexões, ranking, K acima do total, remoção após reindexação, preservação do índice em falhas, rollback de DDL e rejeição de configuração incompatível.
- Demo com Ollama local pergunta sobre credenciais/retenção e registra modelo, pergunta, fontes e resultado observado.
- README, backlog e capa incluem o lab sem declarar validação/publicação que ainda não ocorreu.

## Limites

Varredura exata via `vec_distance_cosine`, sem `vec0`, chunking, indexação incremental, filtros ou validação das citações. Uma atualização do modelo sob o mesmo nome e dimensão exige reindexação manual. Reprodutibilidade da instalação/configuração não garante resposta determinística do modelo.
