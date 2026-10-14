# SPEC — RAG over Source Code

## Problem

Find the class that validates a rule and where it is applied in a small C# code folder, with references to verify the answer.

## Requirements

- C#/.NET 11 console app, explicit HTTP calls to Ollama, no external dependencies.
- Recursively discover `.cs` files, ignoring links and generated folders (`bin`, `obj`, `.git`, `.vs`, `node_modules`).
- Generate one embedding per file and one for the question; rank by cosine similarity in memory.
- Retrieve a configurable positive Top-K and show context, scores, the answer, and sources with paths and line ranges.
- Request an answer in Portuguese based only on the context and including source IDs.
- Configure the folder, question, endpoint, models, and optional CPU execution; report failures with exit code 1.

## Acceptance Criteria

- Build with no warnings or errors; HTTP test checks discovery, exclusions, ranking, sources, context, and invalid inputs.
- Demo with local Ollama identifies the discount rule in the samples; record the models, question, and actual output.
- README explains how to run the lab, data flows, limitations, and the lack of syntax analysis.

## Limitations

Whole files and embeddings remain in memory. There is no parser, method-level splitting, call graph, persistence, or automatic validation of claims/citations. Line ranges refer to the entire file. Semantic search may miss real relationships or retrieve irrelevant context.
