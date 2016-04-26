namespace Labo.DownloadManager.Win.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Windows.Forms;

    using Labo.DownloadManager.Segment;
    using Labo.DownloadManager.Win.Controls.Helper;

    public partial class DownloadTaskList : UserControl
    {
        private sealed class ListViewItemComparer : IComparer
        {
            private readonly int m_Col;

            public ListViewItemComparer(int column)
            {
                m_Col = column;
            }

            public int Compare(object x, object y)
            {
                ListViewItem listViewItem1 = (ListViewItem)x;
                ListViewItem listViewItem2 = (ListViewItem)y;

                ListView listView = listViewItem1.ListView;
                int order = listView.Columns[m_Col].ImageIndex == 2 ? -1 : 1;

                return order * string.CompareOrdinal(listViewItem1.SubItems[m_Col].Text, listViewItem2.SubItems[m_Col].Text);
            }
        }
  
        private DownloadHelper m_DownloadHelper;
        private Dictionary<ListViewItem, Guid> m_DownloadTaskItemMap;
        private readonly HashSet<Guid> m_DownloadTaskGuids = new HashSet<Guid>();
        private ListViewItem m_LastDownloadTaskSelection;

        public DownloadHelper DownloadHelper
        {
            get
            {
                if (m_DownloadHelper == null)
                {
                    throw new InvalidOperationException("Download Helper is not initialized.");
                }

                return m_DownloadHelper;
            }
        }

        public DownloadTaskList()
        {
            InitializeComponent();

            lvwDownloadTasks.ColumnClick += lvwDownloadTasks_ColumnClick;
            lvwDownloadTasks.SelectedIndexChanged += lvwDownloadTasks_SelectedIndexChanged;
            lblDownloadLocation.LinkClicked += lblDownloadLocation_LinkClicked;

            lvwDownloadTasks.Sorting = SortOrder.None;
            lvwDownloadTasks.SmallImageList = FileTypeImageList.GetImageList();

            picFileType.Image = FileTypeImageList.GetFileTypeIcon(".", IconReader.EnumIconSize.Large).ToBitmap();

            SetColumnHeaderImages();
        }

        public void Init(DownloadHelper downloadHelper)
        {
            m_DownloadHelper = downloadHelper;
            m_DownloadTaskItemMap = new Dictionary<ListViewItem, Guid>();
        }

        public void AddNewDownloadTask(DownloadTaskInfo downloadTaskInfo)
        {
            Guid newTaskGuid = DownloadHelper.AddNewDownloadTask(downloadTaskInfo);

            AddExistingDownloadTask(DownloadHelper.GetDownloadTaskStatistics(newTaskGuid));
        }

        public void UpdateList()
        {
            BeginInvoke(
                (MethodInvoker)delegate
                    {
                        UpdateListForNewDownloads();

                        UpdateTaskList();

                        UpdateDownloadTasksStatus();
                    });
        }

        public double GetTotalDownloadRate()
        {
            double result = 0;
            IList<DownloadTaskStatistics> downloadTaskStatisticsList = DownloadHelper.GetDownloadTaskStatistics();
            for (int i = 0; i < downloadTaskStatisticsList.Count; i++)
            {
                DownloadTaskStatistics downloadTaskStatistics = downloadTaskStatisticsList[i];
                if (downloadTaskStatistics.DownloadTaskState == DownloadTaskState.Working)
                {
                    result += downloadTaskStatistics.DownloadRate.GetValueOrDefault(0);
                }
            }

            return result;
        }

        private void UpdateDownloadTasksStatus()
        {
            BeginInvoke(
                (MethodInvoker)delegate
                    {
                        DownloadManager downloadManager = DownloadHelper.DownloadManager;
                        lblDownloadTasksInfo.Text = string.Format(
                            CultureInfo.InvariantCulture,
                            "Active Downloads: {0}, Waiting Queue Length: {1}",
                            downloadManager.ActiveDownloadsCount,
                            downloadManager.DownloadQueueLength);
                    });
        }

        private void UpdateTaskList()
        {
            ListView.ListViewItemCollection listViewItemCollection = lvwDownloadTasks.Items;
            for (int i = 0; i < listViewItemCollection.Count; i++)
            {
                ListViewItem item = listViewItemCollection[i];
                if (item == null)
                {
                    continue;
                }

                Guid guid = m_DownloadTaskItemMap[item];
                DownloadTaskStatistics downloadTaskStatistics = DownloadHelper.GetDownloadTaskStatistics(guid);
                if (downloadTaskStatistics == null)
                {
                    continue;
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

                if (state != downloadTaskStatistics.DownloadTaskState || state == DownloadTaskState.Working
                    || state == DownloadTaskState.WaitingForReconnect)
                {
                    item.SubItems[1].Text = ByteFormatter.ToString(downloadTaskStatistics.FileSize);
                    item.SubItems[2].Text = ByteFormatter.ToString(downloadTaskStatistics.TransferedDownload);
                    item.SubItems[3].Text = GetDownloadProgressText(downloadTaskStatistics);
                    item.SubItems[4].Text = TimeSpanFormatter.ToString(downloadTaskStatistics.RemainingTime);
                    item.SubItems[5].Text = GetDownloadRateText(downloadTaskStatistics);
                    item.SubItems[6].Text = GetAverageDownloadRateText(downloadTaskStatistics);

                    if (!string.IsNullOrWhiteSpace(downloadTaskStatistics.LastError))
                    {
                        item.SubItems[8].Text = string.Format(
                            CultureInfo.CurrentCulture,
                            "{0}, {1}",
                            downloadTaskStatistics.DownloadTaskState,
                            downloadTaskStatistics.LastError);
                    }
                    else
                    {
                        item.SubItems[8].Text = string.IsNullOrEmpty(downloadTaskStatistics.StatusMessage)
                                                    ? downloadTaskStatistics.DownloadTaskState.ToString()
                                                    : string.Format(
                                                        CultureInfo.CurrentCulture,
                                                        "{0}, {1}",
                                                        downloadTaskStatistics.DownloadTaskState,
                                                        downloadTaskStatistics.StatusMessage);
                    }

                    item.SubItems[9].Text = GetResumeText(downloadTaskStatistics);
                    item.SubItems[10].Text = downloadTaskStatistics.FileUri.OriginalString;
                    item.Tag = downloadTaskStatistics.DownloadTaskState;

                    UpdateSegments();
                }
            }
        }

        private void lvwDownloadTasks_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            int columnIndex = e.Column;

            ListView.ColumnHeaderCollection columns = lvwDownloadTasks.Columns;
            for (int i = 0; i < columns.Count; i++)
            {
                if (i != columnIndex)
                {
                    ColumnHeader column = columns[i];
                    column.ImageIndex = 0;
                }
            }

            ColumnHeader columnHeader = columns[columnIndex];
            int imageIndex = columnHeader.ImageIndex;

            columnHeader.ImageIndex = (imageIndex + 1) % 3;

            lvwDownloadTasks.ListViewItemSorter = new ListViewItemComparer(columnIndex);
        }

        private void AddExistingDownloadTask(DownloadTaskStatistics downloadTaskStatistics)
        {
            Guid taskGuid = downloadTaskStatistics.Guid;

            lock (m_DownloadTaskItemMap)
            {
                if (!m_DownloadTaskGuids.Contains(taskGuid))
                {
                    try
                    {
                        lvwDownloadTasks.BeginUpdate();

                        ListViewItem item = new ListViewItem();

                        DateTime createdDate = downloadTaskStatistics.CreatedDate;
                        string fileName = downloadTaskStatistics.FileName;
                        string fileExtension = Path.GetExtension(fileName);

                        item.ImageIndex = FileTypeImageList.GetImageIndexByExtension(fileExtension);
                        item.Text = Path.GetFileName(fileName);

                        item.SubItems.Add(ByteFormatter.ToString(downloadTaskStatistics.FileSize));
                        item.SubItems.Add(ByteFormatter.ToString(downloadTaskStatistics.TransferedDownload));
                        item.SubItems.Add(GetDownloadProgressText(downloadTaskStatistics));
                        item.SubItems.Add(TimeSpanFormatter.ToString(downloadTaskStatistics.RemainingTime));
                        item.SubItems.Add("0");
                        item.SubItems.Add("0");
                        item.SubItems.Add(string.Format(CultureInfo.CurrentCulture, "{0} {1}", createdDate.ToShortDateString(), createdDate.ToLongTimeString()));
                        item.SubItems.Add(downloadTaskStatistics.DownloadTaskState.ToString());
                        item.SubItems.Add(GetResumeText(downloadTaskStatistics));
                        item.SubItems.Add(downloadTaskStatistics.FileUri.OriginalString);

                        m_DownloadTaskItemMap[item] = taskGuid;
                        m_DownloadTaskGuids.Add(taskGuid);

                        lvwDownloadTasks.Items.Add(item);
                    }
                    finally
                    {
                        lvwDownloadTasks.EndUpdate();
                    }
                }
            }
        }

        private void SetColumnHeaderImages()
        {
            ListView.ColumnHeaderCollection columns = lvwDownloadTasks.Columns;
            for (int i = 0; i < columns.Count; i++)
            {
                ColumnHeader column = columns[i];
                column.ImageList.Images.Add(Resources.arrow_empty_icon);
                column.ImageList.Images.Add(Resources.arrow_up);
                column.ImageList.Images.Add(Resources.arrow_down);

                column.ImageIndex = 0;
            }
        }

        private void UpdateListForNewDownloads()
        {
            IList<DownloadTaskStatistics> downloadTaskStatisticsList = DownloadHelper.GetDownloadTaskStatistics();
            for (int i = 0; i < downloadTaskStatisticsList.Count; i++)
            {
                DownloadTaskStatistics downloadTaskStatistics = downloadTaskStatisticsList[i];
                AddExistingDownloadTask(downloadTaskStatistics);
            }
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
                    DownloadTaskStatistics downloadTaskStatistics = DownloadHelper.GetDownloadTaskStatistics(guid);

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

        private void UpdateDownloadTaskInfo()
        {
            if (lvwDownloadTasks.SelectedItems.Count == 1)
            {
                BeginInvoke(
                    (MethodInvoker)delegate
                        {
                            ListViewItem newDownloadTaskSelection = lvwDownloadTasks.SelectedItems[0];
                            Guid guid = m_DownloadTaskItemMap[newDownloadTaskSelection];
                            DownloadTaskStatistics downloadTaskStatistics = DownloadHelper.GetDownloadTaskStatistics(guid);

                            string fileName = downloadTaskStatistics.FileName;
                            lblFileNameValue.Text = fileName;
                            lblDownloadLocation.Text = downloadTaskStatistics.DownloadLocation;
                            lblAverageSpeedValue.Text = GetDownloadRateText(downloadTaskStatistics);
                            lblUrlValue.Text = downloadTaskStatistics.FileUri.ToString();
                            lblSizeValue.Text = string.Format("Total {0}, Downloaded {1}", ByteFormatter.ToString(downloadTaskStatistics.FileSize), ByteFormatter.ToString(downloadTaskStatistics.TransferedDownload));
                            picFileType.Image = FileTypeImageList.GetFileTypeIcon(Path.GetExtension(fileName), IconReader.EnumIconSize.Large).ToBitmap();
                        });
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
                listViewItem.SubItems[2].Text = ByteFormatter.ToString(segmentDownloaderInfo.Size);
                listViewItem.SubItems[3].Text = ByteFormatter.ToString(segmentDownloaderInfo.StartPosition);
                listViewItem.SubItems[4].Text = ByteFormatter.ToString(segmentDownloaderInfo.EndPosition);
                listViewItem.SubItems[5].Text = string.Format("{0:0.##}", segmentDownloaderInfo.DownloadRate / 1024.0);
                listViewItem.SubItems[6].Text = TimeSpanFormatter.ToString(segmentDownloaderInfo.RemainingTime);        
                listViewItem.SubItems[7].Text = segmentDownloaderInfo.LastException != null ? string.Format(CultureInfo.CurrentCulture, "{0}, {1}", segmentDownloaderInfo.State, segmentDownloaderInfo.LastException.Message) : segmentDownloaderInfo.State.ToString();
                listViewItem.SubItems[8].Text = segmentDownloaderInfo.Uri.OriginalString;
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
                item.SubItems.Add(segmentDownloaderInfo.Uri.OriginalString);

                lvwDownloadTaskSegments.Items.Add(item);
            }
        }

        private static string GetDownloadRateText(DownloadTaskStatistics downloadTaskStatistics)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0:0.##}", downloadTaskStatistics.DownloadRate / 1024.0);
        }

        private static string GetAverageDownloadRateText(DownloadTaskStatistics downloadTaskStatistics)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0:0.##}", downloadTaskStatistics.AverageDownloadRate / 1024.0);
        }

        private static string GetResumeText(DownloadTaskStatistics downloadTaskStatistics)
        {
            return downloadTaskStatistics.IsDownloadResumable ? "Yes" : "No";
        }

        private static string GetDownloadProgressText(DownloadTaskStatistics downloadTaskStatistics)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0:0.##}%", downloadTaskStatistics.DownloadProgress);
        }

        private void DownloadTaskMenuStripOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            BeginInvoke(
                (MethodInvoker)delegate
                    {
                        if (lvwDownloadTasks.SelectedItems.Count > 0)
                        {
                            ListViewItem listViewItem = lvwDownloadTasks.SelectedItems[0];
                            Guid guid = m_DownloadTaskItemMap[listViewItem];

                            DownloadTaskStatistics downloadTaskStatistics = DownloadHelper.GetDownloadTaskStatistics(guid);

                            DownloadTaskState downloadTaskState = downloadTaskStatistics.DownloadTaskState;

                            pauseToolStripMenuItem.Enabled = DownloadTask.IsWorking(downloadTaskState);
                            startToolStripMenuItem.Enabled = downloadTaskState == DownloadTaskState.Paused
                                                             || downloadTaskState == DownloadTaskState.Stopped
                                                             || downloadTaskState == DownloadTaskState.Ended
                                                             || downloadTaskState == DownloadTaskState.EndedWithError;
                        }
                        else
                        {
                            pauseToolStripMenuItem.Enabled = false;
                        }
                    });
        }

        private void PauseToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (lvwDownloadTasks.SelectedItems.Count > 0)
            {
                ListViewItem listViewItem = lvwDownloadTasks.SelectedItems[0];
                Guid guid = m_DownloadTaskItemMap[listViewItem];

                DownloadHelper.PauseDownloadTask(guid);
            }
        }

        private void StartToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (lvwDownloadTasks.SelectedItems.Count > 0)
            {
                ListViewItem listViewItem = lvwDownloadTasks.SelectedItems[0];
                Guid guid = m_DownloadTaskItemMap[listViewItem];

                DownloadHelper.StartDownloadTask(guid);
            }
        }

        private void lvwDownloadTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDownloadTaskInfo();
        }

        private void lblDownloadLocation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string location = lblDownloadLocation.Text;
            if (!string.IsNullOrWhiteSpace(location))
            {
                Process.Start(location);
            }
        }

        private void lblCopyUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = lblUrlValue.Text;
            if (!string.IsNullOrWhiteSpace(url))
            {
                Clipboard.SetText(url);
            }
        }
    }
}
