using ManagedCommon;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Wox.Plugin;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace mptr.jira
{
    public class Main : IPlugin
    {
        private string IconPath { get; set; }

        private PluginInitContext Context { get; set; }
        public string Name => "jira";

        public string Description => "Open Jira tickets based on ticket number.";

        // url to jira (should end with "browse/").
        private string UrlPrefix => "https://my.internal.server/browse/";
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
            
            if (!Regex.IsMatch(value, @"\b\d{3}\d*\b"))
            {
                return new List<Result>
                {
                    new Result
                    {
                        Title = "Query must contain at least 3 numers",
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
            string FullTicketNumber;
            if (UserTicket.Contains(DefaultTicketPrefix + "-"))
            {
                FullTicketNumber = UserTicket; // query is "TICKET-XXXX".
            }
            else if (IsQueryNumbersOnly)
            {
                FullTicketNumber = DefaultTicketPrefix + "-" + UserTicket; // query is "XXXX".
            } else
            {
                FullTicketNumber = value.ToUpper();
            }

            return new List<Result>
            {
                new Result
                {
                    Title = FullTicketNumber,
                    SubTitle = UrlPrefix + FullTicketNumber,
                    IcoPath = IconPath,
                    Action = e =>
                    {
                        OpenUrl(UrlPrefix + FullTicketNumber);
                        return true;
                    },
                }
            };
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
