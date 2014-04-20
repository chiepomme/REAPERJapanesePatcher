using System;
using System.Collections.Generic;
using System.IO;

namespace REAPERJapanesePatcher
{
    public class ReaperFinder
    {
        readonly string ProgramFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        readonly string ProgramFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

        readonly List<string> DefaultReaperPaths = new List<string>();

        public ReaperFinder()
        {
            DefaultReaperPaths.AddRange(new[]{
                Path.Combine(ProgramFiles, @"REAPER (x64)\reaper.exe"),
                Path.Combine(ProgramFiles, @"REAPER\reaper.exe"),
                Path.Combine(ProgramFilesX86, @"REAPER\reaper.exe"),
                "reaper.exe",
            });
        }

        public string FindPath()
        {
            foreach (var path in DefaultReaperPaths)
            {
                if (File.Exists(path)) return path;
            }

            return null;
        }
    }
}
