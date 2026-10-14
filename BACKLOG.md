# Backlog — KISS AI Labs

Current learning path: in-memory semantic retrieval, sources, Top-K comparison, source-code search, and local persistence.

| Order | Lab | Stack | Expected outcome | Dependency | Status |
|---:|---|---|---|---|---|
| 1 | **Local RAG without a Vector Database** | C#, .NET 11, Ollama, embeddings | Ask questions about Markdown; generate embeddings, rank by cosine similarity in memory, and answer using retrieved context. | — | Completed in `03-rag-local-memory/` |
| 2 | **RAG with Source Citations** | C#, .NET 11, Ollama, embeddings | Show the file and excerpt used to answer. | 1 | Included in `03-rag-local-memory/` |
| 3 | **RAG with Configurable Top-K** | C#, .NET 11, Ollama | Vary the number of retrieved excerpts and observe context, precision, and noise. | 1 | Completed in `04-rag-top-k/` |
| 4 | **RAG over Source Code** | C#, .NET 11, Ollama, embeddings | Index `.cs` files and find rules, classes, and validations. | 1, 2 | Completed in `05-rag-source-code/` |
| 5 | **Local RAG with SQLite-vec** | Python, Ollama, SQLite-vec | Persist embeddings locally for repeatable indexing without an external service. | Concepts from 1 | Implemented in `06-rag-sqlite-vec/`; native validation and publication are pending |

## SDD, agents, and design

Added at the user's direct request. Items removed from the roadmap remain outside the learning path.

| Lab | Deliverable | Status |
|---|---|---|
| 07 | SDD + agents + business RAG; complete kit and .NET 11 simulator | Core/tests approved; real integration and publication pending |
| 08 | Figma-based specification via MCP + TL/Front/Back + RAG | Client and replay implemented; Figma link delivered as an accepted alternative; MCP capture and visual review not validated |
| 09 | KISS AI Squad: SDD + RAG + development and review by separate models | Implemented; evidence and limitations in [VALIDATION.md](09-ai-squad/VALIDATION.md) |

## Shared completion criteria

- Every lab has a `SPEC.md`, run instructions, and a demo question.
- Endpoints and models can be configured; prerequisites and common failures are documented.
- The example explains what stays local and what data is sent to the configured model.
- Code keeps the flow explicit and avoids abstractions that are not needed to teach the concept.

## Delivery pending validation/publication

Lab 06 already has code, documentation, and prepared tests. Run the tests with SQLite-vec installed and the demo with Ollama before calling this lab complete. See `06-rag-sqlite-vec/VALIDATION.md`.
