# Análise de coerência

O lab transforma a promessa de “time de IA” em um fluxo observável e pequeno: quatro papéis, contratos de saída e uma correção. A parte demonstrável é a passagem de propostas por testes que os agentes não controlam.

## Riscos que a apresentação deve evitar

- Confundir o runner sequencial com agentes autônomos programando em paralelo.
- Chamar recuperação TF-IDF de embeddings semânticos.
- Apresentar erro injetado no offline como descoberta espontânea do modelo.
- Prometer código livre quando o mecanismo usa templates.
- Tratar comentário do QA como prova de correção.
- Tratar existência de relatório ou PR como aprovação humana.
- Afirmar captura Figma quando há somente link de referência.

REQ-1 e REQ-6 exigem rótulos de modo nos eventos e relatórios. REQ-3 exige explicar a materialização por templates. REQ-4 impede aprovação com testes reprovados. REQ-7 mantém revisão humana no draft PR.

## Limites de cobertura

Testes fixos demonstram os casos previstos, não ausência de todos os defeitos. Executar a demo não comprova compatibilidade com um modelo específico do Ollama. Build não comprova acessibilidade visual nem operação do GitHub. Essas verificações devem aparecer separadas no registro final.

A revisão de outro modelo acrescenta um gate adversarial. Não equiparar dois aliases a dois modelos nem apresentar o modo preparado como revisão real. A identidade é verificável; a independência de raciocínio não é garantida. Após mudança da candidata, a revisão anterior deixa de valer: a correção passa por nova rodada.
