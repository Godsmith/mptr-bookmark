# mptr-bookmark

Plugin for Microsoft Powertoys Run that opens Chrome bookmarks containing the provided text.

Based on https://github.com/novafaen/mptr-jira.

## Usage

### Show all bookmarks containing both "foo" and "bar"

`bookmark foo bar`

Or, if the "include in global results" setting in the PowerToys settings is enabled:

`foo bar`

At least one search term has to be three characters or more.

## Installation

1. Build the solution at least once, either from inside Visual Studio or with `dotnet build`.
2. Run cmd as administrator and navigate to this folder.
3. Run `install.bat`. This will create a symlink from the PowerToys plugin folder to 
   the output directory of this solution. PowerToys will be restarted.
