# Change evidence

The delivery must make it possible to understand why a discount was accepted or rejected. The interface and backend must communicate the Standard and VIP limits consistently.

Each run records the business sources consulted and their integrity. The SPEC produced by agents is a candidate; it does not replace the approved specification. Deterministic tests are the gate, and the QA agent's opinion cannot make them pass.

At most one backend correction is allowed per run. Another failure ends the workflow with an explicit status. Publishing a draft PR requires a separate action. Never merge automatically.

A distinct model adversarially reviews each candidate. `high`/`critical` findings block approval, even when tests pass. After the one allowed correction, run the tests and request a new review; do not carry the previous approval over to the changed candidate.
