# Lab 10 — AI Recruiter Invitation + Google Calendar

![Lab 10: recruiter provides non-sensitive professional context; candidates book through Google Calendar. The app does not access email, contacts, or sensitive information.](../assets/lab-10-calendar-agent.png)

The reusable delivery path is **Tracer AI → Figma → AI Tech Lead Agent → AI Dev Agent → AI Adversarial Review Agent → Human Review → GitHub**. Lab 10 brings that path to an interview booking scenario: a local AI drafts a short invitation in Brazilian Portuguese, and Google Calendar lets an interested candidate choose an available time.

The recruiter may provide only non-sensitive professional context, such as relevant skills and experience already gathered from a candidate’s public profile. A local AI uses that context to draft a relevant invitation. The recruiter reviews every fact, chooses a communication channel, and sends the message manually. The candidate accepts by booking a time through the Google Calendar appointment-schedule link. The app does not read email, access contacts, search the web, fetch candidate profiles, or access sensitive information. Its Calendar integration is limited to the booking-page URL; it does not read private calendar events.

![Lab 10 workflow: recruiter prepares an invitation, candidate picks an available time, and Google Calendar records the booking](../assets/lab-10-calendar-workflow.png)

## What it demonstrates

- Interactive collection of role details and non-sensitive recruiter-provided professional context, such as relevant skills or experience; candidate first name and company name are optional. Do not enter sensitive information.
- Local text generation through Ollama’s `/api/generate` endpoint.
- Exact insertion of the Google Calendar booking-page URL by the app, outside the model prompt.
- Human review and manual copy/send through a channel chosen by the recruiter.
- Candidate self-booking on Google Calendar’s appointment schedule page.
- No email reading, contact access, private calendar access, or sensitive-information access; Calendar use is limited to the booking-page link.

## Prerequisites

- [.NET 11 SDK](https://dotnet.microsoft.com/)
- [Ollama](https://ollama.com/) running on this computer
- A local Ollama model, such as `llama3.2`:

```powershell
ollama pull llama3.2
```

For local-only Ollama on Windows, run `setx OLLAMA_NO_CLOUD 1`, then quit Ollama from the taskbar and launch it again so the server picks up the setting. The app checks `/api/tags` to confirm the selected model is installed locally, rejects cloud-model names, and uses a direct loopback endpoint without a proxy or HTTP redirects. Ollama documents this setting and restart requirement in its [FAQ](https://docs.ollama.com/faq#how-do-i-disable-ollamas-cloud-features).

## Set up the Google Calendar booking page

1. In Google Calendar, create an **Appointment schedule** and set the interview duration, availability, booking window, and other options that fit your own process.
2. Review the booking page’s preview and the account/profile information it displays before sharing it. Anyone with the booking-page link can access the page.
3. Save the schedule and copy its booking-page link. Google Calendar manages available slots and adds a candidate’s booking to the recruiter’s calendar.
4. Set the copied HTTPS URL in the environment for the current PowerShell session:

```powershell
$env:GOOGLE_CALENDAR_BOOKING_URL = "https://calendar.google.com/calendar/appointments/schedules/YOUR_SCHEDULE_ID"
```

Use your actual link; the example URL above is not a working booking page. To stop sharing, change or remove the schedule or its booking-page link in Google Calendar.

The app accepts the standard `calendar.google.com/calendar/appointments/schedules/...` link and the short `calendar.app.google/...` booking link. It checks the URL’s HTTPS scheme, domain, and path format; it cannot confirm that a schedule is active, so open the booking page yourself before sharing.

Google setup guides: [create an appointment schedule](https://support.google.com/calendar/answer/10729749) · [share an appointment schedule](https://support.google.com/calendar/answer/10733297) · [learn about appointment schedules](https://support.google.com/calendar/answer/11608416).

## Run the lab

From the repository root:

```powershell
dotnet run --project 10-ai-recruiter-calendar
```

The app prompts for the candidate’s optional first name, optional company name, and role details. Input is entered interactively, not as command-line arguments. Review the generated text for factual accuracy, then copy and send it through your chosen channel. Nothing is sent automatically.

Optional local settings:

```powershell
$env:OLLAMA_MODEL = "llama3.2"
$env:OLLAMA_BASE_URL = "http://localhost:11434"
```

`OLLAMA_BASE_URL` must point to an HTTP endpoint on the same computer (localhost or a loopback IP). The default model is `llama3.2` and the default endpoint is `http://localhost:11434`.

### Synthetic demo

To use fictional sample details without typing them, run:

```powershell
dotnet run --project 10-ai-recruiter-calendar -- --demo
```

Demo mode still calls the configured local Ollama model. It requires the booking URL and does not send a message.

## Implementation notes

- Enter only information you are allowed to process. The app sends the recruiting details to the local Ollama endpoint and does not write them to files or logs. The model receives no booking URL; generated links are removed before the configured link is appended once.
- The app does not connect to a messaging provider or call the Google Calendar API. The recruiter sends the draft; Google Calendar handles the candidate’s booking.
- This lab demonstrates an educational draft-and-booking-link workflow. It is not a production deployment of the SAM AI Agent Recruiter.
