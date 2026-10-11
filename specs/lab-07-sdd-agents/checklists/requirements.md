# Checklist de requisitos

Base: [SPEC](../SPEC.md), [análise](../analysis.md).

- [x] Demanda tem usuário, objetivo e exemplo de resultado.
- [x] Escopo exclui cobrança, persistência de pedido, login e aprovação gerencial.
- [x] Fontes B1/B2/B3 e requisitos REQ-1..6 estão identificados.
- [x] Todos os requisitos possuem critério de aceite e tarefa.
- [x] .NET 11/ASP.NET Core e frontend vanilla estão definidos antes da entrega.
- [x] Contrato de API é compartilhado; cálculo usa decimal.
- [x] TL delega exatamente Front/Back e mantém limites de atuação.
- [x] Refs inválidas e contrato divergente são recusados por validação.
- [x] Replay registra que não chamou modelo; código proposto não é executado.
- [x] Testes core e builds têm resultado informado sem extrapolar evidência.
- [ ] AC-3/AC-4 verificados com navegador e HTTP real.
- [ ] AC-1/AC-2/AC-5/AC-6 demonstrados com Ollama real e revisão humana do conteúdo.
- [ ] Publicação remota confirmada.

Conclusão: pronto para revisão do core; validação integral e publicação ainda dependem das integrações pendentes. Checklist de processo não é aprovação automática das propostas.
