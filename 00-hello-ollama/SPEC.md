# SPEC — Hello Ollama

## Purpose

Demonstrate the simplest possible integration between a .NET 11 Console application and an LLM running locally through **Ollama**, using only native HTTP calls.

## Expected Behavior

The application sends a prompt to an Ollama model (through its local REST API) and displays the complete response in the console.

## Input

| Parameter       | Type   | Default       | Description                             |
|-----------------|--------|---------------|----------------------------------------|
| `--model`       | string | `llama3.2`    | Name of an installed Ollama model       |
| `--prompt`      | string | `Olá!`         | Message sent to the model               |
| `--url`         | string | `http://localhost:11434` | Ollama address                 |

## Output

- **Console**: Ollama model response, printed line by line as it streams (or in full if streaming fails).
- **Exit Code**:
  - `0` = success
  - `1` = error (for example, Ollama is offline or the model is not installed)

Error messages should be clear and helpful (for example, "Check that Ollama is running").

## Dependencies

- [Ollama](https://ollama.com/) installed and running locally (port 11434)
- .NET 11 SDK
- Ollama model downloaded (for example, `ollama pull llama3.2`)

## Acceptance Criteria

1. ✅ `dotnet run` runs without errors using the defaults
2. ✅ `dotnet run -- --model qwen3 --prompt "Oi"` uses custom parameters
3. ✅ Returns a helpful message if Ollama is not running
4. ✅ Returns a helpful message if the model is not installed
5. ✅ No NuGet dependencies beyond the standard SDK (no third-party libraries)

## How to Run

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
