# mptr-jira

For Microsoft Powertoys Run, Open Jira tickets based on ticket number.

## Usage

### Opening a ticket with the default ticket prefix

`jira 123`

### Opening a specific ticket

`jira TICKET-123`

### Searching

`jira my search query`

## Installation

1. Close PowerToys
2. Build
3. Rename the output directory in `bin/Debug` to mptr-jira
4. Copy the output directory to `C:\Program Files\PowerToys\modules\launcher\Plugins`
5. Edit the settings file `C:\Program Files\PowerToys\modules\launcher\Plugins\mptr-jira\settings.json` to your liking
6. Start PowerToys

## Settings File

`UrlPrefix` is the URL to your JIRA server. It must end with a slash character (`/`).

`DefaultTicketPrefix`

```
{
  "UrlPrefix": "https://my.internal.server/",
  "DefaultTicketPrefix": "TICKET"
}
```
