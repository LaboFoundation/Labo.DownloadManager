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
        /// Determines whether download task is resumable
        /// </summary>
        private bool m_IsResumable;

        /// <summary>
        /// The segment download manager
        /// </summary>
        private SegmentDownloadManager m_SegmentDownloadManager;

        /// <summary>
        /// The lock object
        /// </summary>
        private static readonly object s_LockObject = new object();

        private SegmentWriter m_SegmentWriter;

        private RemoteFileInfo m_RemoteFileInfo;

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
            DownloadTaskState downloadTaskState = State;
            return IsWorking(downloadTaskState);
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
        public void StartDownload(bool restart = false)
        {
            try
            {
                if (m_SegmentDownloadManager == null || restart)
                {
                    lock (s_LockObject)
                    {
                        if (m_SegmentDownloadManager == null || restart)
                        {
                            ChangeState(DownloadTaskState.Preparing);

                            m_StartDate = DateTime.Now;

                            INetworkProtocolProvider networkProtocolProvider = m_NetworkProtocolProviderFactory.CreateProvider(m_File.Uri);

                            m_RemoteFileInfo = networkProtocolProvider.GetRemoteFileInfo(new DownloadFileRequestInfo(m_File));
                            m_RemoteFileInfo.FileName = m_File.FileName;

                            m_FileSize = m_RemoteFileInfo.FileSize;
                            m_IsResumable = m_RemoteFileInfo.AcceptRanges;

                            DownloadSegmentPositions[] segmentPositionInfos;

                            if (m_RemoteFileInfo.AcceptRanges)
                            {
                                segmentPositionInfos = m_DownloadSegmentCalculator.Calculate(m_Settings.MinimumSegmentSize, m_Settings.MaximumSegmentCount, m_File.SegmentCount, m_RemoteFileInfo.FileSize);
                            }
                            else
                            {
                                segmentPositionInfos = new[] { new DownloadSegmentPositions(0, m_RemoteFileInfo.FileSize - 1) };
                            }

                            m_SegmentWriter = new SegmentWriter();

                            SegmentDownloadTaskCollection segmentDownloadTasks = new SegmentDownloadTaskCollection(segmentPositionInfos.Length);
                            for (int i = 0; i < segmentPositionInfos.Length; i++)
                            {
                                DownloadSegmentPositions segmentPosition = segmentPositionInfos[i];
                                segmentDownloadTasks.Add(new DoubleBufferSegmentDownloadTask(m_Settings.DownloadBufferSize, m_Settings.MaximumRetries, m_Settings.RetryDelayTimeInMilliseconds, new SegmentDownloader(m_File, networkProtocolProvider, segmentPosition, new SegmentDownloadRateCalculator()), m_SegmentWriter));
                            }

                            m_SegmentDownloadManager = new SegmentDownloadManager(segmentDownloadTasks);

                            ChangeState(DownloadTaskState.Prepared);
                        }
                    }
                }

                using (Stream stream = m_DownloadStreamManager.CreateStream(m_RemoteFileInfo))
                {
                    m_SegmentWriter.SetStream(stream);

                    if (State == DownloadTaskState.Paused)
                    {
                        return;
                    }

                    m_SegmentDownloadManager.Start();

                    ChangeState(DownloadTaskState.Working);

                    m_SegmentDownloadManager.Finish(true);

                    if (State != DownloadTaskState.Paused)
                    {
                        ChangeState(DownloadTaskState.Ended);                        
                    }

                    m_EventManager.EventPublisher.Publish(new DownloadTaskFinishedEventMessage(this, stream, m_RemoteFileInfo));
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

                    m_EventManager.EventPublisher.Publish(new DownloadTaskFinishedEventMessage(this, null, m_RemoteFileInfo));
                }
            }
        }

        /// <summary>
        /// Pauses the download task.
        /// </summary>
        public void PauseDownload()
        {
            if (m_SegmentDownloadManager != null)
            {
                lock (s_LockObject)
                {
                    if (m_SegmentDownloadManager != null)
                    {
                        ChangeState(DownloadTaskState.Pausing);

                        m_SegmentDownloadManager.Pause();

                        ChangeState(DownloadTaskState.Paused);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the download task statistics.
        /// </summary>
        /// <returns>The download statistics.</returns>
        public DownloadTaskStatistics GetDownloadTaskStatistics()
        {
            DownloadTaskStatistics downloadTaskStatistics;
            SegmentDownloadManager segmentDownloadManager = m_SegmentDownloadManager;
            if (segmentDownloadManager == null)
            {
                downloadTaskStatistics = new DownloadTaskStatistics();
            }
            else
            {
                downloadTaskStatistics = segmentDownloadManager.SegmentDownloadTasks.GetDownloadTaskStatistics();

                TimeSpan elapsedTime = TimeSpan.Zero;
                if (m_StartDate.HasValue)
                {
                    elapsedTime = downloadTaskStatistics.IsDownloadFinished && downloadTaskStatistics.DownloadFinishDate.HasValue
                                               ? downloadTaskStatistics.DownloadFinishDate.Value - m_StartDate.Value
                                               : DateTime.Now - m_StartDate.Value;
                }

                downloadTaskStatistics.DownloadProgress = m_FileSize <= 0 ? 0 : (downloadTaskStatistics.TransferedDownload / (double)m_FileSize * 100);
                downloadTaskStatistics.AverageDownloadRate = downloadTaskStatistics.TransferedDownload <= 0 ? new double?() : (Math.Abs(elapsedTime.TotalSeconds) < 1D ? 0 : downloadTaskStatistics.TransferedDownload / elapsedTime.TotalSeconds);
            }
            
            downloadTaskStatistics.Guid = Guid;
            downloadTaskStatistics.DownloadTaskState = State;
            downloadTaskStatistics.FileUri = m_File.Uri;
            downloadTaskStatistics.FileName = m_RemoteFileInfo == null ? m_File.FileName : m_RemoteFileInfo.FileName;
            downloadTaskStatistics.DownloadLocation = m_File.DownloadLocation;
            downloadTaskStatistics.LastError = m_LastException == null ? string.Empty : m_LastException.Message;
            downloadTaskStatistics.CreatedDate = m_CreateDate;
            downloadTaskStatistics.IsDownloadResumable = m_IsResumable;
            downloadTaskStatistics.FileSize = m_FileSize;

            return downloadTaskStatistics;
        }

        /// <summary>
        /// Determines whether the specified download task state is working.
        /// </summary>
        /// <param name="downloadTaskState">State of the download task.</param>
        /// <c>true</c> if [is download task state is working]; otherwise, <c>false</c>.
        public static bool IsWorking(DownloadTaskState downloadTaskState)
        {
            return downloadTaskState == DownloadTaskState.Working 
                   || downloadTaskState == DownloadTaskState.Prepared
                   || downloadTaskState == DownloadTaskState.WaitingForReconnect
                   || downloadTaskState == DownloadTaskState.Preparing;
        }
    }
}
