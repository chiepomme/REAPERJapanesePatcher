using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace REAPERJapanesePatcher
{
    public partial class MainView : Form
    {
        readonly JapanesePatcher Patcher;

        public MainView(JapanesePatcher patcher)
        {
            InitializeComponent();
            Patcher = patcher;

            TargetReaperBox.Text = Patcher.ReaperPath;

            FontFixCheck.Checked = Patcher.IsFontFixNeeded;
            IncludeAllCheck.Checked = Patcher.IncludesAllFiles;

            switch (Patcher.SelectedLangPack)
            {
                case JapanesePatcher.LangPackType.Master:
                    MastarLangPackRadio.Checked = true;
                    break;
                case JapanesePatcher.LangPackType.Translation:
                    TranslationLangPackRadio.Checked = true;
                    break;
            }
        }

        private void BrowseReaperButton_Click(object sender, System.EventArgs e)
        {
            ReaperBrowseDialog.ShowDialog();
            var path = ReaperBrowseDialog.FileName;

            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                TargetReaperBox.Text = path;
                Patcher.ReaperPath = path;
            }
        }

        private void FontFixCheck_CheckedChanged(object sender, System.EventArgs e)
        {
            Patcher.IsFontFixNeeded = FontFixCheck.Checked;
        }

        private void IncludeAllCheck_CheckedChanged(object sender, System.EventArgs e)
        {
            Patcher.IncludesAllFiles = IncludeAllCheck.Checked;
        }

        private void LangPackRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (MastarLangPackRadio.Checked)
                Patcher.SelectedLangPack = JapanesePatcher.LangPackType.Master;
            else if (TranslationLangPackRadio.Checked)
                Patcher.SelectedLangPack = JapanesePatcher.LangPackType.Translation;
        }

        private async void ExecuteButton_Click(object sender, System.EventArgs e)
        {
            ExecuteButton.Enabled = false;
            SettingPanel.Enabled = false;

            if (Patcher.IsFontFixNeeded)
            {
                var fontProgress = new Progress<FontFixProgress>(p =>
                {
                    ProgressLabel.Text = p.CurrentFile + "を処理中";
                    ProgressBar.Maximum = p.NumOfFiles;
                    ProgressBar.Value = p.CurrentIndex;
                });

                await Patcher.FixFonts(fontProgress);
                ProgressBar.Value = 0;
                ProgressLabel.Text = "フォントサイズの修正が完了しました";
            }

            ProgressLabel.Text = "日本語言語パックをダウンロード中";
            var downloadProgress = new Progress<DownloadProgress>(p =>
            {
                ProgressBar.Maximum = 1000;
                var rate = (double)p.DownloadedLength / p.TotalLength;
                ProgressBar.Value = (int)(rate * ProgressBar.Maximum);
            });

            await Patcher.DownloadLangPack(downloadProgress);
            ProgressLabel.Text = "日本語言語パックのダウンロードが完了しました";
            Patcher.InstallLangPack();

            ProgressLabel.Text = "日本語化が完了しました。REAPER 上で OK を押した後、REAPER を再起動してください。";

            ExecuteButton.Enabled = true;
            SettingPanel.Enabled = true;
        }

        private void AuthorLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/chiepomme/REAPERJapanesePatcher");
        }
    }
}
