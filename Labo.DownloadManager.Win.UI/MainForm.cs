namespace Labo.DownloadManager.Win.UI
{
    using System.Windows.Forms;

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            DownloadHelper downloadHelper = new DownloadHelper();
            downloadHelper.Start();

            downloadTaskList.Init(downloadHelper);
        }

        private void tsbNewDownload_Click(object sender, System.EventArgs e)
        {
            using (NewDownloadForm newDownloadForm = new NewDownloadForm())
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
        }
    }
}
