# SPEC — Lab 10: AI Recruiter Outreach + Google Calendar

- **ID:** LAB-010
- **Status:** Approved for implementation — educational prototype
- **Product owner:** User
- **Coordinator:** Codex
- **Last updated:** 2026-10-20

## 1. Problem and context

KISS AI Labs needs a small, hands-on example of an AI agent supporting a recruiter invitation flow with Google Calendar. The supplied SAM AI Agent Recruiter notes describe a discovery effort; production rules, candidate consent, and messaging channels remain unconfirmed. The user approved an educational prototype in which the recruiter prepares and manually sends a candidate invitation containing vacancy information and a Google Calendar booking-page link.

## 2. Goal and expected outcome

Deliver a minimal .NET lab where a recruiter provides a few vacancy/candidate details and a Google Calendar booking-page link. A local AI model prepares a concise invitation in Brazilian Portuguese. The recruiter reviews, copies, and sends it through a channel they choose. The candidate follows the link and books an available interview slot; Google Calendar handles availability, conflicts, and the booking event.

The prototype must demonstrate the user's requested workflow without inventing production recruiting rules or automating contact. The recruiter may provide relevant skills and experience gathered from a candidate's public professional profile; the app does not search the web. Lab 09's separate-model workflow plus a human review gate will be used before the reviewed work is pushed to GitHub.

Reusable delivery path: Tracer AI → Figma → AI Tech Lead Agent → AI Dev Agent → AI Adversarial Review Agent → Human Review → GitHub. Lab 10 demonstrates the Google Calendar booking step within that path.

## 3. Users and journeys

- **Users:** .NET learners and a recruiter exploring a lightweight interview invitation workflow.
- **Primary journey:** The recruiter reviews publicly available candidate information and the role; creates a Google Calendar appointment schedule and copies its booking link; enters relevant skills/experience and role-fit context; reviews and manually sends the generated invitation; the candidate accepts by booking through Google's page.
- **Relevant states:** setup guidance, missing configuration, ready for input, generated draft, empty/invalid input, unavailable local model, and recovery guidance.

## 4. Scope

### In scope
- A small .NET console app using a local Ollama model to prepare an invitation draft in Brazilian Portuguese.
- Inputs for candidate first name (optional), company name (optional), role title, short role description, recruiter-provided candidate-fit context (including relevant skills or experience already gathered), and the recruiter's booking-page URL.
- Preserve the configured booking URL exactly in the final draft.
- Recruiter reviews, copies, and sends the message using any existing communication channel.
- The candidate books an available slot on the recruiter's Google Calendar appointment schedule.
- Instructions for creating/configuring an appointment schedule, copying its link, running the sample, and revoking access.
- Synthetic sample input; no candidate data is committed.
- SDD traceability, validation evidence, review by a separate AI/model, resolution of critical findings, and GitHub publication.

### Out of scope
- Sending or scheduling messages through WhatsApp, email, SMS, LinkedIn, or another service.
- Reading, listing, or changing calendar events through the Google Calendar API.
- Creating, changing, or canceling appointment schedules from the sample.
- Automatic candidate screening, ranking, consent inference, reminders, rescheduling, or cancellation.
- Implementing or deploying the production SAM AI Agent Recruiter workflow.
- Automated web search, scraping, or retrieval of candidate profile data.
- Real candidate data in the demo, screenshots, prompts included in the repository, or validation evidence.

## 5. Functional requirements

| ID | Requirement | Priority | Acceptance criterion/criteria |
|---|---|---|---|
| REQ-1 | The app must collect a role title, short role description, recruiter-provided candidate-fit context (including relevant skills or experience already gathered), and optional candidate first name and company name without placing those values in shell history. | Must | AC-1 |
| REQ-2 | The app must use the configured local Ollama model to produce a concise, professional Brazilian Portuguese invitation draft based only on supplied details; it must not invent vacancy terms or candidate attributes. | Must | AC-2 |
| REQ-3 | The final invitation must include the configured booking-page URL exactly as provided. | Must | AC-3 |
| REQ-4 | The app must display the draft for the recruiter to copy; it must not send a message or persist candidate/vacancy input. | Must | AC-4 |
| REQ-5 | The app must explain how to create and share a Google Calendar appointment schedule and configure the booking-page URL. | Must | AC-5 |
| REQ-6 | The app must handle missing/invalid booking-link configuration, invalid user input, and an unavailable or non-local Ollama model/endpoint with clear recovery guidance. | Must | AC-6 |
| REQ-7 | The lab must provide a synthetic walkthrough and make the prototype's limitations clear. | Must | AC-7 |
| REQ-8 | A separate AI/model must review the implementation adversarially; critical findings must be fixed or documented as blockers before publication. | Must | AC-8 |
| REQ-9 | Reviewed changes must be committed and pushed to the GitHub remote after required delivery gates. | Must | AC-9 |

## 6. Non-functional requirements and constraints

| ID | Requirement/constraint | How to verify |
|---|---|---|
| NFR-1 | Keep the implementation small and consistent with repository conventions. | Review changed files and setup/run steps. |
| NFR-2 | Keep inference on the configured loopback Ollama endpoint, verify the model appears in `/api/tags`, disable proxies and HTTP redirects, and document `OLLAMA_NO_CLOUD=1` plus restarting Ollama; do not call an external messaging service or Google Calendar API. | Inspect network endpoint, model check, redirect behavior, and setup guide. |
| NFR-3 | Do not write candidate or vacancy inputs to files, logs, telemetry, or committed output. | Inspect implementation and run output. |
| NFR-4 | Require a control-character-free HTTPS Google Calendar booking URL in a supported host/path format, reject userinfo, and preserve it as literal text rather than asking the model to recreate it. URL format validation does not establish that the schedule is active. | Inspect configuration validation and generated draft. |
| NFR-5 | Do not claim a real booking-page or Google account flow was validated unless it was tested with a dedicated account. | Record commands, account used, and limitations. |

## 7. Acceptance criteria

### AC-1 — Collect recruiter context (REQ-1)
- **Given** the recruiter starts the app
- **When** they provide the requested vacancy details and optionally a candidate first name
- **Then** the input is collected interactively, the optional first name rejects link/control syntax, and no values are passed in command-line arguments; end-of-input exits as cancellation

### AC-2 — Generate a grounded Brazilian Portuguese draft (REQ-2)
- **Given** the recruiter has supplied the role title, description, and reason for interest
- **When** the local model generates the draft
- **Then** it is concise, professional, in Brazilian Portuguese, and contains no facts beyond the supplied context

### AC-3 — Preserve the booking link (REQ-3, NFR-4)
- **Given** a valid HTTPS Google Calendar booking-page URL is configured
- **When** the draft is displayed
- **Then** a supported, configured Google Calendar booking URL appears once in the final invitation, while model-generated links and invalid candidate-name syntax cannot create additional URLs

### AC-4 — Keep the recruiter in control (REQ-4, NFR-2, NFR-3)
- **Given** the draft has been generated
- **When** the app finishes
- **Then** it displays the text for the recruiter to copy, sends nothing, and stores no candidate or vacancy details

### AC-5 — Set up Google Calendar booking (REQ-5)
- **Given** the learner follows the guide
- **When** they prepare the booking link
- **Then** they can create a Google Calendar appointment schedule, set its availability and duration, copy its public booking-page URL, and configure that URL for the lab

### AC-6 — Recover from setup/runtime errors (REQ-6)
- **Given** configuration is missing/invalid, input is empty, or Ollama is unavailable
- **When** the app starts or generates a draft
- **Then** it reports the failing step and a concrete recovery action without exposing credentials or input values; cloud model names and models absent from the local `/api/tags` list are rejected, and redirects are not followed

### AC-7 — Run the synthetic walkthrough (REQ-7)
- **Given** the learner has installed the documented local model and configured a sample booking link
- **When** they follow the walkthrough
- **Then** it creates a sample invitation using fictional data and sends no message

### AC-8 — Complete independent adversarial review (REQ-8)
- **Given** implementation and validation evidence are ready
- **When** a separate AI/model reviews the change against this SPEC, privacy boundaries, and failure cases
- **Then** findings are recorded, critical findings are fixed or block delivery, and the outcome is included in the handoff

### AC-9 — Publish reviewed changes (REQ-9)
- **Given** required implementation and review gates are complete
- **When** the changes are ready for delivery
- **Then** a commit is pushed to the GitHub remote and its branch/commit is reported

## 8. Data, interfaces, and integrations

- **AI:** Local Ollama `/api/generate` endpoint; default URL and model follow repository conventions. The recruiter supplies any candidate profile highlights; the app does not browse or fetch candidate data.
- **Calendar:** The recruiter creates an appointment schedule in Google Calendar and copies its booking-page URL into local configuration. The sample does not access private calendar data or call the Google Calendar API.
- **Inputs:** Candidate first name and company name (both optional), role title, short role description, why the recruiter is interested, and a configured booking-page URL.
- **Output:** A channel-neutral invitation draft in Brazilian Portuguese with a call to action and the exact booking URL. The recruiter selects the channel and sends it outside the app.
- **Retention:** Inputs and generated output exist only for the current console interaction. The app does not write them to disk or log them.
- **Calendar link visibility:** Google Calendar booking pages are accessible to people with the link. The setup guide must remind the recruiter to review the page title and account/profile details before sharing.

## 9. UX and content

Keep the flow to one short terminal session: collect a few details, show the generated draft, show a copy/send reminder, exit. Provide explicit setup, loading, success, empty-input, missing-link, and Ollama-error messages. Keep candidate communication in the recruiter's control and avoid claims that this is production-ready for SAM.

## 10. Technical design

Approved solution: .NET 11 console app, interactive input, direct loopback Ollama `/api/tags` and `/api/generate` calls with redirects disabled, exact booking URL injection by application code, and console-only draft output. Configure Ollama with `OLLAMA_NO_CLOUD=1` and restart the service. The Google appointment schedule is created and configured by the recruiter in the Google Calendar UI. See [ADR-001](ADR-001.md).

## 11. Implementation plan

| Task | Requirements | Owner | Dependencies | Status |
|---|---|---|---|---|
| Confirm the compact invitation-and-booking journey; pass Gate 1 | REQ-1–REQ-9 | User / Coordinator | User decisions received | Complete |
| Record design and privacy boundaries; pass Gate 2 | REQ-1–REQ-7, NFR-1–NFR-5 | Coordinator / Architect | Gate 1 | Complete |
| Implement the app, docs, main README and site feature | REQ-1–REQ-7 | Implementation agent / Coordinator | Gate 2 | Complete |
| Build and record synthetic/live-flow limitations | All | Coordinator / QA | Implementation | Build complete; live flow pending |
| Run separate adversarial AI review and resolve findings | REQ-8 | Independent reviewer / implementation agent | Implementation and validation | Complete; findings fixed |
| Commit and push reviewed changes | REQ-9 | Coordinator | Required gates complete | Complete — pushed to `main`; GitHub Pages verified. |

## 12. Validation plan and evidence

| Scenario/REQ | Layer | Command or procedure | Result/evidence |
|---|---|---|---|
| AC-1 / REQ-1 | Manual | Run the app and verify it prompts for details interactively rather than accepting sensitive values as CLI arguments | Pending |
| AC-2 / REQ-2 | Manual/model | Use synthetic data and inspect that the draft is in Brazilian Portuguese and adds no unsupported facts | Pending |
| AC-3 / REQ-3 | Manual | Configure a synthetic HTTPS booking URL and verify the exact string appears in the draft | Pending |
| AC-4 / REQ-4 | Code/manual | Inspect network flow, output, storage, and logs; verify no sending or persistence | Pending |
| AC-5 / REQ-5 | Documentation | Follow official Google Calendar appointment schedule setup and link-sharing instructions | Pending |
| AC-6 / REQ-6 | Manual | Exercise missing/invalid URL, empty input, and stopped Ollama endpoint | Pending |
| AC-7 / REQ-7 | Manual | Follow the full synthetic walkthrough and verify no message is sent | Pending |
| AC-8 / REQ-8 | Independent review | Separate AI review and follow-up check of fixes | Pass — no blocking/high findings remained; medium/low findings were fixed and the reviewer confirmed the fixes. See [VALIDATION.md](VALIDATION.md). |
| AC-9 / REQ-9 | Delivery | Confirm commit exists on the intended remote branch and verify the published project site | Pass — commit `b2e45ab` is on `main`; the public page returns HTTP 200 with the Lab 10 feature and image. |

## 13. Risks, questions, and deferred ideas

| Item | Impact | Owner | Resolution |
|---|---|---|---|
| Production outreach channel, candidate authorization, final wording, and exception handling have not been confirmed by the recruiter. | Automated contact could violate the recruiter's process or candidate expectations. | Recruiter / Product owner | Deferred: the prototype only generates a draft; recruiter selects channel and sends manually. Revisit after R4–R6 are answered. |
| Google booking pages are public to anyone with the link and show some account/profile information. | A recruiter could share a page that reveals more profile details than intended. | Recruiter | Document a check of the booking page before sharing; use a test account for the walkthrough. |
| Real Google booking flow requires a Google account and a configured appointment schedule. | Without an account, only the synthetic draft can be validated. | User / Coordinator | Document clearly; do not claim live booking validation without a dedicated test account. |

Future ideas, not requirements for this release: channel-specific message variants; role-specific templates; a follow-up reminder draft; a checklist that confirms the booking page and role details before send; support for multiple recruiters/booking URLs; and, only after discovery and approval, a messaging connector.

## 14. Change history

| Date | Change | Reason/approver |
|---|---|---|
| 2026-10-20 | Created draft for a Google Calendar agent with event listing, availability, and confirmed event creation. | Initial user request and choice of all three capability options. |
| 2026-10-20 | Replaced direct Calendar API operations with the approved channel-neutral invitation-and-booking flow. | User approved the compact V1: recruiter sends the AI-prepared invite; candidate books directly on the recruiter's Google Calendar booking page. |
| 2026-10-20 | Tightened local-model checks, booking-link validation, and draft review guidance after adversarial review. | Coordinator approved fixes; no product behavior or delivery scope added. |
| 2026-10-20 | Clarified recruiter-provided candidate research context and added a human review stage after adversarial AI review in the reusable delivery path. | User request; app remains a Calendar booking-link prototype and does not search the web. |
