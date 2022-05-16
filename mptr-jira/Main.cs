using ManagedCommon;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Wox.Plugin;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Web;

namespace mptr.jira
{
    public class Main : IPlugin
    {
        private string IconPath { get; set; }

        private PluginInitContext Context { get; set; }
        public string Name => "jira";

        public string Description => "Microsoft Powertoys Run plugin for Atlassian Jira.";

        // url to jira (should end with a slash, i.e. "/").
        private string UrlPrefix => "https://my.internal.server/";
        // default ticket type, if query is only numbers (should be without dash, i.e. "-").
        private string DefaultTicketPrefix => "TICKET";

        public List<Result> Query(Query query)
        {
            if (query?.Search is null)
            {
                return new List<Result>(0);  // no query.
            }

            var value = query.Search.Trim();

            if (string.IsNullOrEmpty(value))
            {
                return new List<Result>(0);  // empty query.
            }
            
            if (value.Length < 3)
            {
                return new List<Result>
                {
                    new Result
                    {
                        Title = "Search query must contain at least 3 characters",
                        SubTitle = "Please add more to your query.",
                        IcoPath = IconPath,
                        Action = e =>
                        {
                            OpenUrl("https://www.youtube.com/watch?v=oHg5SJYRHA0");
                            return true;
                        },
                    }
                };
            }

            bool IsQueryNumbersOnly = Regex.IsMatch(value, @"^\d{3}\d*$");
            string UserTicket = value.ToString().Trim();
            string FullTicketNumber = null;
            string JiraURL = null;

            if (UserTicket.Contains(DefaultTicketPrefix + "-"))
            {
                FullTicketNumber = UserTicket;
                JiraURL = UrlPrefix + "browse/" + FullTicketNumber; // query is "TICKET-XXXX".
            }
            else if (IsQueryNumbersOnly)
            {
                FullTicketNumber = DefaultTicketPrefix + "-" + UserTicket;
                JiraURL = UrlPrefix + "browse/" + FullTicketNumber; // query is "XXXX".
            }

            List<Result> ToReturn = new List<Result>();

            if (FullTicketNumber != null && JiraURL != null)
            {
                ToReturn.Add(
                    new Result
                    {
                        Title = FullTicketNumber,
                        SubTitle = JiraURL,
                        IcoPath = IconPath,
                        Action = e =>
                        {
                            OpenUrl(JiraURL);
                            return true;
                        },
                    }
                );
            }

            string SearchQuery = value;
            string JiraSearchURL = UrlPrefix + "secure/QuickSearch.jspa?searchString=" + HttpUtility.UrlEncode(value);

            ToReturn.Add(new Result
                {
                    Title = "Search for: " + SearchQuery,
                    SubTitle = JiraSearchURL,
                    IcoPath = "images/jira.search.png",
                    Action = e =>
                    {
                        OpenUrl(JiraSearchURL);
                        return true;
                    },
                }
            );

            return ToReturn;
        }

        public void Init(PluginInitContext context)
        {
            Context = context;
            Context.API.ThemeChanged += OnThemeChanged;
            UpdateIconPath(Context.API.GetCurrentTheme());
        }

        private void UpdateIconPath(Theme theme)
        {
            if (theme == Theme.Light || theme == Theme.HighContrastWhite)
            {
                IconPath = "images/jira.light.png";
            }
            else
            {
                IconPath = "images/jira.dark.png";
            }
        }

        private void OnThemeChanged(Theme currentTheme, Theme newTheme)
        {
            UpdateIconPath(newTheme);
        }

        // copied from https://brockallen.com/2016/09/24/process-start-for-urls-on-net-core/
        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
