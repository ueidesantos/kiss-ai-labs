# SPEC — RAG sobre Código-Fonte

## Problema

Encontrar a classe que valida uma regra e onde ela é aplicada em uma pequena pasta de código C#, com referências para conferir a resposta.

## Requisitos

- Console em C#/.NET 11, HTTP explícito para Ollama, sem dependências externas.
- Descobrir `.cs` recursivamente, ignorando links e pastas geradas (`bin`, `obj`, `.git`, `.vs`, `node_modules`).
- Gerar embedding por arquivo e para a pergunta; ranquear por cosseno em memória.
- Recuperar Top-K positivo configurável e mostrar contexto, scores, resposta e fontes com caminho e linhas.
- Pedir resposta em português baseada apenas no contexto e com IDs de fontes.
- Configurar pasta, pergunta, endpoint, modelos e execução opcional na CPU; reportar falhas com código 1.

## Critérios de aceite

- Build sem avisos/erros; teste HTTP verifica descoberta, exclusões, ranking, fontes, contexto e entradas inválidas.
- Demonstração com Ollama local identifica a regra de desconto nos exemplos; registrar modelos, pergunta e saída real.
- README explica execução, trânsito de dados, limitações e ausência de análise sintática.

## Limites

Arquivos inteiros e embeddings em memória. Sem parser, divisão por métodos, grafo de chamadas, persistência ou validação automática das afirmações/citações. Intervalos de linhas representam o arquivo completo. Busca semântica pode omitir relações reais ou recuperar contexto irrelevante.
