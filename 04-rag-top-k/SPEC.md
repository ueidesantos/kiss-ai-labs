# SPEC — RAG with Configurable Top-K

## Purpose

Compare the number of documents retrieved for the same question, making context, relevance, and noise visible without adding a vector database or RAG framework.

## Behavior

- Indexes complete Markdown files and calculates the cosine ranking once per run.
- `--top-k N` runs one round (default: 3); `--compare 1,3,5` runs multiple rounds using the same embeddings and ranking.
- Preserves the requested value order and removes duplicates.
- Limits retrieval to the number of available files and shows the requested K and the effective count.
- Displays files, scores, full context, size in UTF-16 characters, the answer, and sources with `[S1]` IDs.
- Uses the same models, question, temperature zero, seed 42, and 384-token output limit across rounds; identical responses across machines are not guaranteed.
- Validates options before calling Ollama and exits with code 1 on errors.

## Limitations

- Each file is one chunk; there is no chunk splitting, persistence, or minimum-similarity threshold.
- A higher K includes less similar results: more context does not guarantee greater accuracy.
- Quality is not measured automatically; comparison requires checking the answer and sources.
- Characters are not tokens. Generation context is still limited by the model and may be truncated by Ollama; use small documents.
- The embeddings API receives `truncate=false` to reject documents larger than the embedding model’s context window.
- Document and question text is sent to the configured endpoint; local processing requires a local endpoint.
- Generated citations are probabilistic; the displayed source list allows manual verification.

## Acceptance Criteria

1. `--compare 1,3,5` uses one embedding per file and one for the question, with one generation per distinct K.
2. With three documents, the rounds retrieve 1, 3, and 3 files, respectively.
3. Ranking and IDs remain consistent across rounds; the printed context matches the context sent for generation.
4. Zero, negative values, malformed lists, missing values, and combining `--top-k` with `--compare` fail clearly.
5. Builds on .NET 11 and works with the Ollama HTTP API.
