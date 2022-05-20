using ManagedCommon;
using System.Collections.Generic;
using Wox.Plugin;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;

namespace mptr.bookmark
{
    public class Main : IPlugin
    {
        private string IconPath { get; set; }

        public string Name => "Bookmark";

        private List<(string, string)> bookmarks;

        private PluginInitContext Context { get; set; }

        public string Description => "Microsoft Powertoys Run plugin for searching among Chrome bookmarks.";

        public List<Result> Query(Query query)
        {
            if (query?.Search is null)
            {
                return new List<Result>(0);
            }

            var searchTerms= query.Search.Trim().Split(" ");

            if (searchTerms.Length == 0)
            {
                return new List<Result>(0);
            }

            if (searchTerms.All(searchTerm => searchTerm.Length < 3))
            {
                return new List<Result>(0);
            }

            List<Result> ToReturn = new List<Result>();
            foreach ((string bookmarkName, string url) in bookmarks)
            {
                if (searchTerms.All(searchTerm => url.Contains(searchTerm)))
                {
                    ToReturn.Add(
                        new Result
                        {
                            Title = bookmarkName,
                            SubTitle = url,
                            IcoPath = IconPath,
                            Action = e =>
                            {
                                OpenUrl(url);
                                return true;
                            },
                        }
                    );
                }
            }

            return ToReturn;
        }

        public void Init(PluginInitContext context)
        {
            Context = context;
            Context.API.ThemeChanged += OnThemeChanged;
            UpdateIconPath(Context.API.GetCurrentTheme());

            bookmarks = ChromeBookmarks.Get();
        }

        private void UpdateIconPath(Theme theme)
        {
            if (theme == Theme.Light || theme == Theme.HighContrastWhite)
            {
                IconPath = "images/chrome.light.png";
            }
            else
            {
                IconPath = "images/chrome.dark.png";
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
