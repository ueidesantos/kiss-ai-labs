# Dados e invariantes

Referência: [SPEC](SPEC.md). Não há tabelas ou persistência de pedidos.

| Objeto | Campos | Invariantes |
|---|---|---|
| Pedido de simulação | `amount`, `discount` | Exatamente dois campos JSON numéricos; representáveis como decimal; amount > 0; discount ∈ [0,20] |
| Resultado | `original`, `discount`, `total` | Strings decimais invariantes; dinheiro com duas casas |
| Source | `Id`, `Path`, `Text`, `Score` | ID estável na ordenação de arquivos; caminho de origem e score de cosseno |
| Requirement | `Id`, `Text`, `Sources` | Requisito cita IDs do contexto recuperado |
| Acceptance | `Id`, `Requirements`, `Scenario` | Critério refere requisitos existentes |
| DevTask | `Role`, `Requirements`, `Acceptance`, `Instruction` | Exatamente uma tarefa front e uma back |
| Contract | `Method`, `Path`, `Request`, `Success`, `Error` | Preserva contrato aprovado da API |
| Proposal | `Role`, `Requirements`, `Sources`, `Code`, `Checks` | Papel e requisitos correspondem à tarefa; checks são sugestões, não execução |

Fluxo de estado da página: aguardando entrada → calculando → sucesso ou erro → nova simulação. Resultado anterior é ocultado ao iniciar nova tentativa. O botão desabilitado evita duplicação durante a requisição.

Artefatos de execução: `sources.json`, `plan.json`, `handoffs.json`, `SPEC.candidate.md`, `tasks.md`, `front.proposal.html`, `back.proposal.cs`, `review.json`. `review.json` informa modo, revisão pendente e que código gerado não foi executado.
