# Front-end handoff

Role: Front Dev / implementation of REQ-3 and AC-3, per `specs/lab-07-sdd-agents/SPEC.md`.

- `index.html`: vanilla HTML/CSS/JS, no dependencies or build step.
- Backend contract: ASP.NET Core / .NET 11; the frontend uses HTTP/JSON only.
- Labeled fields, positive order amount, 0–20% discount, and a message within the policy limit.
- POST `/api/discount` with numeric `amount` and `discount`; returned string values are formatted as BRL. The API calculates and validates the rules; the page only displays the result.
- Calculation, success, and error states; button disabled during the call, status and alert regions, messages inserted through `textContent`.
- Previous result hidden when simulating again; transport/JSON failures receive a readable message.

## Evidence

Validation performed with bundled Node.js on 2026-10-11:

1. Extracted the `<script>` and compiled with `node:vm.Script`: `JavaScript syntax OK`.
2. Ran the script with simulated DOM and `fetch`: POST/numeric payload, `90.00` formatting, API error, rejection of 21% without an HTTP call, and button restoration all passed.
3. Reproducible test: `node --test tests/frontend.test.cjs`, run from the lab directory. Nine tests cover syntax/labels, payload/BRL, loading and duplicate-submission protection, API error, invalid inputs, transport/contract failures, and currency precision.
4. QA fix: `100000000000000.01` is rejected before fetch if conversion to a JSON number loses digits. Normalized decimal identity accepts scientific notation and equivalent trailing zeros without an arbitrary currency cap. Currency responses are formatted directly from strings to preserve cents for large values. The amount field allows any positive increment; the API remains responsible for rules and rounding.

This validation did not run a browser or the real server. HTTP integration and visual review remain with the coordinator/reviewer; Front Dev did not approve their own work.
