# 06 — RAG Local com SQLite-vec

![KISS AI Labs com SQLite-vec](../assets/kiss-ai-labs-sqlite-vec.png)

Indexe documentos Markdown uma vez e reutilize textos e embeddings num arquivo SQLite. Python chama o Ollama por HTTP; a extensão SQLite-vec calcula a distância de cosseno em SQL. Não há serviço de banco externo.

**Estado desta entrega:** implementação e testes preparados; validação da extensão nativa e demonstração com Ollama pendentes neste ambiente. Consulte [VALIDATION.md](VALIDATION.md).

## Executar

Pré-requisitos: Python 3.10 ou superior com `sqlite3` capaz de carregar extensões, SQLite 3.41 ou superior recomendado, Ollama em execução e os modelos abaixo. Execute na raiz do repositório:

```bash
python -m venv .venv
```

Ative o ambiente no PowerShell com `.\.venv\Scripts\Activate.ps1`, ou no Linux/macOS com `source .venv/bin/activate`. Depois:

```bash
python -m pip install -r 06-rag-sqlite-vec/requirements.txt
ollama pull nomic-embed-text
ollama pull llama3.2
python 06-rag-sqlite-vec/src/rag.py index --cpu
python 06-rag-sqlite-vec/src/rag.py ask --question "Como protegemos credenciais?" --top-k 1 --cpu
python 06-rag-sqlite-vec/src/rag.py ask --question "Por quanto tempo guardamos logs?" --top-k 2 --cpu
```

`--cpu` é opcional: envia `num_gpu=0` tanto para embeddings quanto para geração. Nos labs anteriores, resolveu uma falha de repetição de tokens observada na execução padrão do Ollama. Sem essa opção, o Ollama decide como executar os modelos.

Para seus documentos:

```bash
python 06-rag-sqlite-vec/src/rag.py index --docs ./meus-documentos --db ./meu-rag.sqlite3
python 06-rag-sqlite-vec/src/rag.py ask --db ./meu-rag.sqlite3 --question "Qual é a política de retenção?"
```

O diretório pai de `--db` precisa existir. O arquivo padrão é `06-rag-sqlite-vec/rag.sqlite3`, ignorado pelo Git. Use um banco exclusivo do lab; arquivos SQLite com outras tabelas são recusados.

## Indexar e consultar

```text
index: Markdown → Ollama /api/embed → textos + vetores float32 → rag.sqlite3
ask:   pergunta → Ollama /api/embed → distância de cosseno em SQL → Top-K
                                      → contexto + pergunta → Ollama /api/generate
```

`index` procura `.md` e `.MD` recursivamente, ignora arquivos vazios e links simbólicos de arquivos, e trata cada arquivo inteiro como uma unidade. Os arquivos de exemplo discutem credenciais, retenção e implantação.

O índice guarda `documents` (caminho relativo, texto e vetor) e `metadata` (formato, dimensão, modelo de embedding e endpoint). A consulta usa `vec_distance_cosine`, uma função da extensão SQLite-vec, com `ORDER BY distance, path LIMIT K`: menor distância significa maior proximidade. É uma varredura exata da tabela, adequada para este lab pequeno; não utiliza tabela virtual `vec0` nem índice aproximado. Veja a [documentação oficial de consultas KNN](https://alexgarcia.xyz/sqlite-vec/features/knn.html).

`ask` não acessa a pasta de documentos: gera apenas o embedding da pergunta, consulta os vetores persistidos e faz uma chamada de geração. Imprime caminhos, distâncias, IDs `[S1]`, contexto completo e resposta. Confira a resposta contra esse contexto; citações e afirmações do modelo não são validadas automaticamente.

Modificou, adicionou ou removeu documentos? Execute `index` novamente. A reconstrução gera todos os embeddings e substitui o conjunto anterior numa única transação, sem atualização incremental. Uma falha durante a geração ou gravação preserva o índice anterior. Arquivos vazios não apagam o índice anterior. A primeira execução que falha pode deixar um arquivo SQLite vazio, reutilizável pelo próximo `index`.

## Opções

| Comando | Opção | Padrão |
|---|---|---|
| Ambos | `--db` | `06-rag-sqlite-vec/rag.sqlite3` |
| Ambos | `--url` | `http://localhost:11434` |
| Ambos | `--cpu` | Ausente; Ollama decide o dispositivo |
| index | `--docs` | Documentos de exemplo do lab |
| index | `--embedding-model` | `nomic-embed-text` |
| ask | `--question` | `Como protegemos credenciais?` |
| ask | `--top-k` | 2; inteiro positivo |
| ask | `--model` | `llama3.2` |

Use `python 06-rag-sqlite-vec/src/rag.py index --help` ou `ask --help`. `ask` usa o modelo de embedding gravado no índice e exige o mesmo endpoint. Para mudar modelo/endpoint, reindexe. Uma mudança na dimensão é recusada; uma atualização do modelo sob o mesmo nome com a mesma dimensão não é detectada, portanto também exige reindexação manual.

## Dados, limites e falhas comuns

Textos completos vão ao endpoint para indexação; a pergunta vai para embedding em cada consulta; pergunta e documentos selecionados vão para geração. Com endpoint e modelos locais, esse processamento ocorre na máquina. Endpoint remoto recebe os textos. O SQLite contém textos e embeddings sem criptografia, e o terminal exibe o contexto.

Sem chunking, filtros mínimos, persistência da pergunta ou estimativa de tokens. Arquivos grandes podem exceder a janela de embedding (truncamento desativado). A geração não usa streaming, tem timeout de 300 segundos e limite de 384 tokens, podendo terminar antes de completar a resposta. Temperatura zero e seed fixa não garantem precisão.

- Dependência ausente: instale `requirements.txt` no mesmo Python que executa o lab. A versão foi fixada em `0.1.9` para reprodução, mas ainda precisa ser validada neste ambiente.
- Python sem suporte a extensões: use uma distribuição que exponha `enable_load_extension`; consulte o [guia oficial de Python](https://alexgarcia.xyz/sqlite-vec/python.html).
- Banco ausente: execute `index` primeiro. Pasta sem Markdown não modifica o índice anterior.
- Modelo ausente, rede ou timeout: confira Ollama, os modelos baixados e `--url`.
- Modelo ou dimensão diferente: reindexe com a configuração desejada.
- Argumentos inválidos retornam código 2; erros de execução retornam código 1.

## Validar

```bash
python -m py_compile 06-rag-sqlite-vec/src/rag.py 06-rag-sqlite-vec/tests/test_rag.py
python 06-rag-sqlite-vec/tests/test_rag.py
```

A suíte usa o SQLite-vec real para persistência e ranking, e substitui apenas as respostas HTTP do Ollama por dados controlados. Não exige serviço Ollama ou modelos baixados. Se a extensão estiver ausente, os testes nativos são omitidos e o comando retorna 1 para sinalizar validação incompleta. A demo real acima avalia o fluxo semântico e deve ser executada separadamente.

[Especificação](SPEC.md) · [Registro de validação](VALIDATION.md)
