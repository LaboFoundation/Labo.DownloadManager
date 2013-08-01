using System.Collections.Generic;
using System.IO;
using Labo.DownloadManager.EventAggregator;
using Labo.DownloadManager.Events;
using System.Net;
using Labo.DownloadManager.EventArgs;
using Labo.DownloadManager.Protocol;
using Labo.DownloadManager.Settings;

namespace Labo.DownloadManager
{
    public sealed class DownloadTask : IDownloadTask
    {
        private readonly INetworkProtocolProviderFactory m_NetworkProtocolProviderFactory;
        private readonly IDownloadSegmentPositionsCalculator m_DownloadSegmentCalculator;
        private readonly IEventManager m_EventManager;
        private readonly DownloadFileInfo m_File;
        private readonly IDownloadStreamManager m_DownloadStreamManager;
        private readonly IDownloadSettings m_Settings;

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

        public IEventManager EventManager
        {
            get { return m_EventManager; }
        }

        public void StartDownload()
        {
            INetworkProtocolProvider networkProtocolProvider = m_NetworkProtocolProviderFactory.CreateProvider(m_File.Url);

                RemoteFileInfo remoteFileInfo = networkProtocolProvider.GetRemoteFileInfo(m_File);

                int maximumSegmentCount = m_Settings.MaximumSegmentCount;
                ServicePointManager.DefaultConnectionLimit = maximumSegmentCount;

                DownloadSegmentPositions[] segmentPositionInfos = m_DownloadSegmentCalculator.Calculate(m_Settings.MinimumSegmentSize, maximumSegmentCount, m_File.SegmentCount, remoteFileInfo.FileSize);

                using (Stream stream = m_DownloadStreamManager.CreateStream(remoteFileInfo))
                {
                    SegmentWriter segmentWriter = new SegmentWriter(stream);
                    IList<ISegmentDownloadTask> segmentDownloadTasks = new List<ISegmentDownloadTask>(segmentPositionInfos.Length);
                    for (int i = 0; i < segmentPositionInfos.Length; i++)
                    {
                        DownloadSegmentPositions segmentPosition = segmentPositionInfos[i];         
                        segmentDownloadTasks.Add(new DoubleBufferSegmentDownloadTask(m_Settings.DownloadBufferSize, new SegmentDownloader(m_File, networkProtocolProvider, segmentPosition, new SegmentDownloadRateCalculator(segmentPosition.StartPosition)), segmentWriter));
                    }

                    SegmentDownloadManager segmentDownloadManager = new SegmentDownloadManager(segmentDownloadTasks);
                    segmentDownloadManager.Start();

                    m_EventManager.EventPublisher.Publish(new DownloadTaskFinishedEventArgs(stream, remoteFileInfo));
                }
            }
        }
    }
