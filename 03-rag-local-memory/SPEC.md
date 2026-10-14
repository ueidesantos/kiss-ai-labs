# SPEC — Local RAG Without a Vector Database

## Purpose

Demonstrate retrieval-augmented generation (RAG) with as few components as possible: local Markdown files, Ollama embeddings, cosine similarity calculated by the program, and an answer generated from the retrieved excerpts.

## Behavior

- Reads `.md` files from a local folder without using vector storage services.
- Generates one embedding per document using Ollama’s local `/api/embed` API.
- Calculates cosine similarity in memory and selects the closest documents.
- Sends the question and retrieved context to `/api/generate` and prints the response.
- Accepts the question, document path, models, and number of results as options.
- Displays the retrieved files, citation IDs, and excerpt previews.

## Limitations

- Each Markdown file is a single document; chunking and persistence are left to later labs.
- The model’s textual citations are probabilistic; the source and excerpt list printed by the program is the verifiable reference.
- All documents and vectors remain in memory during execution.
- The documents are not sent to an external service, but their embeddings and the question go through the configured Ollama endpoint.
- Retrieval quality depends on the embedding model and the supplied content.

## Acceptance Criteria

1. Uses no vector database or RAG abstraction package.
2. Allows specifying a Markdown folder, models, endpoint, and Top-K.
3. Clearly reports when the folder does not exist or is empty, or when Ollama does not respond.
4. Performs semantic cosine search in memory before generation and displays the retrieved sources.
