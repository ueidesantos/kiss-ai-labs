# SPEC — RAG Local sem Vector Database

## Objetivo

Demonstrar recuperação aumentada por geração (RAG) com o mínimo de peças: arquivos Markdown locais, embeddings do Ollama, similaridade de cosseno calculada pelo próprio programa e uma resposta gerada com os trechos recuperados.

## Comportamento

- Lê arquivos `.md` de uma pasta local, sem recorrer a serviços de armazenamento vetorial.
- Gera um embedding por documento usando a API local `/api/embed` do Ollama.
- Calcula similaridade de cosseno em memória e seleciona os trechos mais próximos.
- Envia pergunta e contexto recuperado para `/api/generate` e imprime a resposta.
- Recebe pergunta, caminho dos documentos, modelos e quantidade de resultados como opções.
- Mostra os arquivos recuperados para tornar o fluxo observável.

## Limites

- Cada arquivo Markdown é um único documento; chunking, persistência e citações com trechos precisos ficam para laboratórios posteriores.
- Todos os documentos e vetores ficam em memória durante a execução.
- Os documentos não são enviados a um serviço externo, mas seus embeddings e a pergunta passam pelo endpoint Ollama configurado.
- A qualidade da recuperação depende do modelo de embedding e do conteúdo fornecido.

## Critérios de aceite

1. Não usa vector database nem pacote de abstração de RAG.
2. Permite indicar uma pasta Markdown, modelos, endpoint e Top-K.
3. Informa claramente quando a pasta não existe, está vazia ou o Ollama não responde.
4. Faz busca por cosseno semântico em memória antes da geração.
