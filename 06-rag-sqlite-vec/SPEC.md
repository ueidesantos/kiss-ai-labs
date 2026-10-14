# SPEC — Local RAG with SQLite-vec

## Problem

Query small local documents without recalculating their embeddings for every question and without running a vector database service.

## Requirements

- Python, Ollama, and the SQLite-vec extension; only the standard library besides the extension.
- Separate `index` and `ask` commands with configurable folder, database, endpoint, models, and CPU settings.
- Index whole Markdown files; persist text, float32 vectors, and embedding configuration.
- Rebuild the index in a transaction; failures preserve the previous set, and deleted files disappear after reindexing.
- Reject databases from other projects, missing indexes, and embeddings that are empty, non-finite, have zero norm, or incompatible dimensions.
- Query cosine distance in SQL, retrieve Top-K, and show sources/context before generating an answer.
- Reuse persisted documents even without the original files, without reindexing during `ask`.
- Document data flows and limitations.

## Acceptance Criteria

- Python compilation and input/HTTP contract tests pass.
- Tests with real SQLite-vec pass: persistence across connections, ranking, K above the total, removal after reindexing, index preservation on failures, DDL rollback, and rejection of incompatible configuration.
- Demo with local Ollama asks about credentials/retention and records the model, question, sources, and observed result.
- README, backlog, and cover include the lab without claiming validation/publication that has not occurred.

## Limitations

Exact scan via `vec_distance_cosine`, with no `vec0`, chunking, incremental indexing, filters, or citation validation. A model update under the same name and dimensions requires manual reindexing. Reproducible installation/configuration does not guarantee deterministic model responses.
