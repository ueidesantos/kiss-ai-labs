# Coherence analysis

The lab turns the promise of an “AI team” into a small, observable workflow: four roles, output contracts, and one correction. The demonstrable part is proposals passing through tests the agents do not control.

## Presentation risks to avoid

- Confusing the sequential runner with autonomous agents coding in parallel.
- Calling TF-IDF retrieval semantic embeddings.
- Presenting an injected offline error as the model's spontaneous discovery.
- Promising free-form code when the mechanism uses templates.
- Treating QA commentary as proof of correctness.
- Treating a report or PR's existence as human approval.
- Claiming a Figma capture when there is only a reference link.

REQ-1 and REQ-6 require mode labels in events and reports. REQ-3 requires explaining template materialization. REQ-4 prevents approval when tests fail. REQ-7 retains human review in the draft PR.

## Coverage limits

Fixed tests demonstrate expected cases, not the absence of all defects. Running the demo does not prove compatibility with a specific Ollama model. A build does not prove visual accessibility or GitHub operation. These checks must appear separately in the final record.

Review by another model adds an adversarial gate. Do not equate two aliases with two models or present prepared mode as real review. Identity is verifiable; reasoning independence is not guaranteed. After a candidate changes, the previous review is no longer valid: the correction goes through a new round.
