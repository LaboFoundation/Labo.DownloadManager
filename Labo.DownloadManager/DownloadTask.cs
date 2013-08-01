using System.Collections.Generic;
using System.IO;
<<<<<<< HEAD
using Labo.DownloadManager.EventAggregator;
using Labo.DownloadManager.Events;
=======
using System.Net;
using Labo.DownloadManager.EventAggregator;
using Labo.DownloadManager.EventArgs;
>>>>>>> 6bb4df88966e7f0c08b9450197079385b2b1d098
using Labo.DownloadManager.Protocol;
using Labo.DownloadManager.Settings;

namespace Labo.DownloadManager
{
    public sealed class DownloadTask : IDownloadTask
    {
        private readonly INetworkProtocolProviderFactory m_NetworkProtocolProviderFactory;
        private readonly IDownloadSegmentPositionsCalculator m_DownloadSegmentCalculator;
<<<<<<< HEAD
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
=======
        private readonly IDownloadStreamManager m_DownloadStreamManager;
        private readonly IDownloadSettings m_Settings;
        private readonly DownloadFile m_File;
        private readonly int m_SegmentCount;
        private readonly EventManager m_EventManager;

        public DownloadTask(INetworkProtocolProviderFactory networkProtocolProviderFactory, 
                            IDownloadSegmentPositionsCalculator downloadSegmentCalculator, 
                            IDownloadStreamManager downloadStreamManager,
                            IDownloadSettings settings,
                            DownloadFile file,
                            int segmentCount)
        {
            m_NetworkProtocolProviderFactory = networkProtocolProviderFactory;
            m_DownloadSegmentCalculator = downloadSegmentCalculator;
            m_Settings = settings;
            m_File = file;
            m_SegmentCount = segmentCount;
            m_DownloadStreamManager = downloadStreamManager;
            m_EventManager = new EventManager();
        }

        public IEventManager EventManager
        {
            get { return m_EventManager; }
>>>>>>> 6bb4df88966e7f0c08b9450197079385b2b1d098
        }

        public void StartDownload()
        {
            INetworkProtocolProvider networkProtocolProvider = m_NetworkProtocolProviderFactory.CreateProvider(m_File.Url);
            Stream initialInputStream = null;
            try
            {
                RemoteFileInfo remoteFileInfo = networkProtocolProvider.GetRemoteFileInfo(m_File, out initialInputStream);

                int maximumSegmentCount = m_Settings.MaximumSegmentCount;
                ServicePointManager.DefaultConnectionLimit = maximumSegmentCount;

                DownloadSegmentPositions[] segmentPositionInfos = m_DownloadSegmentCalculator.Calculate(m_Settings.MinimumSegmentSize, maximumSegmentCount, m_SegmentCount, remoteFileInfo.FileSize);

                using (Stream stream = m_DownloadStreamManager.CreateStream(remoteFileInfo))
                {
                    SegmentWriter segmentWriter = new SegmentWriter(stream);
                    IList<ISegmentDownloadTask> segmentDownloadTasks = new List<ISegmentDownloadTask>(segmentPositionInfos.Length);
                    for (int i = 0; i < segmentPositionInfos.Length; i++)
                    {
                        DownloadSegmentPositions segmentPosition = segmentPositionInfos[i];
                        if (i == 0)
                        {
                            segmentDownloadTasks.Add(new DoubleBufferSegmentDownloadTask(m_Settings.DownloadBufferSize, new SegmentDownloader(initialInputStream, segmentPosition, new SegmentDownloadRateCalculator(segmentPosition.StartPosition)), segmentWriter));
                        }
                        else
                        {
                            segmentDownloadTasks.Add(new DoubleBufferSegmentDownloadTask(m_Settings.DownloadBufferSize, new SegmentDownloader(m_File, networkProtocolProvider, segmentPosition, new SegmentDownloadRateCalculator(segmentPosition.StartPosition)), segmentWriter));
                        }
                    }

                    SegmentDownloadManager segmentDownloadManager = new SegmentDownloadManager(segmentDownloadTasks);
                    segmentDownloadManager.Start();

                    m_EventManager.EventPublisher.Publish(new DownloadTaskFinishedEventArgs(stream, remoteFileInfo));
                }
            }
            finally
            {
                if (initialInputStream != null)
                {
                    initialInputStream.Dispose();
                }
<<<<<<< HEAD

                m_SegmentDownloadManager = new SegmentDownloadManager(segmentDownloadTasks);
                m_SegmentDownloadManager.Start();
=======
>>>>>>> 6bb4df88966e7f0c08b9450197079385b2b1d098
            }
        }
    }
}
