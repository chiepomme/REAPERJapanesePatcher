using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace REAPERJapanesePatcher.Test
{
    [TestClass]
    public class PathStringExtensionTest
    {
        [TestMethod]
        public void パスの文字列をダブルクオーテーションで囲める()
        {
            Assert.AreEqual(@"""miko""", "miko".Quote());
            Assert.AreEqual(@"""mi ko""", "mi ko".Quote());
        }

        [TestMethod]
        public void パスの文字列を二重には囲まない()
        {
            Assert.AreEqual(@"""miko""", @"""miko""".Quote());
            Assert.AreEqual(@"""mi ko""", @"""mi ko""".Quote());
        }
    }
}
