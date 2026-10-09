# SPEC — AI Code Reviewer

## Objetivo

Demonstrar uma aplicação de IA aplicada ao desenvolvimento: revisar mudanças do Git com um modelo local via Ollama e retornar possíveis problemas em português.

## Comportamento

- Lê o diff das alterações não staged por padrão ou somente as staged com `--staged`.
- Envia o diff para o endpoint `/api/generate` do Ollama.
- Pede ao modelo que priorize bugs, riscos de segurança e regressões, sem sugestões cosméticas.
- Exibe a resposta no terminal e nunca altera arquivos.
- Limita o tamanho do diff enviado e informa quando o conteúdo foi truncado.

## Parâmetros

| Opção | Padrão | Descrição |
|---|---|---|
| `--model` | `llama3.2` | Modelo Ollama instalado |
| `--url` | `http://localhost:11434` | Endpoint do Ollama |
| `--staged` | desativado | Revisa somente alterações staged |
| `--max-diff-chars` | `24000` | Máximo de caracteres enviados ao modelo |
| `--help` | — | Exibe a ajuda |

## Segurança e limites

- O diff é tratado como conteúdo não confiável. Instruções encontradas no código devem ser ignoradas pelo modelo.
- O conteúdo não sai do ambiente local se o Ollama estiver rodando localmente.
- A revisão é uma sugestão probabilística; o desenvolvedor continua responsável por validar os achados.
- Arquivos não rastreados não aparecem em `git diff` e não são revisados.

## Critérios de aceite

1. Usa somente recursos padrão do .NET e Git instalado.
2. Retorna orientação clara quando não há diff, Git não está disponível ou Ollama está offline.
3. Permite selecionar modelo, endpoint, alterações staged e limite de entrada.
4. Nunca grava nem modifica arquivos do repositório.
