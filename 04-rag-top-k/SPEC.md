# SPEC — RAG com Top-K Configurável

## Objetivo

Comparar a quantidade de documentos recuperados para uma mesma pergunta, tornando visíveis contexto, relevância e ruído sem adicionar banco vetorial ou framework de RAG.

## Comportamento

- Indexa arquivos Markdown inteiros e calcula o ranking por cosseno uma vez por execução.
- `--top-k N` executa uma rodada (padrão: 3); `--compare 1,3,5` executa várias rodadas com os mesmos embeddings e ranking.
- Preserva a ordem dos valores solicitados e remove valores repetidos.
- Limita a recuperação ao número de arquivos disponíveis e mostra K solicitado e quantidade efetiva.
- Exibe arquivos, scores, contexto completo, tamanho em caracteres UTF-16, resposta e fontes com IDs `[S1]`.
- Usa os mesmos modelos, pergunta, temperatura zero, seed 42 e limite de 384 tokens de saída nas rodadas; não promete respostas idênticas entre máquinas.
- Valida opções antes de chamar Ollama e encerra com código 1 em erros.

## Limites

- Cada arquivo é um chunk; não há chunking, persistência nem filtro mínimo de similaridade.
- K maior inclui resultados menos próximos: mais contexto não garante mais precisão.
- Não mede qualidade automaticamente; a comparação exige conferir resposta e fontes.
- Caracteres não são tokens. Contexto de geração continua limitado pelo modelo e pode ser truncado pelo Ollama; use documentos pequenos.
- A API de embeddings recebe `truncate=false` para rejeitar documentos maiores que a janela do modelo de embeddings.
- Texto dos documentos e da pergunta vai ao endpoint configurado; processamento local exige endpoint local.
- Citações geradas são probabilísticas; a lista de fontes exibida permite conferência manual.

## Critérios de aceite

1. `--compare 1,3,5` usa um embedding por arquivo e um da pergunta, com uma geração por K distinto.
2. Com três documentos, as rodadas recuperam respectivamente 1, 3 e 3 arquivos.
3. Ranking e IDs permanecem consistentes entre rodadas; o contexto impresso corresponde ao enviado à geração.
4. Zero, negativos, lista malformada, valor ausente e combinação de `--top-k` com `--compare` falham claramente.
5. Compila em .NET 11 e funciona com a API HTTP do Ollama.
