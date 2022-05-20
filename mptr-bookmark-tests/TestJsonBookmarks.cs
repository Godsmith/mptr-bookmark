using mptr.bookmark;
using System.Linq;

namespace mptr.bookmark.tests
{
    [TestClass]
    public class TestJsonBookmarks
    {
        [TestMethod]
        public void TestGetChromeBookmarks()
        {
            var names_and_urls = JsonBookmarks.Get($@"C:\Users\{Environment.UserName}\AppData\Local\Google\Chrome\User Data\Default\Bookmarks", "images/chrome.light.png");
            Assert.IsTrue(names_and_urls.First().Url.Contains("http"));
        }

        [TestMethod]
        public void TestGetEdgeBookmarks()
        {
            var names_and_urls = JsonBookmarks.Get($@"C:\Users\{Environment.UserName}\AppData\Local\Microsoft\Edge\User Data\Default\Bookmarks", "images/chrome.light.png");
            Assert.IsTrue(names_and_urls.First().Url.Contains("http"));
        }
    }
}