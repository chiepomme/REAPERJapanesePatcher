using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace REAPERJapanesePatcher
{
    public class ReaperBinaryFixController
    {
        const string MainExecutionName = "reaper.exe";
        List<IBinaryFixer> MainExecutionFixers = new List<IBinaryFixer>() { 
            new FontSizeFixer(),
            new RoutingDialogFixer(),
        };

        List<IBinaryFixer> OtherFileFixers = new List<IBinaryFixer>() { 
            new FontSizeFixer(),
        };

        public async Task Fix(string path)
        {
            var bytes = await AsyncFile.ReadAllBytes(path);

            if (Path.GetFileName(path).ToLower() == MainExecutionName)
                MainExecutionFixers.ForEach(fixer => fixer.Fix(bytes));
            else
                OtherFileFixers.ForEach(fixer => fixer.Fix(bytes));

            await AsyncFile.WriteAllBytes(path, bytes);
        }
    }
}
