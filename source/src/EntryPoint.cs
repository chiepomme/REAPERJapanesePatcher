using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace REAPERJapanesePatcher
{
    static class EntryPoint
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainView(new JapanesePatcher()));
        }
    }
}
