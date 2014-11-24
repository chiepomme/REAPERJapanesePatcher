using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace REAPERJapanesePatcher.Test
{
    [TestClass]
    public class SubsequenceTest
    {
        [TestMethod]
        public void サブシーケンスを検索することができる()
        {
            var source = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var finder = new SubsequenceFinder(new byte[] { 1 });
            Assert.AreEqual(0, finder.FindIn(source, 0));
            Assert.IsNull(finder.FindIn(source, 1));

            finder = new SubsequenceFinder(new byte[] { 3, 4, 5 });
            Assert.AreEqual(2, finder.FindIn(source, 0));
            Assert.IsNull(finder.FindIn(source, 3));

            finder = new SubsequenceFinder(new byte[] { 8, 9, 10 });
            Assert.AreEqual(7, finder.FindIn(source, 0));
        }
    }
}
