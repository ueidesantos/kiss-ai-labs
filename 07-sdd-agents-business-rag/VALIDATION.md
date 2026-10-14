# Validation — LAB 07

2026-10-11, America/Sao_Paulo. SDK 11.0.100-rc.1.26425.128.

- Three lab projects built: app, coordinator, and harness; zero warnings/errors.
- 66 .NET checks passed: decimal calculation, limits, invalid types, in-memory HTTP handler, body limit, lexical RAG/ranking/diacritics/empty corpus, sources, contract, roles, delegation, replay, and artifact generation.
- Nine JS tests passed: payload, BRL, loading, errors, invalid inputs, currency precision, scientific notation, and large amounts.
- Independent review found currency loss in the frontend; it was fixed and covered by tests. An evidence gap around normalization/empty corpus was also fixed by adding those scenarios.
- Offline replay ran: retrieved simulacao.md, descontos.md, and glossario.md and wrote SPEC/plan/handoffs without calling a model.
- ASP.NET Core server started and announced `Now listening on: http://127.0.0.1:8087`. A real HTTP request was blocked by socket permissions; the browser also denied access. Therefore, do not report browser/real HTTP tests as passed. Server was stopped after the attempt.
- Real Ollama was not run in this lab; integration was validated with a fake HttpMessageHandler. Semantic review of real output remains pending.
- Architecture image and main cover were generated with integrated image_gen; previous images were preserved.
- Publication: GitHub fetch/push were blocked by DNS in this session. Record the local commit in the delivery; do not confuse it with confirmed publication.

In-memory checks are evidence for the executable core. The real integration gate remains pending in [convergence.md](../specs/lab-07-sdd-agents/convergence.md).
