using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;

namespace Labo.DownloadManager.Tests.FileServer
{
    public partial class MainForm : Form, ILoggingSource
    {
        private readonly Server m_Server;
        private readonly LimittedCollection<string> m_Logs;

        public MainForm()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            m_Server = new Server(new Logger(new Collection<ILoggingSource>{ this }));
            m_Logs = new LimittedCollection<string>(100);

            Closing += MainForm_Closing;
        }

        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (m_Server.Started)
            {
                m_Server.Stop();
            }
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (m_Server.Started)
            {
                m_Server.Stop();
            }
            else
            {
                m_Server.Start(txtBaseAddress.Text.Trim());
            }
        }

        public void Write(string message)
        {
            lock (m_Logs)
            {
                string formattedMessage = string.Format("{0} at {1}.", message, DateTime.Now);
                m_Logs.Add(formattedMessage);
                txtLogs.Lines = m_Logs.Items.ToArray();
            }
        }
    }
}
