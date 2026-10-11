# Validação — LAB07

2026-10-11, America/Sao_Paulo. SDK11.0.100-rc.1.26425.128.

- Três projetos do lab compilados: app, coordenador e harness; zeroavisos/erros.
- 66checks .NET passaram: cálculo decimal, limites, tipos inválidos, handler HTTP em memória, corpo limitado, RAG lexical/ranking/acentos/corpusvazio, fontes, contrato, papéis, delegação, replay e geração de artefatos.
- 9testes JS passaram: payload, BRL, carregamento, erros, entradas inválidas, precisão monetária, notação científica e valores grandes.
- Revisão independente encontrou perda monetária no frontend; corrigida e coberta por testes. Corrigida também lacuna de evidência sobre normalização/corpus vazio por inclusão dos cenários.
- Replay offline executado: recuperou simulacao.md, descontos.md e glossario.md e gravou SPEC/plano/handoffs sem chamar um modelo.
- Servidor ASP.NET Core iniciou e anunciou `Now listening on: http://127.0.0.1:8087`. A solicitação HTTP real foi bloqueada por permissões de socket; navegador também recusou acesso. Portanto, não declarar teste de navegador/HTTP real aprovado. Servidor encerrado após a tentativa.
- Ollama real não executado neste lab; integração validada com HttpMessageHandler fictício. A revisão semântica da saída real permanece pendente.
- Imagem de arquitetura e capa principal geradas com image_gen integrada; imagens anteriores preservadas.
- Publicação: fetch/push do GitHub bloqueados por DNS nesta sessão. Registrar o commit local na entrega; não confundir com publicação confirmada.

Os checks em memória são evidência do núcleo executável. O gate de integração real está pendente em [convergence.md](../specs/lab-07-sdd-agents/convergence.md).
