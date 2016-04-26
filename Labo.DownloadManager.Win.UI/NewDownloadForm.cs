namespace Labo.DownloadManager.Win.UI
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using Labo.DownloadManager.Protocol;
    using Labo.DownloadManager.Win.Controls.Helper;

    public partial class NewDownloadForm : Form
    {
        private readonly IFileNameCorrector m_FileNameCorrector;

        private bool m_FileNameTextChangedByUser;

        private bool m_UrlTextChanged;

        private long? m_DownloadFileSize;

        private DriveInfo m_DriveInfo;

        public DownloadTaskInfo DownloadTaskInfo { get; private set; }

        private string DownloadUrl
        {
            get
            {
                return txtUrl.Text.Trim();
            }
        }

        private string DownloadFolder
        {
            get
            {
                return txtDownloadFolder.Text.Trim();
            }
        }

        public NewDownloadForm(IFileNameCorrector fileNameCorrector)
        {
            m_FileNameCorrector = fileNameCorrector;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            txtDownloadFolder.Text = @"C:\Downloads\";

            if (Clipboard.ContainsText())
            {
                string urlCandidate = Clipboard.GetText();

                Uri uri;
                if (TryParseUri(urlCandidate, out uri))
                {
                    txtUrl.Text = urlCandidate;

                    UpdateFileInfo(uri);
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string fileName = txtFileName.Text.Trim();

            fileName = GetCorrectLocalFileName(fileName);

            DownloadTaskInfo = new DownloadTaskInfo(new DownloadFileInfo(new Uri(DownloadUrl), Path.Combine(DownloadFolder, fileName), (int)nudSegmentsCount.Value), cbStartNow.Checked);
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

        private void txtUrl_TextChanged(object sender, EventArgs e)
        {
            m_UrlTextChanged = true;
            m_DownloadFileSize = null;

            string url = DownloadUrl;
            Uri uri;
            if (TryParseUri(url, out uri))
            {
                string[] segments = uri.LocalPath.Split('/');
                int length = segments.Length;
                if (length > 0)
                {
                    string lastSegment = WebUtility.UrlDecode(segments[length - 1]);
                    if (!string.IsNullOrWhiteSpace(lastSegment))
                    {
                        string fileName = GetCorrectLocalFileName(lastSegment);

                        txtFileName.Text = fileName;
                    }
                }
            }
        }

        private static bool TryParseUri(string url, out Uri uri)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out uri);
        }

        private static string RemoveInvalidFileNameChars(string fileName)
        {
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            for (int i = 0; i < invalidFileNameChars.Length; i++)
            {
                char invalidFileNameChar = invalidFileNameChars[i];
                fileName = fileName.Replace(invalidFileNameChar, '_');
            }

            return fileName;
        }

        private void txtFileName_TextChanged(object sender, EventArgs e)
        {
            if (txtFileName.Focused)
            {
                m_FileNameTextChangedByUser = true;
            }
        }

        private void txtUrl_Leave(object sender, EventArgs e)
        {
            if (m_UrlTextChanged)
            {
                m_UrlTextChanged = false;

                UpdateFileInfo();
            }
        }

        private void UpdateFileInfo()
        {
            Uri uri;
            if (TryParseUri(txtUrl.Text, out uri))
            {
                UpdateFileInfo(uri);
            }
        }

        private void UpdateFileInfo(Uri uri)
        {
            if (uri == null)
            {
                return;
            }

            Task.Factory.StartNew(
                () =>
                    {
                        RemoteFileInfo remoteFileInfo = TryToGetRemoteFileInfo(uri);
                        if (remoteFileInfo != null)
                        {
                            string fileName = remoteFileInfo.FileName;
                            if (!m_FileNameTextChangedByUser && !string.IsNullOrWhiteSpace(fileName))
                            {
                                txtFileName.UpdateControlThreadSafely(x => x.Text = GetCorrectLocalFileName(fileName));
                            }

                            m_DownloadFileSize = remoteFileInfo.FileSize;

                            SetDownloadInfo();
                        }
                    });
        }

        private void SetDownloadInfo()
        {
            lblInfo.UpdateControlThreadSafely(
                x =>
                    {
                        long freeSpace = -1;
                        string driveName = null;

                        DriveInfo driveInfo = m_DriveInfo;
                        if (driveInfo != null)
                        {
                            freeSpace = driveInfo.AvailableFreeSpace;
                            driveName = driveInfo.Name;
                        }

                        StringBuilder sb = new StringBuilder();
                        if (freeSpace > -1)
                        {
                            sb.AppendFormat("Free space in {0} is {1}", driveName, ByteFormatter.ToString(freeSpace));
                        }

                        long? fileSize = m_DownloadFileSize;
                        if (fileSize != null)
                        {
                            sb.AppendFormat(", File size is {0}", ByteFormatter.ToString(fileSize.Value));
                        }

                        x.Text = sb.ToString();
                    });
        }

        private static DriveInfo GetDriveInfo(string path)
        {
            if (Path.IsPathRooted(path))
            {
                string driveName = Path.GetPathRoot(path);
                if (driveName != null)
                {
                    return new DriveInfo(driveName);
                }
            }

            return null;
        }

        private static RemoteFileInfo TryToGetRemoteFileInfo(Uri uri)
        {
            INetworkProtocolProvider networkProtocolProvider = DownloadManagerRuntime.NetworkProtocolProviderFactory.CreateProvider(uri);
            if (networkProtocolProvider != null)
            {
                return networkProtocolProvider.GetRemoteFileInfo(new DownloadFileRequestInfo(uri));
            }

            return null;
        }

        private void txtDownloadFolder_TextChanged(object sender, EventArgs e)
        {
            Task.Factory.StartNew(
                () =>
                    {
                        m_DriveInfo = GetDriveInfo(DownloadFolder);

                        SetDownloadInfo();
                    });
        }

        private void txtUrl_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DownloadUrl.Length == 0)
            {
                errorProvider.SetError(txtUrl, "Url field cannot be empty.");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(txtUrl, string.Empty);
            }
        }

        private string GetCorrectLocalFileName(string fileName)
        {
            string localFileName = RemoveInvalidFileNameChars(fileName);
            return GetUniqueLocalFileName(DownloadFolder, localFileName);
        }

        private string GetUniqueLocalFileName(string downloadFolder, string fileName)
        {
            string fullPathFileName = Path.Combine(downloadFolder, fileName);
            string uniqueFilePath = m_FileNameCorrector.GetFileName(fullPathFileName);
            return Path.GetFileName(uniqueFilePath);
        }
    }
}
