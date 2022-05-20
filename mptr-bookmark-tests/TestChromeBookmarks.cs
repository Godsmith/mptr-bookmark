using mptr.bookmark;
using System.Linq;

namespace mptr.bookmark.tests
{
    [TestClass]
    public class TestChromeBookmarks
    {
        [TestMethod]
        public void TestGetReturnsBookmarks()
        {
            var names_and_urls = ChromeBookmarks.Get();
            Assert.IsTrue(names_and_urls.First().Item2.Contains("http"));
        }
    }
}