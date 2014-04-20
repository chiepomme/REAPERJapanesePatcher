using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace REAPERJapanesePatcher.Test
{
    [TestClass]
    public class LanguagePackInstallerTest
    {
        [TestMethod]
        public void 言語パックをインストール出来る()
        {
            var installer = new LanguagePackInstaller();
            // テストの際に毎回インストールされてしまうのでコメントアウトしている
            //installer.Install(@"..\..\..\..\Japanese.ReaperLangPack", @"C:\Program Files\REAPER (x64)\reaper.exe");
        }
    }
}
