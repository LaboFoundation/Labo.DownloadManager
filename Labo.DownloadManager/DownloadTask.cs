namespace Labo.DownloadManager
{
    using System;
    using System.IO;

    using Labo.DownloadManager.EventAggregator;
    using Labo.DownloadManager.EventArgs;
    using Labo.DownloadManager.Protocol;
    using Labo.DownloadManager.Segment;
    using Labo.DownloadManager.Settings;
    using Labo.DownloadManager.Streaming;

    /// <summary>
    /// The download task class.
    /// </summary>
    public sealed class DownloadTask : IDownloadTask
    {
        /// <summary>
        /// The network protocol provider factory
        /// </summary>
        private readonly INetworkProtocolProviderFactory m_NetworkProtocolProviderFactory;

        /// <summary>
        /// The download segment calculator
        /// </summary>
        private readonly IDownloadSegmentPositionsCalculator m_DownloadSegmentCalculator;

        /// <summary>
        /// The event manager
        /// </summary>
        private readonly IEventManager m_EventManager;

        /// <summary>
        /// The download file information
        /// </summary>
        private readonly DownloadFileInfo m_File;

        /// <summary>
        /// The download stream manager
        /// </summary>
        private readonly IDownloadStreamManager m_DownloadStreamManager;

        /// <summary>
        /// The download settings
        /// </summary>
        private readonly IDownloadSettings m_Settings;

        /// <summary>
        /// The download task create date
        /// </summary>
        private readonly DateTime m_CreateDate;

        /// <summary>
        /// The download retry count
        /// </summary>
        private int m_RetryCount = 1;

        /// <summary>
        /// The file size
        /// </summary>
        private long m_FileSize;

        /// <summary>
        /// The last exception
        /// </summary>
        private Exception m_LastException;

        /// <summary>
        /// The download start date
        /// </summary>
        private DateTime? m_StartDate;

        /// <summary>
        /// The segment download tasks
        /// </summary>
        private SegmentDownloadTaskCollection m_SegmentDownloadTasks;

        /// <summary>
        /// Determines whether download task is resumable
        /// </summary>
        private bool m_IsResumable;

        /// <summary>
        /// Gets the state of the download task.
        /// </summary>
        /// <value>
        /// The download task state.
        /// </value>
        public DownloadTaskState State { get; private set; }

        /// <summary>
        /// Determines whether download task is working.
        /// </summary>
        /// <returns>
        /// <c>true</c> if [is download task is working]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsWorking()
        {
            return State == DownloadTaskState.Working || State == DownloadTaskState.Prepared ||
                   State == DownloadTaskState.WaitingForReconnect || State == DownloadTaskState.Preparing;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadTask"/> class.
        /// </summary>
        /// <param name="networkProtocolProviderFactory">The network protocol provider factory.</param>
        /// <param name="downloadSegmentCalculator">The download segment calculator.</param>
        /// <param name="downloadStreamManager">The download stream manager.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="file">The file.</param>
        /// <param name="eventManager">The event manager.</param>
        public DownloadTask(
                           INetworkProtocolProviderFactory networkProtocolProviderFactory,
                           IDownloadSegmentPositionsCalculator downloadSegmentCalculator,
                           IDownloadStreamManager downloadStreamManager,
                           IDownloadSettings settings,
                           DownloadFileInfo file,
                           IEventManager eventManager)
        {
            m_NetworkProtocolProviderFactory = networkProtocolProviderFactory;
            m_DownloadSegmentCalculator = downloadSegmentCalculator;
            m_Settings = settings;
            m_File = file;
            m_DownloadStreamManager = downloadStreamManager;
            m_EventManager = eventManager;
            m_CreateDate = DateTime.Now;

            Guid = Guid.NewGuid();
        }

        /// <summary>
        /// Changes the state of the download task.
        /// </summary>
        /// <param name="downloadTaskState">State of the download task.</param>
        public void ChangeState(DownloadTaskState downloadTaskState)
        {
            State = downloadTaskState;

            m_EventManager.EventPublisher.Publish(new DownloadTaskStateChangedEventMessage(downloadTaskState));
        }

        /// <summary>
        /// Gets or sets the segment download tasks.
        /// </summary>
        /// <value>
        /// The segment download tasks.
        /// </value>
        private SegmentDownloadTaskCollection SegmentDownloadTasks
        {
            get
            {
                return m_SegmentDownloadTasks ?? (m_SegmentDownloadTasks = new SegmentDownloadTaskCollection());
            }

            set
            {
                m_SegmentDownloadTasks = value;
            }
        }

        /// <summary>
        /// Gets the event manager.
        /// </summary>
        /// <value>
        /// The event manager.
        /// </value>
        public IEventManager EventManager
        {
            get { return m_EventManager; }
        }

        /// <summary>
        /// Gets the GUID.
        /// </summary>
        /// <value>
        /// The GUID.
        /// </value>
        public Guid Guid { get; private set; }

        /// <summary>
        /// Starts the download task.
        /// </summary>
        public void StartDownload()
        {
            RemoteFileInfo remoteFileInfo = null;

            try
            {
                ChangeState(DownloadTaskState.Preparing);

                m_StartDate = DateTime.Now;

                INetworkProtocolProvider networkProtocolProvider = m_NetworkProtocolProviderFactory.CreateProvider(m_File.Uri);

                remoteFileInfo = networkProtocolProvider.GetRemoteFileInfo(m_File);
                remoteFileInfo.FileName = m_File.FileName;

                m_FileSize = remoteFileInfo.FileSize;
                m_IsResumable = remoteFileInfo.AcceptRanges;

                DownloadSegmentPositions[] segmentPositionInfos;

                if (remoteFileInfo.AcceptRanges)
                {
                    segmentPositionInfos = m_DownloadSegmentCalculator.Calculate(m_Settings.MinimumSegmentSize, m_Settings.MaximumSegmentCount, m_File.SegmentCount, remoteFileInfo.FileSize);                    
                }
                else
                {
                    segmentPositionInfos = new[] { new DownloadSegmentPositions(0, remoteFileInfo.FileSize - 1) };
                }

                using (Stream stream = m_DownloadStreamManager.CreateStream(remoteFileInfo))
                {
                    SegmentWriter segmentWriter = new SegmentWriter(stream);
                    SegmentDownloadTasks = new SegmentDownloadTaskCollection(segmentPositionInfos.Length);
                    for (int i = 0; i < segmentPositionInfos.Length; i++)
                    {
                        DownloadSegmentPositions segmentPosition = segmentPositionInfos[i];
                        SegmentDownloadTasks.Add(new DoubleBufferSegmentDownloadTask(m_Settings.DownloadBufferSize, new SegmentDownloader(m_File, networkProtocolProvider, segmentPosition, new SegmentDownloadRateCalculator(segmentPosition.StartPosition)), segmentWriter));
                    }

                    SegmentDownloadManager segmentDownloadManager = new SegmentDownloadManager(SegmentDownloadTasks);
                    segmentDownloadManager.Start();

                    ChangeState(DownloadTaskState.Working);

                    segmentDownloadManager.Finish(true);

                    ChangeState(DownloadTaskState.Ended);

                    m_EventManager.EventPublisher.Publish(new DownloadTaskFinishedEventMessage(this, stream, remoteFileInfo));
                }
            }
            catch (Exception ex)
            {
                if (m_RetryCount < m_Settings.MaximumRetries)
                {
                    m_RetryCount++;
                    
                    StartDownload();
                }
                else
                {
                    m_LastException = ex;

                    ChangeState(DownloadTaskState.EndedWithError);

                    m_EventManager.EventPublisher.Publish(new DownloadTaskFinishedEventMessage(this, null, remoteFileInfo));
                }
            }
        }

        /// <summary>
        /// Gets the download task statistics.
        /// </summary>
        /// <returns>The download statistics.</returns>
        public DownloadTaskStatistics GetDownloadTaskStatistics()
        {
            DownloadTaskStatistics downloadTaskStatistics = SegmentDownloadTasks.GetDownloadTaskStatistics();

            TimeSpan elapsedTime = TimeSpan.Zero;
            if (m_StartDate.HasValue)
            {
                elapsedTime = downloadTaskStatistics.IsDownloadFinished && downloadTaskStatistics.DownloadFinishDate.HasValue
                                           ? downloadTaskStatistics.DownloadFinishDate.Value - m_StartDate.Value
                                           : DateTime.Now - m_StartDate.Value;
            }

            downloadTaskStatistics.Guid = Guid;
            downloadTaskStatistics.DownloadTaskState = State;
            downloadTaskStatistics.FileUri = m_File.Uri;
            downloadTaskStatistics.FileName = m_File.FileName;
            downloadTaskStatistics.LastError = m_LastException == null ? string.Empty : m_LastException.Message;
            downloadTaskStatistics.CreatedDate = m_CreateDate;
            downloadTaskStatistics.IsDownloadResumable = m_IsResumable;
            downloadTaskStatistics.FileSize = m_FileSize;
            downloadTaskStatistics.DownloadProgress = m_FileSize <= 0 ? 0 : (downloadTaskStatistics.TransferedDownload / (double)m_FileSize * 100);
            downloadTaskStatistics.AverageDownloadRate = downloadTaskStatistics.TransferedDownload <= 0 ? new double?() : (Math.Abs(elapsedTime.TotalSeconds) < 1D ? 0 : downloadTaskStatistics.TransferedDownload / elapsedTime.TotalSeconds);
            return downloadTaskStatistics;
        }
    }
}
