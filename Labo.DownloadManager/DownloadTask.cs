using System.Collections.Generic;
using System.IO;
using Labo.DownloadManager.EventAggregator;
using Labo.DownloadManager.Events;
using Labo.DownloadManager.Protocol;

namespace Labo.DownloadManager
{
    public sealed class DownloadTask : IDownloadTask
    {
        private readonly INetworkProtocolProviderFactory m_NetworkProtocolProviderFactory;
        private readonly IDownloadSegmentPositionsCalculator m_DownloadSegmentCalculator;
        private readonly ILocalFileAllocator m_LocalFileAllocator;
        private readonly IEventManager m_EventManager;
        private readonly DownloadFileInfo m_File;
        private readonly int m_SegmentCount;
        private SegmentDownloadManager m_SegmentDownloadManager;

        public DownloadTaskState State { get; private set; }

        public bool IsWorking()
        {
            return State == DownloadTaskState.Working || State == DownloadTaskState.Prepared ||
                   State == DownloadTaskState.WaitingForReconnect || State == DownloadTaskState.Preparing;
        }

        public DownloadTask(INetworkProtocolProviderFactory networkProtocolProviderFactory, 
            IDownloadSegmentPositionsCalculator downloadSegmentCalculator, 
            ILocalFileAllocator localFileAllocator,
            IEventManager eventManager,
            DownloadFileInfo file)
        {
            m_NetworkProtocolProviderFactory = networkProtocolProviderFactory;
            m_DownloadSegmentCalculator = downloadSegmentCalculator;
            m_LocalFileAllocator = localFileAllocator;
            m_EventManager = eventManager;
            m_File = file;
            m_SegmentCount = file.SegmentCount;
        }

        public void ChangeState(DownloadTaskState downloadTaskState)
        {
            State = downloadTaskState;

            m_EventManager.EventPublisher.Publish(new DownloadTaskStateChangedEventMessage(downloadTaskState));
        }

        public void StartDownload()
        {
            INetworkProtocolProvider networkProtocolProvider = m_NetworkProtocolProviderFactory.CreateProvider(m_File.Url);
            RemoteFileInfo remoteFileInfo = networkProtocolProvider.GetRemoteFileInfo(m_File);
            DownloadSegmentPositions[] segmentPositionInfos = m_DownloadSegmentCalculator.Calculate(200, 5, m_SegmentCount, remoteFileInfo.FileSize);
            LocalFileInfo localFileInfo = m_LocalFileAllocator.AllocateFile(remoteFileInfo.FileName, remoteFileInfo.FileSize);

            using (FileStream fs = new FileStream(localFileInfo.FileName, FileMode.Open, FileAccess.Write))
            {
                SegmentWriter segmentWriter = new SegmentWriter(fs);
                IList<ISegmentDownloadTask> segmentDownloadTasks = new List<ISegmentDownloadTask>(segmentPositionInfos.Length);
                for (int i = 0; i < segmentPositionInfos.Length; i++)
                {
                    DownloadSegmentPositions segmentPosition = segmentPositionInfos[i];
                    Stream segmentDownloaderStream = networkProtocolProvider.CreateStream(m_File, segmentPosition.StartPosition, segmentPosition.EndPosition);
                    segmentDownloadTasks.Add(new DoubleBufferSegmentDownloadTask(8192, new SegmentDownloader(segmentDownloaderStream, segmentPosition, new SegmentDownloadRateCalculator(segmentPosition.StartPosition)), segmentWriter));
                }

                m_SegmentDownloadManager = new SegmentDownloadManager(segmentDownloadTasks);
                m_SegmentDownloadManager.Start();
            }
        }
    }
}
