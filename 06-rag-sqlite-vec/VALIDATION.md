# Validação — RAG Local com SQLite-vec

Execução em 2026-10-11, America/Sao_Paulo. **Validação completa e publicação pendentes.**

## Executado

- Compilação com `py_compile` passou para a aplicação e a suíte de testes. Links locais, UTF-8, assinatura/dimensões do PNG (1671x941) e `git diff --check` validados; capa revisada visualmente.
- Dez testes de entrada e contrato HTTP passaram: vetores float32, dimensões, valores inválidos, argumentos, banco ausente, pasta vazia, payload UTF-8, erro HTTP e opções de embedding/CPU.
- Oito testes que exigem SQLite-vec real ficaram omitidos porque a extensão não está instalada. A suíte retorna código 1 explicitamente nesse caso; não é considerada aprovada integralmente.
- Implementação conferida com a documentação oficial: [Python e carregamento da extensão](https://alexgarcia.xyz/sqlite-vec/python.html), [KNN com funções escalares e distância de cosseno](https://alexgarcia.xyz/sqlite-vec/features/knn.html).

## Bloqueios do ambiente

- `pip install sqlite-vec==0.1.9` não conseguiu obter a distribuição no ambiente com rede restrita. A versão consta no [PyPI oficial](https://pypi.org/project/sqlite-vec/0.1.9/), mas não foi instalada nem validada aqui.
- `ollama list` não conseguiu acessar `127.0.0.1:11434`: socket bloqueado pelas permissões. A demonstração semântica real não foi executada; nenhuma saída de modelo foi inventada.
- Fetch do GitHub falhou com `Could not resolve host: github.com`. O trabalho será commitado localmente; push depende do acesso ao GitHub e da validação completa.

## Retomar a validação

```bash
python -m pip install -r 06-rag-sqlite-vec/requirements.txt
python -m py_compile 06-rag-sqlite-vec/src/rag.py 06-rag-sqlite-vec/tests/test_rag.py
python 06-rag-sqlite-vec/tests/test_rag.py
python 06-rag-sqlite-vec/src/rag.py index --cpu
python 06-rag-sqlite-vec/src/rag.py ask --question "Como protegemos credenciais?" --top-k 1 --cpu
```

Registre a saída real e atualize este arquivo e o estado no backlog depois que a suíte completa passar. Antes do push, sincronize `main` e confira o histórico diário para preservar o limite de um novo lab por data de São Paulo.
