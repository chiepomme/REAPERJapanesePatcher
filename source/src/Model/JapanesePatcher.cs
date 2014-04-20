using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace REAPERJapanesePatcher
{
    public class JapanesePatcher
    {
        public enum LangPackType
        {
            Master,
            Translation,
        }

        public static readonly Dictionary<LangPackType, Uri> LangPackUriMap = new Dictionary<LangPackType, Uri>(){
            {LangPackType.Master, new Uri("https://raw.githubusercontent.com/chiepomme/REAPERJapanesePatcher/master/Japanese.ReaperLangPack")},
            {LangPackType.Translation, new Uri("https://raw.githubusercontent.com/chiepomme/REAPERJapanesePatcher/translation/Japanese.ReaperLangPack")},
        };

        public readonly string LangPackDestination =
            Path.Combine(Path.GetTempPath(), Path.GetFileName(LangPackUriMap[LangPackType.Master].LocalPath));

        readonly ReaperFinder ReaperFinder = new ReaperFinder();
        readonly LanguagePackInstaller LangPackInstaller = new LanguagePackInstaller();
        readonly FontFixer FontFixer = new FontFixer();
        readonly Downloader Downloader = new Downloader();

        public string ReaperPath { get; set; }
        public bool IsFontFixNeeded { get; set; }
        public bool IncludesAllFiles { get; set; }
        public LangPackType SelectedLangPack { get; set; }

        public JapanesePatcher()
        {
            ReaperPath = ReaperFinder.FindPath();

            IsFontFixNeeded = true;
            IncludesAllFiles = false;
            SelectedLangPack = LangPackType.Master;
        }

        public async Task FixFonts(IProgress<FontFixProgress> progress = null)
        {
            if (!IsFontFixNeeded) return;

            var targetFiles = new List<string>() { ReaperPath };

            var pluginFolder = Path.Combine(Path.GetDirectoryName(ReaperPath), @"Plugins");
            if (IncludesAllFiles && Directory.Exists(pluginFolder))
            {
                targetFiles.AddRange(await FindOtherFiles(pluginFolder));
            }

            var numOfFiles = targetFiles.Count;

            for (var i = 0; i < numOfFiles; i++)
            {
                if (progress != null) progress.Report(new FontFixProgress(targetFiles.Count, i, targetFiles[i]));
                await FontFixer.FixFontSizeTo(targetFiles[i], 9);
            }
        }

        async Task<IEnumerable<string>> FindOtherFiles(string root)
        {
            return await Task<IEnumerable<string>>.Run(() =>
                {
                    return Directory.GetFiles(root, "*.exe", SearchOption.AllDirectories)
                                    .Concat(Directory.GetFiles(root, "*.dll", SearchOption.AllDirectories));
                }
            );
        }

        public async Task DownloadLangPack(IProgress<DownloadProgress> progress = null)
        {
            await Downloader.Download(LangPackUriMap[SelectedLangPack], LangPackDestination, progress);
        }

        public void InstallLangPack()
        {
            LangPackInstaller.Install(LangPackDestination, ReaperPath);
        }
    }

    public class FontFixProgress
    {
        public int NumOfFiles;
        public int CurrentIndex;
        public string CurrentFile;

        public FontFixProgress(int numOfFiles, int currentIndex, string currentFile)
        {
            NumOfFiles = numOfFiles;
            CurrentIndex = currentIndex;
            CurrentFile = currentFile;
        }
    }
}
