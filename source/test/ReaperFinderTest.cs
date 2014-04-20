using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace REAPERJapanesePatcher.Test
{
    [TestClass]
    public class ReaperFinderTest
    {
        readonly string ReaperPath = "reaper.exe";

        [TestMethod]
        public void Reaperを既定のパスから探すことができる()
        {
            if (!File.Exists(ReaperPath))
            {
                using (File.Create(ReaperPath)) { }
            }

            var finder = new ReaperFinder();
            var pathFound = finder.FindPath();
            Assert.AreEqual("reaper.exe", Path.GetFileName(pathFound).ToLower());
        }
    }
}
