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

1. Run Visual Studio code as administrator (needed for the build process to copy the plugin to the Program Files directory)
2. Edit the settings.json file to your liking
3. Build the project

The plugin will be automatically copied to the PowerToys Run plugins directory and PowerToys will be automatically restarted.

## Settings File

`UrlPrefix` is the URL to your JIRA server. It must end with a slash character (`/`).

`DefaultTicketPrefix`

```
{
  "UrlPrefix": "https://my.internal.server/",
  "DefaultTicketPrefix": "TICKET"
}
```
