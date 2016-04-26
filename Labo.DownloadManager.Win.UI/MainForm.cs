namespace Labo.DownloadManager.Win.UI
{
    using System.Globalization;
    using System.Windows.Forms;

    using Labo.DownloadManager.Settings;
    using Labo.DownloadManager.Streaming;
    using Labo.DownloadManager.Win.Controls.Helper;

    public partial class MainForm : Form
    {
        private readonly DownloadHelper m_DownloadHelper;

        private readonly IFileNameCorrector m_FileNameCorrector;

        public MainForm()
        {
            InitializeComponent();

            m_FileNameCorrector = new DefaultFileNameCorrector();
            m_DownloadHelper = new DownloadHelper(new InMemoryDownloadSettings(200, 5, 8192, 5, 5, 10 * 1000), new LocalFileDownloadStreamManager(new TempFileAllocator(m_FileNameCorrector)));
            m_DownloadHelper.Start();

            downloadTaskList.Init(m_DownloadHelper);
        }

        private void tsbNewDownload_Click(object sender, System.EventArgs e)
        {
            using (NewDownloadForm newDownloadForm = new NewDownloadForm(m_FileNameCorrector))
            {
                DialogResult dialogResult = newDownloadForm.ShowDialog(this);
                if (dialogResult == DialogResult.OK)
                {
                    downloadTaskList.AddNewDownloadTask(newDownloadForm.DownloadTaskInfo);
                }
            }
        }

        private void tmrRefresh_Tick(object sender, System.EventArgs e)
        {
            downloadTaskList.UpdateList();

            BeginInvoke(
                (MethodInvoker)delegate
                    {
                        toolStripStatusLabel.Text = string.Format(CultureInfo.CurrentCulture, "Speed: {0} / s", ByteFormatter.ToString(downloadTaskList.GetTotalDownloadRate()));
                    });
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_DownloadHelper.Shutdown(false);
        }
    }
}
