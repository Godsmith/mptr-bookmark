using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace mptr.bookmark
{
    public static class ChromeBookmarks
    {
        public static List<(string, string)> Get()
        {
            string path = $@"C:\Users\{Environment.UserName}\AppData\Local\Google\Chrome\User Data\Default\Bookmarks";
            string json = File.ReadAllText(path);
            Regex rx = new Regex("\"name\": \"(.*?)\".*?\n.*?\n.*?\"url\": \"(.*?)\"");
            MatchCollection matches = rx.Matches(json);

            var bookmarks = new List<(string, string)>();
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                Console.WriteLine(groups[1].Value + " " + groups[2].Value);
                bookmarks.Add((groups[1].Value, groups[2].Value));
            }
            return bookmarks;
        }
    }
}
