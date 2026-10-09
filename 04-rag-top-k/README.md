# 04 — RAG com Top-K Configurável

![Capa original KISS AI Labs](../assets/kiss_ai_lab.png)

Quanto contexto enviar ao modelo? Compare vários valores de K com a mesma pergunta, os mesmos embeddings e o mesmo ranking. Este lab aprofunda o `--top-k` do [RAG Local](../03-rag-local-memory/README.md) com uma comparação na mesma execução.

## Executar

Pré-requisitos: .NET 11 SDK (incluindo SDK preview com suporte a `net11.0`), Ollama em execução e modelos baixados. Na raiz do repositório:

```bash
ollama pull nomic-embed-text
ollama pull llama3.2
dotnet build 04-rag-top-k/src/RagTopK.csproj
dotnet run --project 04-rag-top-k/src -- --question "Como protegemos credenciais?" --compare 1,3,5
```

Para uma rodada ou seus próprios documentos:

```bash
dotnet run --project 04-rag-top-k/src -- --top-k 2
dotnet run --project 04-rag-top-k/src -- --docs ./meus-documentos --question "Qual é a política de retenção?" --compare 1,2,3
```

Opções: `--docs`, `--question`, `--url`, `--embedding-model`, `--model`, `--top-k`, `--compare` e `--help`. O padrão é K=3, `nomic-embed-text`, `llama3.2` e `http://localhost:11434`. Use `--top-k` ou `--compare` separadamente. Valores de K devem ser inteiros positivos; repetidos são ignorados, preservando a ordem.

## Experimento

```text
Markdown → embeddings uma vez → ranking único por cosseno
                                     │
                        ┌────────────┼────────────┐
                       K=1          K=3          K=5
                        │            │            │
                 contexto + pergunta → geração por rodada
                        │            │            │
                 arquivos, scores, contexto, resposta e fontes
```

Os três arquivos de exemplo tratam de segurança, retenção e implantação. Cada arquivo inteiro é um chunk. O programa imprime o contexto completo enviado em cada rodada e seu tamanho em caracteres UTF-16, sem estimar tokens.

1. Confira se `seguranca.md` aparece primeiro para a pergunta sobre credenciais.
2. Compare K=1 com K=3: a informação adicional ajudou ou trouxe ruído? Confira se a resposta se sustenta nas fontes.
3. K=5 recupera os mesmos três documentos de K=3, pois só existem três. O console distingue K solicitado da quantidade recuperada.
4. Experimente uma pergunta que exija mais de uma fonte: “Como protegemos credenciais e por quanto tempo guardamos logs?”

Mais contexto pode ajudar perguntas amplas e prejudicar perguntas específicas. A resposta e os scores dependem dos modelos. Temperatura zero e seed fixa reduzem variações; não garantem determinismo ou precisão. A saída é limitada a 384 tokens por rodada e pode terminar antes de completar a resposta. Não há avaliação automática de qualidade nem filtro de similaridade.

## Dados e falhas comuns

Os vetores ficam em memória e são recalculados na próxima execução. O conteúdo dos arquivos vai ao endpoint Ollama para embeddings; pergunta e documentos recuperados vão para geração. Com endpoint remoto, os textos deixam sua máquina. O console também exibe o conteúdo recuperado: use dados adequados para essa demonstração.

- Pasta ausente ou sem `.md`: confira `--docs`; execute os exemplos na raiz do repositório.
- Modelo ausente ou conexão recusada: baixe os modelos e confira o serviço e `--url`.
- Documento grande demais: embeddings rejeitam truncamento; reduza os arquivos. A geração está sujeita à janela de contexto do modelo.
- K inválido, opção sem valor ou opções incompatíveis: consulte `--help`; a execução retorna código 1.
- Citação imprecisa: compare `[S1]` e demais IDs com a lista e os arquivos. O programa não valida afirmações do LLM.

## Validação

Teste de integração sem modelos, em PowerShell 7, na raiz:

```powershell
pwsh -File 04-rag-top-k/tests/validate.ps1
```

O teste usa um servidor HTTP temporário no loopback para conferir ranking, contexto, quantidade de chamadas, K acima do total e erros de entrada. Ele não avalia qualidade semântica. Veja [VALIDATION.md](VALIDATION.md) para o resultado com Ollama real e [SPEC.md](SPEC.md) para os critérios.
