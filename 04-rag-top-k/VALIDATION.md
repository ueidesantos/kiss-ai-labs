# Validação — RAG com Top-K Configurável

Executada em 09/10/2026, em Windows, com .NET SDK `11.0.100-rc.1.26425.128` e Ollama `0.40.2`.

## Compilação e integração

- Os cinco projetos do repositório compilaram com zero avisos e zero erros.
- `pwsh -File 04-rag-top-k/tests/validate.ps1` passou.
- O teste verificou ranking por cosseno, IDs das fontes e igualdade entre o contexto impresso e o enviado à geração.
- Três documentos e a pergunta produziram quatro chamadas de embedding, reutilizadas nas rodadas; três valores de K distintos produziram três gerações.
- A lista `3,1,5,1` manteve a ordem, eliminou a repetição e recuperou 3, 1 e 3 documentos.
- Doze casos de erro passaram: valores inválidos, opções incompatíveis ou desconhecidas, valor ausente, opção repetida, URL inválida, pasta ausente e pasta vazia.
- A capa e os demais arquivos em `assets/` não foram alterados.

## Demonstração com Ollama local

```bash
dotnet run --no-build --project 04-rag-top-k/src -- --compare 1,3,5 --question "Como protegemos credenciais?"
```

Embedding: `nomic-embed-text`; geração: `llama3.2`; temperatura 0, seed 42, limite de 384 tokens de saída. A saída completa desta execução está em [demo-local.txt](demo-local.txt).

| K solicitado | Recuperados | Contexto em caracteres UTF-16 |
|---:|---:|---:|
| 1 | 1 | 411 |
| 3 | 3 | 1244 |
| 5 | 3 | 1244 |

O ranking observado foi `seguranca.md` (0,695), `implantacao.md` (0,589), `retencao.md` (0,583). K=3 e K=5 enviaram o mesmo contexto; os dois documentos adicionais não tratam diretamente da proteção de credenciais. Os tamanhos incluem as quebras de linha dos arquivos no checkout Windows.

Com o modelo padrão, K=1 respondeu que credenciais devem ficar em variáveis de ambiente ou gerenciador de segredos. K=3 e K=5 produziram a mesma resposta, com referência a `[S1]`; K=1 não incluiu o ID na resposta, reforçando a necessidade de conferir a lista de fontes.

Uma tentativa anterior com `qwen2.5:0.5b` foi abortada pelo Ollama por repetição de tokens. Com `llama3.2:1b`, a execução terminou, mas o modelo declarou não encontrar a informação mesmo com a fonte correta recuperada. Isso mostra que recuperação correta não garante uma resposta correta. A qualidade deve ser avaliada conferindo os documentos, sem tomar o término da execução como prova de precisão.
