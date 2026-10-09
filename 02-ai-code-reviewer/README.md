# 02 — AI Code Reviewer

Um revisor de código local, pequeno e direto: pega um diff do Git, envia para o Ollama e pede uma revisão focada em bugs, segurança e regressões. A resposta aparece no terminal; nenhum arquivo é alterado.

## Como funciona

```text
Git diff ──▶ App .NET ──HTTP──▶ Ollama / modelo local
                                  │
Terminal ◀── revisão em português ◀┘
```

O diff é limitado por tamanho e apresentado ao modelo como conteúdo não confiável, para reduzir a chance de instruções escritas dentro do código desviarem a revisão.

## Pré-requisitos

- .NET 11 SDK
- Git no `PATH`
- Ollama instalado e rodando
- Um modelo baixado, por exemplo:

```bash
ollama pull llama3.2
```

## Executar

Na raiz do repositório:

```bash
# Revisa mudanças ainda não staged
dotnet run --project 02-ai-code-reviewer/src

# Revisa somente o que já foi staged
dotnet run --project 02-ai-code-reviewer/src -- --staged

# Escolhe modelo e endpoint
dotnet run --project 02-ai-code-reviewer/src -- --model qwen3 --url http://localhost:11434
```

## Opções

```text
--model <nome>             Modelo Ollama (padrão: llama3.2)
--url <endpoint>           URL do Ollama (padrão: http://localhost:11434)
--staged                   Usa git diff --cached
--max-diff-chars <número>  Limite de entrada (padrão: 24000)
--help                     Mostra a ajuda
```

Se não houver alterações para revisar, o programa informa isso e encerra sem chamar o modelo. Diffs grandes são truncados com um aviso visível.

## O que aprender

1. Como chamar um LLM local usando `HttpClient` e a API REST do Ollama.
2. Como alimentar uma tarefa real de desenvolvimento com contexto obtido pelo Git.
3. Como limitar entrada e orientar o modelo a ignorar instruções presentes no conteúdo analisado.
4. Por que revisão por IA ajuda na triagem, mas não substitui revisão humana.

Consulte [`SPEC.md`](SPEC.md) para os requisitos e limites do exemplo.
