using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace mptr.bookmark
{
    public static class JsonBookmarks
    {
        public static List<Bookmark> Get(string path, string iconPath)
        {
            string json = File.ReadAllText(path);
            Regex rx = new Regex("\"name\": \"(.*?)\".*?\n.*?\n.*?\"url\": \"(.*?)\"");
            MatchCollection matches = rx.Matches(json);

            var bookmarks = new List<Bookmark>();
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                Console.WriteLine(groups[1].Value + " " + groups[2].Value);
                bookmarks.Add(new Bookmark { Name = groups[1].Value, Url = groups[2].Value, IconPath = iconPath });
            }
            return bookmarks;
        }
    }
}
