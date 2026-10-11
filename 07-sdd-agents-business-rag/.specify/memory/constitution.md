# Constituição — Lab 07

Versão 1.0 · 2026-10-11 · Escopo: simulador de descontos e workflow didático.

1. **Negócio antes do código.** Regras são fundamentadas nos Markdown de negócio; conflitos e lacunas são registrados, nunca preenchidos como fatos pelo modelo.
2. **Uma SPEC canônica.** [SPEC da demanda](../../../specs/lab-07-sdd-agents/SPEC.md) é a fonte de verdade. `SPEC.candidate.md` gerada pelo workflow depende de revisão humana.
3. **KISS.** .NET 11, ASP.NET Core e HTML/JS vanilla; sem NuGet externo, banco, login ou cobrança. Recuperação lexical TF-IDF/cosseno basta para três documentos.
4. **Delegação delimitada.** TL entrega uma tarefa Front e uma Back, com requisitos, aceite e contrato. Devs não expandem escopo. Propostas não são executadas automaticamente.
5. **Evidência verificável.** REQ → fonte → tarefa → código → teste. Replay offline e testes com mocks não comprovam geração real ou operação HTTP.
6. **Valores e contrato rigorosos.** Cálculo monetário usa `decimal`; API valida dados, Front apresenta. Mudança de comportamento atualiza a SPEC antes do código.
7. **Convergência honesta.** Integrações pendentes impedem alegar validação completa. CLI/skills oficiais do Spec Kit não foram instalados ou executados nesta entrega.

Alterações desta constituição devem identificar motivo, requisitos afetados e evidências de revisão no kit da demanda.
