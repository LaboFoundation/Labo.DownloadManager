namespace Labo.DownloadManager.EventArgs
{
    public sealed class DownloadTaskStateChangedEventMessage
    {
        /// <summary>
        /// Gets the download task state.
        /// </summary>
        /// <value>
        /// The download task state.
        /// </value>
        public DownloadTaskState State { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadTaskStateChangedEventMessage"/> class.
        /// </summary>
        /// <param name="state">The state.</param>
        public DownloadTaskStateChangedEventMessage(DownloadTaskState state)
        {
            State = state;
        }
    }
}
