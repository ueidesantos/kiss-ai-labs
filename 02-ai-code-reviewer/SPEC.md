# SPEC — AI Code Reviewer

## Purpose

Demonstrate an AI application for software development: review Git changes with a local model through Ollama and report potential issues. The current implementation returns its review in Portuguese.

## Behavior

- Reads the diff for unstaged changes by default, or only staged changes with `--staged`.
- Sends the diff to Ollama’s `/api/generate` endpoint.
- Asks the model to prioritize bugs, security risks, and regressions, without cosmetic suggestions.
- Displays the response in the terminal and never modifies files.
- Limits the size of the submitted diff and reports when content was truncated.

## Parameters

| Option | Default | Description |
|---|---|---|
| `--model` | `llama3.2` | Installed Ollama model |
| `--url` | `http://localhost:11434` | Ollama endpoint |
| `--staged` | disabled | Reviews staged changes only |
| `--max-diff-chars` | `24000` | Maximum characters sent to the model |
| `--help` | — | Displays help |

## Security and Limitations

- The diff is treated as untrusted content. The model should ignore instructions found in the code.
- Content stays in the local environment when Ollama is running locally.
- The review is probabilistic guidance; the developer remains responsible for validating its findings.
- Untracked files do not appear in `git diff` and are not reviewed.

## Acceptance Criteria

1. Uses only built-in .NET features and installed Git.
2. Provides clear guidance when there is no diff, Git is unavailable, or Ollama is offline.
3. Allows selecting the model, endpoint, staged changes, and input limit.
4. Never writes or modifies repository files.
