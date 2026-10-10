# Validação — RAG sobre Código-Fonte

Execução em 2026-10-10, America/Sao_Paulo.

- SDK: `11.0.100-rc.1.26425.128`; seis projetos do repositório compilados com zero avisos e zero erros.
- `pwsh -File 05-rag-source-code/tests/validate.ps1`: passou. Verifica descoberta recursiva `.cs`/`.CS`, exclusão de `bin`, `obj`, `.git` e Markdown, ranking, quatro chamadas de embeddings, uma geração, contexto e fontes com linhas, K acima do total, modo CPU nas duas APIs e 12 casos de erro. O teste usa vetores controlados, sem avaliar qualidade de modelos.
- Demo real: `nomic-embed-text:latest` (`0a109f422b47`) e `llama3.2:latest` (`a80c4f17acd5`), endpoint local, `--cpu`, K=2.

```bash
dotnet run --no-build --project 05-rag-source-code/src -- --question "Onde a regra de desconto é aplicada?" --top-k 2 --cpu
```

Recuperou `DiscountPolicy.cs` (0,632) e `OrderService.cs` (0,605). Resposta: “A regra de desconto é aplicada antes de calcular o total do pedido, de acordo com o método CalculateTotal da classe OrderService.” Código de saída 0. A saída integral está em [demo-local.txt](demo-local.txt).

O modelo identificou a chamada corretamente, mas omitiu os IDs de citação na resposta. A aplicação imprimiu as fontes independentemente; confira sempre os arquivos. Não há garantia de que o modelo siga o formato pedido.

## Limitação observada no ambiente

Tentativas com execução padrão usando `llama3.2` e `qwen2.5-coder:0.5b` abortaram com `prediction aborted, token repeat limit reached`. A falha também ocorreu numa chamada HTTP direta fora do lab. Uma chamada direta com `num_gpu=0` e a demo completa com `--cpu` passaram. Isso aponta para uma diferença entre os modos de execução neste ambiente, sem estabelecer a causa da falha. A opção CPU é configurável e mantém o comportamento padrão do Ollama quando ausente.

Capa atual e todos os assets existentes preservados. README principal, guia do lab, SPEC e backlog atualizados. `git diff --check` passou.
