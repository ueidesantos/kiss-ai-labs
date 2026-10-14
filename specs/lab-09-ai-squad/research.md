# Implementation decisions

## Agents with bounded output

REQ-3 uses JSON to represent the plan, copy, and policy. Known templates materialize the application. This keeps the diff small and auditable, and keeps tests outside the model's influence. The tradeoff is explicit: this is not general-purpose autonomous programming.

## Reused RAG

REQ-2 reuses Lab 07's lexical retrieval over a small set of Markdown files. No vector database, embeddings, or additional infrastructure is needed. Paths, excerpts, and SHA256 make it possible to relate the context used to the document version. SHA256 proves integrity, not source truth.

## Tests before QA's opinion

REQ-4 separates deterministic evaluation from model commentary. Tests verify inputs, currency, and limits defined in the SPEC. QA receives evidence to explain risks; its approval cannot override tests, and its `high`/`critical` findings can block a candidate that passed the tests.

## One correction, then stop

Limiting the workflow to one round makes cost and diagnosis predictable. Another failure ends the run. The limit does not guarantee success; it prevents a demo from hiding unlimited attempts.

## Demo and real model

REQ-6 makes the first run reproducible without Ollama. The Standard=20 proposal is a prepared error, identified as such. Real mode does not inject that error or replace model unavailability with an offline response.

## Separate PR

REQ-7 uses opt-in publication on its own branch and a draft PR. Local evidence is not equivalent to human review and does not authorize automatic merging.

## Review by another model

REQ-1/4 separate developer and reviewer through Ollama identities and digests. The reviewer points to evidence and a fix; `high`/`critical` findings block along with test failures. The cost is one additional call per candidate and the possibility of disagreement. A correction requires another review. Two models do not guarantee independence; this is a complementary control, not proof that no defects remain. Claude/Codex are conceptual examples; no commercial adapters are implemented.
