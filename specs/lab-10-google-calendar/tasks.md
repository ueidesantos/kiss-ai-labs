# Lab 10 Implementation Tasks

| ID | Task | Requirements | Owner | Depends on | Status |
|---|---|---|---|---|---|
| T1 | Create the .NET 11 console project and local configuration for Ollama and the booking URL. | REQ-1, REQ-5, NFR-2, NFR-4 | Implementation agent | Gate 2 | Complete |
| T2 | Collect role/candidate context interactively (including recruiter-provided skills/experience); validate required inputs and HTTPS booking URL. | REQ-1, REQ-6 | Implementation agent | T1 | Complete |
| T3 | Generate a grounded Brazilian Portuguese draft with the local Ollama endpoint; append the exact URL in application code. | REQ-2, REQ-3 | Implementation agent | T2 | Complete |
| T4 | Add README, setup guidance, synthetic walkthrough, and Google appointment schedule instructions. | REQ-5, REQ-7 | Coordinator / implementation agent | T1–T3 | Complete |
| T5 | Feature Lab 10 in the root README and site; document Tracer AI → Figma → AI agents → human review → GitHub. | REQ-7 | Coordinator | T4 | Complete |
| T6 | Build and record exact commands and live-flow limitations. | All | Coordinator / QA | T1–T5 | Build passed; live booking pending |
| T7 | Have a separate AI/model perform adversarial review and address findings. | REQ-8 | Independent reviewer / implementation agent | T6 | Complete |
| T8 | Commit and push after required gates pass. | REQ-9 | Coordinator | T7 | Pending |

## Rollback

The lab is additive. Revert its commit to remove it; no production Calendar data, messaging service, or external account configuration is modified by the app.
