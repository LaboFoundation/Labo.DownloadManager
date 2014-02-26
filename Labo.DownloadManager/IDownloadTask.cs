namespace Labo.DownloadManager
{
    /// <summary>
    /// The download task interface.
    /// </summary>
    public interface IDownloadTask
    {
        /// <summary>
        /// Starts the download task.
        /// </summary>
        void StartDownload();

        /// <summary>
        /// Determines whether download task is working.
        /// </summary>
        /// <returns>
        /// <c>true</c> if [is download task is working]; otherwise, <c>false</c>.
        /// </returns>
        bool IsWorking();

        /// <summary>
        /// Changes the state of the download task.
        /// </summary>
        /// <param name="downloadTaskState">State of the download task.</param>
        void ChangeState(DownloadTaskState downloadTaskState);

        /// <summary>
        /// Gets the state of the download task.
        /// </summary>
        /// <value>
        /// The download task state.
        /// </value>
        DownloadTaskState State { get; }

        /// <summary>
        /// Gets the download task statistics.
        /// </summary>
        /// <returns>The download statistics.</returns>
        DownloadTaskStatistics GetDownloadTaskStatistics();
    }
}