using System.Windows.Forms;

namespace Labo.DownloadManager.Win.UI
{
    using System;
    using System.IO;

    public partial class NewDownloadForm : Form
    {
        public DownloadTaskInfo DownloadTaskInfo { get; private set; }

        public NewDownloadForm()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DownloadTaskInfo = new DownloadTaskInfo(new DownloadFileInfo(new Uri(txtUrl.Text.Trim()), Path.Combine(txtDownloadFolder.Text.Trim(), txtFileName.Text.Trim()), (int)nudSegmentsCount.Value), cbStartNow.Checked);
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDownloadFolder_Click(object sender, System.EventArgs e)
        {
            DialogResult dialogResult = fbdDownloadFolder.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                txtDownloadFolder.Text = fbdDownloadFolder.SelectedPath;
            }
        }
    }
}
