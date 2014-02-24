namespace Labo.DownloadManager.Segment
{
    using System;

    /// <summary>
    /// The segment downloader interface
    /// </summary>
    public interface ISegmentDownloader : ISegmentDownloaderInfo
    {
        /// <summary>
        /// Reads a sequence of bytes from the current stream and advances 
        /// the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">
        /// An array of bytes that contains the specified byte array with the values which will be replaced 
        /// by the bytes read from the current source when the method returns.
        /// </param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if 
        /// that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        int Download(byte[] buffer);

        /// <summary>
        /// Increases the current position of the segment downloader.
        /// </summary>
        /// <param name="size">The increase size.</param>
        void IncreaseCurrentPosition(int size);

        /// <summary>
        /// Sets the error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        void SetError(Exception exception);
    }
}