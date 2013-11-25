namespace Labo.DownloadManager.Win.UI
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Windows.Forms;

    using Labo.DownloadManager.Segment;
    using Labo.DownloadManager.Win.UI.Helper;

    public partial class DownloadTaskList : UserControl
    {
        private readonly DownloadHelper m_DownloadHelper;
        private readonly Dictionary<ListViewItem, Guid> m_DownloadTaskItemMap;

        private ListViewItem m_LastDownloadTaskSelection;

        public DownloadTaskList()
        {
            InitializeComponent();

            m_DownloadHelper = new DownloadHelper();
            m_DownloadTaskItemMap = new Dictionary<ListViewItem, Guid>();
            m_DownloadHelper.Start();
        }

        public void AddNewDownloadTask(DownloadTaskInfo downloadTaskInfo)
        {
            Guid newTaskGuid = m_DownloadHelper.AddNewDownloadTask(downloadTaskInfo);

            ListViewItem item = new ListViewItem();

            DownloadTaskStatistics downloadTaskStatistics = m_DownloadHelper.GetDownloadTaskStatistics(newTaskGuid);

            string fileName = downloadTaskInfo.DownloadFileInfo.FileName;
            string fileExtension = Path.GetExtension(fileName);
            item.ImageIndex = FileTypeImageList.GetImageIndexByExtention(fileExtension);
            item.Text = Path.GetFileName(fileName);

            item.SubItems.Add(ByteFormatter.ToString(downloadTaskStatistics.FileSize));
            item.SubItems.Add(ByteFormatter.ToString(downloadTaskStatistics.TransferedDownload));
            item.SubItems.Add(GetDownloadProgressText(downloadTaskStatistics));
            item.SubItems.Add(TimeSpanFormatter.ToString(downloadTaskStatistics.RemainingTime));
            item.SubItems.Add("0");
            item.SubItems.Add(string.Format(CultureInfo.CurrentCulture, "{0} {1}", downloadTaskStatistics.CreatedDate.ToShortDateString(), downloadTaskStatistics.CreatedDate.ToShortTimeString()));
            item.SubItems.Add(downloadTaskStatistics.DownloadTaskState.ToString());
            item.SubItems.Add(GetResumeText(downloadTaskStatistics));
            item.SubItems.Add(downloadTaskStatistics.FileUrl);

            m_DownloadTaskItemMap[item] = newTaskGuid;

            lvwDownloadTasks.Items.Add(item);
        }

        public void UpdateList()
        {
            ListView.ListViewItemCollection listViewItemCollection = lvwDownloadTasks.Items;
            for (int i = 0; i < listViewItemCollection.Count; i++)
            {
                ListViewItem item = listViewItemCollection[i];
                if (item == null)
                {
                    return;
                }

                Guid guid = m_DownloadTaskItemMap[item];
                DownloadTaskStatistics downloadTaskStatistics = m_DownloadHelper.GetDownloadTaskStatistics(guid);
                if (downloadTaskStatistics == null)
                {
                    return;
                }

                DownloadTaskState state;

                if (item.Tag == null)
                {
                    state = DownloadTaskState.Working;
                }
                else
                {
                    state = (DownloadTaskState)item.Tag;
                }

                if (state != downloadTaskStatistics.DownloadTaskState ||
                    state == DownloadTaskState.Working ||
                    state == DownloadTaskState.WaitingForReconnect)
                {
                    item.SubItems[1].Text = ByteFormatter.ToString(downloadTaskStatistics.FileSize);
                    item.SubItems[2].Text = ByteFormatter.ToString(downloadTaskStatistics.TransferedDownload);
                    item.SubItems[3].Text = GetDownloadProgressText(downloadTaskStatistics);
                    item.SubItems[4].Text = TimeSpanFormatter.ToString(downloadTaskStatistics.RemainingTime);
                    item.SubItems[5].Text = GetDownloadRateText(downloadTaskStatistics);

                    if (!string.IsNullOrWhiteSpace(downloadTaskStatistics.LastError))
                    {
                        item.SubItems[7].Text = string.Format(CultureInfo.CurrentCulture, "{0}, {1}", downloadTaskStatistics.DownloadTaskState, downloadTaskStatistics.LastError);
                    }
                    else
                    {
                        item.SubItems[7].Text = string.IsNullOrEmpty(downloadTaskStatistics.StatusMessage) ? downloadTaskStatistics.DownloadTaskState.ToString() : string.Format(CultureInfo.CurrentCulture, "{0}, {1}", downloadTaskStatistics.DownloadTaskState, downloadTaskStatistics.StatusMessage);
                    }

                    item.SubItems[8].Text = GetResumeText(downloadTaskStatistics);
                    item.SubItems[9].Text = downloadTaskStatistics.FileUrl;
                    item.Tag = downloadTaskStatistics.DownloadTaskState;
                }
            }

            UpdateSegments();
        }

        private void UpdateSegments()
        {
            try
            {
                lvwDownloadTaskSegments.BeginUpdate();

                if (lvwDownloadTasks.SelectedItems.Count == 1)
                {
                    ListViewItem newDownloadTaskSelection = lvwDownloadTasks.SelectedItems[0];
                    Guid guid = m_DownloadTaskItemMap[newDownloadTaskSelection];
                    DownloadTaskStatistics downloadTaskStatistics = m_DownloadHelper.GetDownloadTaskStatistics(guid);

                    if (m_LastDownloadTaskSelection == newDownloadTaskSelection)
                    {
                        if (downloadTaskStatistics.Segments.Count == lvwDownloadTaskSegments.Items.Count)
                        {
                            UpdateSegmentsWithoutInsert(downloadTaskStatistics.Segments);
                        }
                        else
                        {
                            UpdateSegmentsInserting(newDownloadTaskSelection, downloadTaskStatistics.Segments);
                        }
                    }
                    else
                    {
                        UpdateSegmentsInserting(newDownloadTaskSelection, downloadTaskStatistics.Segments);
                    }
                }
                else
                {
                    m_LastDownloadTaskSelection = null;

                    lvwDownloadTaskSegments.Items.Clear();
                }
            }
            finally
            {
                lvwDownloadTaskSegments.EndUpdate();
            }
        }

        private void UpdateSegmentsWithoutInsert(SegmentDownloaderInfoCollection segmentDownloaderInfoCollection)
        {
            for (int i = 0; i < segmentDownloaderInfoCollection.Count; i++)
            {
                ISegmentDownloaderInfo segmentDownloaderInfo = segmentDownloaderInfoCollection[i];
                ListViewItem listViewItem = lvwDownloadTaskSegments.Items[i];
                listViewItem.SubItems[0].Text = string.Format("{0:0.##}%", segmentDownloaderInfo.DownloadProgress);
                listViewItem.SubItems[1].Text = ByteFormatter.ToString(segmentDownloaderInfo.TransferedDownload);
                listViewItem.SubItems[2].Text = ByteFormatter.ToString(segmentDownloaderInfo.EndPosition - segmentDownloaderInfo.StartPosition);
                listViewItem.SubItems[3].Text = ByteFormatter.ToString(segmentDownloaderInfo.StartPosition);
                listViewItem.SubItems[4].Text = ByteFormatter.ToString(segmentDownloaderInfo.EndPosition);
                listViewItem.SubItems[5].Text = string.Format("{0:0.##}", segmentDownloaderInfo.DownloadRate / 1024.0);
                listViewItem.SubItems[6].Text = TimeSpanFormatter.ToString(segmentDownloaderInfo.RemainingTime);        
                listViewItem.SubItems[7].Text = segmentDownloaderInfo.LastException != null ? string.Format(CultureInfo.CurrentCulture, "{0}, {1}", segmentDownloaderInfo.State, segmentDownloaderInfo.LastException.Message) : segmentDownloaderInfo.State.ToString();
                listViewItem.SubItems[8].Text = segmentDownloaderInfo.Url;
            }
        }

        private void UpdateSegmentsInserting(ListViewItem newSelection, SegmentDownloaderInfoCollection segmentDownloaderInfoCollection)
        {
            m_LastDownloadTaskSelection = newSelection;

            lvwDownloadTaskSegments.Items.Clear();

            for (int i = 0; i < segmentDownloaderInfoCollection.Count; i++)
            {
                ListViewItem item = new ListViewItem();

                ISegmentDownloaderInfo segmentDownloaderInfo = segmentDownloaderInfoCollection[i];
                item.Text = string.Format("{0:0.##}%", segmentDownloaderInfo.DownloadProgress);
                item.SubItems.Add(ByteFormatter.ToString(segmentDownloaderInfo.TransferedDownload));
                item.SubItems.Add(ByteFormatter.ToString(segmentDownloaderInfo.EndPosition - segmentDownloaderInfo.StartPosition));
                item.SubItems.Add(ByteFormatter.ToString(segmentDownloaderInfo.StartPosition));
                item.SubItems.Add(ByteFormatter.ToString(segmentDownloaderInfo.EndPosition));
                item.SubItems.Add(string.Format("{0:0.##}", segmentDownloaderInfo.DownloadRate / 1024.0));
                item.SubItems.Add(TimeSpanFormatter.ToString(segmentDownloaderInfo.RemainingTime));
                item.SubItems.Add(segmentDownloaderInfo.State.ToString());
                item.SubItems.Add(segmentDownloaderInfo.Url);

                lvwDownloadTaskSegments.Items.Add(item);
            }
        }

        private static string GetDownloadRateText(DownloadTaskStatistics downloadTaskStatistics)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0:0.##}", downloadTaskStatistics.DownloadRate / 1024.0);
        }

        private static string GetResumeText(DownloadTaskStatistics downloadTaskStatistics)
        {
            return downloadTaskStatistics.IsDownloadResumable ? "Yes" : "No";
        }

        private static string GetDownloadProgressText(DownloadTaskStatistics downloadTaskStatistics)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0:0.##}%", downloadTaskStatistics.DownloadProgress);
        }
    }
}
