using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Linq;

namespace mptr.bookmark
{
    public static class JsonBookmarks
    {
        public static IEnumerable<Bookmark> Get(string path, string iconPath)
        {
            string json = File.ReadAllText(path);
            JsonDocument document = JsonDocument.Parse(json);
            return GetBookmarks(document.RootElement, iconPath);
        }

        private static IEnumerable<Bookmark> GetBookmarks(JsonElement element, string iconPath)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                JsonElement url;
                bool hasUrl = element.TryGetProperty("url", out url);
                JsonElement name;
                bool hasName = element.TryGetProperty("name", out name);
                if (hasUrl && hasName)
                {
                    return new List<Bookmark>() { new Bookmark { Name = name.GetString().ToLower(), Url = url.GetString().ToLower(), IconPath = iconPath } };
                }
                else { return element.EnumerateObject().SelectMany(property => GetBookmarks(property.Value, iconPath)); }
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                return element.EnumerateArray().SelectMany(element => GetBookmarks(element, iconPath));
            }
            else
            {
                return new List<Bookmark>();
            }
        }
    }
}
