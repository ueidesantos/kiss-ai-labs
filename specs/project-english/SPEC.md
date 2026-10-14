# SPEC — English-language Markdown content

- **ID:** PROJECT-ENGLISH
- **Status:** Approved for implementation
- **Product owner:** User
- **Coordinator:** Codex
- **Last updated:** 2026-10-14

## 1. Problem and context

The repository's maintained Markdown content is primarily written in Portuguese, which makes the learning materials inconsistent for English-speaking readers. The user clarified that this localization must affect Markdown files only.

## 2. Objective and expected outcome

Translate the human-readable prose in the repository's Markdown files into clear American English while preserving technical meaning and keeping examples aligned with the existing implementation. Use fluent U.S. technical language and natural idioms where they fit; keep slang light and avoid forcing it into specifications, policies, or validation records.

## 3. Users and journeys

- **Users:** Readers and contributors using the KISS AI Labs repository.
- **Primary journey:** Read the project overview, lab guides, specifications, operating notes, and templates in English.
- **Relevant states:** Documentation is readable, links remain intact, and technical examples remain usable.

## 4. Scope

### Included

- Translate prose, headings, tables, comments, and explanatory text in repository `*.md` files, including Markdown files under `specs/`, `templates/`, `assets/`, and lab directories.
- Translate the root `AGENTS.md` while preserving its workflow and governance requirements.
- Preserve technical identifiers, filenames, paths, links, URLs, commands, code, machine-readable examples, quoted source material, and intentional proper nouns.
- Preserve existing local edits while translating their current content.

### Excluded

- Any non-Markdown file, including source code, HTML, JSON, YAML, CSS, images, and generated binaries.
- Changes to product behavior, APIs, business rules, dependencies, or technical examples.

## 5. Functional requirements

| ID | Requirement | Priority | Acceptance criteria |
|---|---|---|---|
| REQ-1 | Markdown prose is written in English. | Must | AC-1, AC-2 |
| REQ-2 | The translation preserves meaning, technical terminology, links, and traceability. | Must | AC-3 |
| REQ-3 | Only Markdown files are changed. | Must | AC-4 |
| REQ-4 | Existing local edits are retained in translated files. | Must | AC-5 |

## 6. Non-functional requirements and constraints

| ID | Requirement/constraint | Verification |
|---|---|---|
| NFR-1 | Markdown structure, code fences, commands, paths, URLs, and identifiers remain valid and unchanged unless prose around them is translated. | Review diffs and Markdown file set |
| NFR-2 | No validation is claimed unless it is run. | Report actual checks and limitations |

## 7. Acceptance criteria

### AC-1 — Project documentation (REQ-1)
- **Given** a maintained Markdown document with Portuguese prose
- **When** this work is complete
- **Then** its human-readable prose is in English.

### AC-2 — Markdown coverage (REQ-1)
- **Given** all repository Markdown files
- **When** the translation is reviewed
- **Then** prose in guides, specifications, templates, examples, and operational records is in English, except content explicitly preserved by scope.

### AC-3 — Fidelity (REQ-2)
- **Given** technical examples and requirements
- **When** their prose is translated
- **Then** their original meaning, numbers, business rules, and references are preserved.

### AC-4 — File boundary (REQ-3)
- **Given** the final working tree
- **When** changed paths are reviewed
- **Then** every changed file has a `.md` extension.

### AC-5 — Existing work (REQ-4)
- **Given** Markdown files already modified before this task
- **When** they are translated
- **Then** their existing edits remain represented in the final content.

## 8. Data, interfaces, and integrations

No runtime interfaces or integrations change. Markdown links, commands, identifiers, and machine-readable examples retain their current contracts.

## 9. UX and content

Use clear, idiomatic American English and retain the repository's concise educational tone. Use technically fluent U.S. terminology and natural idioms when appropriate, without overdoing slang. Keep established technical terms such as SDD, RAG, MCP, Ollama, and Top-K.

## 10. Technical design

Edit Markdown files in place. Do not rename files or update non-Markdown assets. Review diffs for structure and scope.

## 11. Implementation plan

| Task | Requirements | Owner | Dependencies | Status |
|---|---|---|---|---|
| Translate Markdown content by repository area | REQ-1, REQ-2, REQ-4 | Coordinator and assigned reviewers | Approved scope | In progress |
| Review changed paths and remaining Portuguese prose | REQ-1 through REQ-4 | Coordinator | Translation complete | Pending |

## 12. Validation plan and evidence

| Scenario/REQ | Layer | Procedure | Result/evidence |
|---|---|---|---|
| AC-1, AC-2 | Documentation | Review Markdown content for untranslated prose | Pending |
| AC-3 | Documentation | Review technical fidelity and references | Pending |
| AC-4, AC-5 | Repository | Inspect changed paths and pre-task diffs | Pending |

## 13. Risks, questions, and pending decisions

No open scope questions remain after the user's clarification. Historical validations and quoted material must retain their factual meaning while being translated.

## 14. Change history

| Date | Change | Reason/approver |
|---|---|---|
| 2026-10-14 | Initial approved scope: translate Markdown files only | User clarified the file boundary |
| 2026-10-14 | Set the translation style to American English with light, natural U.S. idioms | User clarified the language preference |
