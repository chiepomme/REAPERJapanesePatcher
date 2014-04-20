using System.Diagnostics;
using System.IO;

namespace REAPERJapanesePatcher
{
    public class LanguagePackInstaller
    {
        public void Install(string languagePath, string reaperPath)
        {
            var startInfo = new ProcessStartInfo()
            {
                FileName = reaperPath,
                Arguments = Path.GetFullPath(languagePath).Quote(),
                UseShellExecute = false,
            };

            Process.Start(startInfo);
        }
    }
}
