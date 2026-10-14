# Requirements checklist

Based on the [SPEC](../SPEC.md) and [analysis](../analysis.md).

- [x] Request identifies user, goal, and example outcome.
- [x] Scope excludes charges, order persistence, login, and management approval.
- [x] Sources B1/B2/B3 and requirements REQ-1..6 are identified.
- [x] Every requirement has an acceptance criterion and task.
- [x] .NET 11/ASP.NET Core and vanilla frontend were defined before delivery.
- [x] API contract is shared; calculation uses decimal.
- [x] TL delegates exactly to Front/Back and maintains role boundaries.
- [x] Invalid refs and divergent contracts are rejected by validation.
- [x] Replay records that no model was called; proposed code is not run.
- [x] Core tests and builds have reported results without overstating evidence.
- [ ] AC-3/AC-4 verified in a browser and over real HTTP.
- [ ] AC-1/AC-2/AC-5/AC-6 demonstrated with real Ollama and human content review.
- [ ] Remote publication confirmed.

Conclusion: ready for core review; full validation and publication still depend on pending integrations. A process checklist is not automatic approval of proposals.
