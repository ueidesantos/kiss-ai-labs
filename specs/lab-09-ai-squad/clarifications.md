# Clarifications

| Question | Decision |
|---|---|
| Is this commercial customer validation? | No. It is a fictional demo. Profile selection does not authenticate or authorize anyone. |
| What do two models guarantee? | Nothing about independence. They may share biases; tests remain mandatory. |
| How does review block? | A `high`/`critical` finding or failed test closes the gate. |
| How many corrections? | One Back correction, followed by build/tests and another review. A persistent issue ends in rejection. |
| Does offline mode use AI? | No. Proposals and reviews are prepared; tests are actually run. |
| Does RAG use embeddings? | No; it reuses Lab 07's lexical TF-IDF and cosine similarity. |
| Is Figma required? | No; it is a complementary reference only, with no claim of capture. |
| Is delivery to `main`? | The lab is on `main`; the candidate can go to a separate opt-in draft PR. |
| Which cover? | Latest request: Lab 09 featured, Lab 08 secondary; preserve the scene. |
