# SPEC — Hello Ollama

## Objetivo

Demonstrar a integração mais simples possível entre uma aplicação .NET 11 Console e um LLM executado localmente via **Ollama**, usando apenas chamadas HTTP nativas.

## Comportamento Esperado

A aplicação envia um prompt para um modelo do Ollama (via API REST local) e exibe a resposta completa no console.

## Entrada

| Parâmetro       | Tipo   | Default       | Descrição                              |
|-----------------|--------|---------------|----------------------------------------|
| `--model`       | string | `llama3.2`    | Nome do modelo Ollama já instalado    |
| `--prompt`      | string | `Olá!`        | Mensagem enviada ao modelo             |
| `--url`         | string | `http://localhost:11434` | Endereço do Ollama          |

## Saída

- **Console**: Resposta do modelo Ollama, linha a linha conforme stream (ou completa se streaming falhar).
- **Exit Code**:
  - `0` = sucesso
  - `1` = erro (ex: Ollama offline, modelo não instalado)

Mensagens de erro devem ser claras e didáticas (ex: "Verifique se o Ollama está rodando").

## Dependências

- [Ollama](https://ollama.com/) instalado e rodando localmente (porta 11434)
- .NET 11 SDK
- Modelo Ollama baixado (ex: `ollama pull llama3.2`)

## Critérios de Aceite

1. ✅ `dotnet run` executa sem erros com valores default
2. ✅ `dotnet run -- --model qwen3 --prompt "Oi"` usa parâmetros customizados
3. ✅ Retorna mensagem amigável se Ollama não estiver rodando
4. ✅ Retorna mensagem amigável se modelo não estiver instalado
5. ✅ Nenhuma dependência NuGet além do SDK padrão (sem bibliotecas de terceiros)

## Como Executar

```bash
# 1. Instale e inicie o Ollama (se ainda não estiver rodando)
ollama serve  # ou apenas abra o app Ollama

# 2. Baixe um modelo (uma vez)
ollama pull llama3.2

# 3. Rode o exemplo (default)
cd src
dotnet run

# 4. Com parâmetros customizados
dotnet run -- --model qwen3 --prompt "Explique .NET 11 em 1 frase"
```
