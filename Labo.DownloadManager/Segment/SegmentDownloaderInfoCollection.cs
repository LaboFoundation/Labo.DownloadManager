namespace Labo.DownloadManager.Segment
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public sealed class SegmentDownloaderInfoCollection : List<ISegmentDownloaderInfo>
    {
        public SegmentDownloaderInfoCollection()
            : base()
        {
        }

        public SegmentDownloaderInfoCollection(IEnumerable<ISegmentDownloaderInfo> collection)
            : base(collection)
        {
        }

        public SegmentDownloaderInfoCollection(int capacity)
            : base(capacity)
        {
        }
    }
}