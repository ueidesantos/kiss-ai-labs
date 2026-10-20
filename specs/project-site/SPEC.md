# SPEC — KISS AI Labs public site

Role: coordinator, UX, and frontend implementation. Authorized request: introduce the project first and place its history below, on GitHub Pages.

- REQ-1: A static Portuguese-language page with a Lab 10 cover/highlight for the .NET Google Calendar booking app, plus links to documentation and the project workflow.
- REQ-2: Catalog all eleven labs, 00–10, with accurate descriptions and limitations.
- REQ-3: Place project history after the catalog, with a link to the full commit history.
- REQ-4: Responsive layout, keyboard navigation, adequate contrast, and no JavaScript dependency for reading the content.
- REQ-5: Publish from `main` and enable Pages at `/docs`; verify the public URL before declaring publication complete.

Design: static HTML/CSS, dark background, and amber accents inspired by the approved cover. Copy the original image unchanged to `docs/assets`. No framework, dependencies, or data collection. GitHub's Code tab keeps its own structure.

Acceptance: internal links and assets resolve; nine cards; history follows the catalog; no horizontal scrolling at desktop/mobile viewports; the public deployment serves the page. Real Figma integration remains unvalidated.

## Verification

Independent QA review: the initial date was corrected to 09/10 based on the commits. Edge headless at 1440 and 390 px: nine cards, image loaded, anchors valid, history below the catalog, and no horizontal overflow. The first keyboard focus is the skip-to-content link. Mobile screenshot inspected. Publication will be confirmed by the public HTTP response.

Publication confirmed: GitHub Pages on `main` at `/docs`; build 8239f6e completed successfully. The public URL returned HTTP 200 with the expected content and order. Link added to the repository's About field.

## About me

REQ-6: Add an About Me section as the last section after the history and the last menu item, after GitHub. Include the name Ueide Santos, a button linking to https://ueidesantos.github.io/ueidesantos-cv-web/ and a navigation anchor. Do not assume or invent a biography.

## Adversarial Marketing and Sales review — 2026-10-11

Roles: UX and frontend implementation. Authorized scope: improve clarity, navigation, and conversion to lab and resume content while preserving the identity and actual evidence.

- REQ-7: Speak to .NET/Python developers, describe practical application problems and delivered content, and avoid invented metrics or social proof.
- REQ-8: Offer four entry points: local model (00), document/code search (03–05), specification/delegation (07–09), and recruiter booking with Google Calendar (10).
- REQ-9: Explain acronyms and dependencies; distinguish experiments from production solutions; show limitations for 06, 07, and 08 alongside the cards.
- REQ-10: Keep eleven cards and feature Lab 10 with its updated cover; use a primary CTA to choose a learning path, a secondary CTA for Lab 00, evidence links, and consistent metadata.

Acceptance: About Me is the last menu link and last section in `main`; three entry paths; eleven cards; anchors and assets valid; no overflow at 320/390/768/1440 px; preserve keyboard support and reduced-motion behavior. Evidence is in MARKETING-REVIEW.md. The coordinator verifies deployment.


## Lab 10 site update — 2026-10-20

At the user's request, feature Lab 10 and add it to the catalog. The cover depicts the recruiter reviewing candidate profile, skills, and experience; a tailored invitation; candidate booking through Google Calendar; and the reusable Tracer AI → Figma → AI Tech Lead Agent → AI Dev Agent → AI Adversarial Review Agent → Human Review → GitHub trail. The .NET app does not browse candidate profiles or integrate with the other workflow components; it uses recruiter-provided context and focuses on Google Calendar booking.
