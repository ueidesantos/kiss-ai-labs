# Validation — Lab 10: AI Recruiter Outreach + Google Calendar

Link each run to a requirement in [SPEC.md](SPEC.md). Do not mark a scenario as passed until it is actually run.

| ID | Requirement/scenario | Type | Command or procedure | Result | Evidence/notes |
|---|---|---|---|---|---|
| AC-1 / REQ-1 | Enter the role details and optional candidate/company names in the interactive console | Manual | Pending | Pending | EOF cancellation and first-name URL/control rejection, including bare domains and www links, are implemented but not runtime-tested. |
| AC-2 / REQ-2 | Generate a concise, professional Brazilian Portuguese draft grounded in supplied sample data | Manual/model | Pending | Pending | |
| AC-3 / REQ-3 | Verify exact configured booking URL appears in the draft | Manual | Pending | Pending | |
| AC-4 / REQ-4 | Confirm the app sends no message and persists/logs no input or output | Code/manual review | Pending | Pending | |
| AC-5 / REQ-5 | Follow Google Calendar schedule setup/link instructions | Documentation | Pending | Pending | Live booking requires a dedicated account. |
| AC-6 / REQ-6 | Exercise invalid/missing booking URL, invalid name, EOF, unavailable Ollama, redirect, and missing/cloud model cases | Manual | Pending | Pending | Error/recovery paths are implemented but not runtime-tested. |
| AC-7 / REQ-7 | Complete the synthetic walkthrough | Manual | Pending | Pending | |
| AC-8 / REQ-8 | Separate AI adversarial review; resolve or document all findings | Review | Independent static review, followed by a check of both corrections | Pass | No blocking/high findings remained. The reviewer confirmed the link filter preserves ASP.NET, Node.js, and Vue.js and candidate names reject bare/www domains. Earlier medium/low findings were fixed. No runtime tests were performed. |
| AC-9 / REQ-9 | Push reviewed changes and verify GitHub Pages publication | Delivery | `git ls-remote origin refs/heads/main`; request the public page with a cache-busting query | Pass | Commit `b2e45ab` is on `main`; `https://ueidesantos.github.io/kiss-ai-labs/?v=b2e45ab` returned HTTP 200 and contained the Lab 10 feature and image reference. |
| NFR-6 | Confirm the Lab 10 descriptions set privacy boundaries | Documentation/code review | Inspect root/site/lab copy and integrations for email, contacts, sensitive data, and private Calendar event access | Pass | Descriptions state no email or contact access, no sensitive information access, and Calendar use limited to the booking-page URL; app contains no email/contact/Calendar API integration. |
| NFR-2 | Confirm local-only configuration and behavior | Documentation/code review | `OLLAMA_NO_CLOUD=1` setup and restart instructions; loopback endpoint; `/api/tags` model match; redirect-disabled/no-proxy handler; cloud model rejection | Code inspection complete | Ollama official FAQ documents the environment variable and restart requirement: https://docs.ollama.com/faq#how-do-i-disable-ollamas-cloud-features. Runtime behavior was not exercised. |
| NFR-4 | Validate supported Google booking-link format and output link handling | Code review | Inspect scheme, host/path, userinfo/control checks; model URL filtering; validated candidate name; single literal URL insertion | Code inspection complete | Active schedule status cannot be validated by URL syntax and must be checked by the recruiter. No runtime URL cases were exercised. |
| Build | Compile the .NET console app | Build | `dotnet build 10-ai-recruiter-calendar/RecruiterCalendar.csproj` | Pass | .NET SDK 11.0.100-rc.1.26425.128; build succeeded with 0 warnings and 0 errors. SDK emitted informational preview notice NETSDK1057. |

## Defects and exceptions

| ID | Requirement | Description | Severity | Status/acceptance |
|---|---|---|---|---|

## Summary

- Passed criteria: Build; independent adversarial review and correction confirmation; publication to `main` and public-site verification
- Reviewed without runtime validation: NFR-2 and NFR-4 code/documentation
- Pending criteria: Manual AI walkthrough and live Google Calendar booking
- Known limitation: No dedicated Google Calendar booking account has been provided; do not claim a live booking flow was validated unless one is used.
- No automated or runtime tests were run. The .NET app was compiled with `dotnet build`; Ollama was unavailable locally and no Google test calendar was provided.
