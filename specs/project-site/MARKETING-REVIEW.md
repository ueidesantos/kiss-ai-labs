# Adversarial review — Marketing and Sales

Date: 2026-10-11 (America/Sao_Paulo). Roles: UX, communications, and frontend implementation. Scope: KISS AI Labs public site, without changing the labs or their evidence. Conversion goal: help visitors choose and open a relevant example, then learn about the author's background and contact information. No consulting offers, clients, testimonials, or business results were inferred.

## Findings and fixes

| Priority | Adversarial finding | Remediation | Requirement |
| --- | --- | --- | --- |
| High | About Me interrupted exploration of the labs and was not in the requested position. | Make it the last menu item after GitHub and the last section in `main`, after the history. | REQ-6 |
| High | The introduction listed technologies without explaining who they were for or what problems they solved. | Identify .NET/Python developers and give concrete examples: review a diff, find rules, delegate a request. | REQ-7 |
| High | Nine equivalent choices made it hard to get started; there was no path for visitors already familiar with AI. | Provide three problem-based entry points linked to 00, 03, and 07; make the next steps to 05/08 clear in the learning path. | REQ-8 |
| High | The feature language could imply that Figma integration had been validated. Card 07 omitted the limitation on real execution. | Present it as a proposal, make evidence accessible, and show Figma/Ollama/SQLite gaps beside the cards. | REQ-9 |
| Medium | Acronyms and prerequisites made it harder for visitors to choose. | Add an expandable overview of RAG, MCP, SDD, Spec Kit, TL, SDK/Python/Ollama, and offline demos. | REQ-9 |
| Medium | Generic CTAs did not explain what would open after a click. | Use “Choose my learning path,” “View code and spec,” and “View the Figma design”; note that catalog links open GitHub and the README. | REQ-10 |
| Medium | The transition from technical content to the author's background lacked context. | Connect the projects to professional experience in About Me; add resume and contact CTAs. | REQ-6, REQ-7 |
| Medium | Social sharing metadata did not control the title, summary, and cover. | Add canonical and Open Graph metadata, image/alt text, and a social card without unverified claims. | REQ-10 |
| Medium | The menu needed to handle intermediate widths after changing its order and text. | Allow the menu to wrap, make the learning-path grid responsive, and adjust flow on narrow screens. | REQ-4 |

All findings above were addressed within the page scope. Conversion gains were neither measured nor promised. The cover, colors, nine labs, and original links were preserved.

## Validation performed

- Playwright with Edge headless, local file, viewports 320, 390, 768, and 1440 px: no horizontal overflow; nine cards; three entry points; About Me is the last link and last section; all anchors resolve; image loaded.
- First keyboard focus: Skip to content. Pressing Enter on the summary expands the requirements. Reduced motion: `scroll-behavior: auto`. No page errors.
- Full screenshots at 390 and 1440 px inspected: hierarchy and spacing preserved; paths stack on small screens.
- Checked all GitHub links to files/directories against the local checkout: no missing destinations.
- HTTP check with Node and system certificates: 18 URLs returned 200, including the site, resume, code, and documentation. Figma Community returned 403 to the automated request; the user-provided and approved link was retained. This neither validates nor invalidates human access to the design, and is not evidence of real MCP integration.
- The first HTTP check using Playwright's request encountered a certificate-chain error; it was repeated with `node --use-system-ca`, without disabling TLS validation.
- Deployment of this revision is the coordinator's responsibility; this local evidence does not claim that a new version was published.
