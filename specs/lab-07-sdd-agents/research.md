# Pesquisa e decisões técnicas

Referência: [SPEC](SPEC.md). Pesquisa de processo: [Spec Kit oficial](https://github.com/github/spec-kit), consultado em 2026-10-11. Os artefatos abaixo são autorais desta demanda.

| Questão | Decisão | Motivo e limite |
|---|---|---|
| Stack | .NET 11 / ASP.NET Core / HTML vanilla | Atende à stack solicitada; três projetos sem NuGet externos |
| Recuperação | TF-IDF e cosseno, documentos Markdown inteiros | Corpus de três textos; simples, local e inspecionável. Não equivale a embeddings semânticos |
| Coordenação | TL primeiro, depois uma chamada Front e uma Back | Contrato compartilhado antes da proposta; três chamadas sequenciais |
| Dinheiro | `decimal`, duas casas, `AwayFromZero` | Para valores positivos, implementa HALF_UP das fontes |
| Persistência | Somente artefatos do workflow | Simulação não cria pedido ou cobrança; dispensa banco |
| Execução das propostas | Revisão humana | Código gerado é arquivo de proposta, não executável automático |
| Demonstração sem modelo | Replay explícito | Exercita artefatos e validações; não mede qualidade de IA |

Não adotados nesta demanda: vector database, microserviços, framework Front, autenticação, MCP e implantação automática. Omissões são limites de escopo, não promessas de recursos.

Próxima evidência necessária: executar aplicação HTTP e três chamadas Ollama em ambiente com acesso autorizado; revisar fidelidade das regras citadas, além da validade dos IDs.
