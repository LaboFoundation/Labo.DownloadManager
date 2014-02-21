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

    public sealed class DownloadTask : IDownloadTask
    {
        private readonly INetworkProtocolProviderFactory m_NetworkProtocolProviderFactory;
        private readonly IDownloadSegmentPositionsCalculator m_DownloadSegmentCalculator;
        private readonly IEventManager m_EventManager;
        private readonly DownloadFileInfo m_File;
        private readonly IDownloadStreamManager m_DownloadStreamManager;
        private readonly IDownloadSettings m_Settings;
        private int m_RetryCount = 1;

        private SegmentDownloadTaskCollection m_SegmentDownloadTasks;

        public DownloadTaskState State { get; private set; }

        public bool IsWorking()
        {
            return State == DownloadTaskState.Working || State == DownloadTaskState.Prepared ||
                   State == DownloadTaskState.WaitingForReconnect || State == DownloadTaskState.Preparing;
        }

        public DownloadTask(INetworkProtocolProviderFactory networkProtocolProviderFactory,
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
        }

        public void ChangeState(DownloadTaskState downloadTaskState)
        {
            State = downloadTaskState;

            m_EventManager.EventPublisher.Publish(new DownloadTaskStateChangedEventMessage(downloadTaskState));
        }

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

        public IEventManager EventManager
        {
            get { return m_EventManager; }
        }

        public void StartDownload()
        {
            try
            {
                ChangeState(DownloadTaskState.Preparing);

                INetworkProtocolProvider networkProtocolProvider = m_NetworkProtocolProviderFactory.CreateProvider(m_File.Url);

                RemoteFileInfo remoteFileInfo = networkProtocolProvider.GetRemoteFileInfo(m_File);
                remoteFileInfo.FileName = m_File.FileName;

                DownloadSegmentPositions[] segmentPositionInfos = m_DownloadSegmentCalculator.Calculate(m_Settings.MinimumSegmentSize, m_Settings.MaximumSegmentCount, m_File.SegmentCount, remoteFileInfo.FileSize);

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
            catch (Exception)
            {
                if (m_RetryCount < m_Settings.MaximumRetries)
                {
                    m_RetryCount++;
                    
                    StartDownload();
                }
                else
                {
                    ChangeState(DownloadTaskState.EndedWithError);
                }
            }
        }

        public DownloadTaskStatistics GetDownloadTaskStatistics()
        {
            DownloadTaskStatistics downloadTaskStatistics = SegmentDownloadTasks.GetDownloadTaskStatistics();
            downloadTaskStatistics.DownloadTaskState = State;
            downloadTaskStatistics.FileUrl = m_File.Url;
            return downloadTaskStatistics;
        }
    }
}
