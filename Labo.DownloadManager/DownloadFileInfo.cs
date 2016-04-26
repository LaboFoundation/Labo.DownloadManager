namespace Labo.DownloadManager
{
    using System;
    using System.IO;

    /// <summary>
    /// The download file info class.
    /// </summary>
    public sealed class DownloadFileInfo
    {
        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        /// <value>
        /// The URI.
        /// </value>
        public Uri Uri { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [authenticate].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [authenticate]; otherwise, <c>false</c>.
        /// </value>
        public bool Authenticate { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets the download location.
        /// </summary>
        /// <value>
        /// The download location.
        /// </value>
        public string DownloadLocation { get; private set; }

        /// <summary>
        /// Gets or sets the segment count.
        /// </summary>
        /// <value>
        /// The segment count.
        /// </value>
        public int SegmentCount { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadFileInfo"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="segmentCount">The segment count.</param>
        public DownloadFileInfo(Uri uri, string fileName, int segmentCount)
            : this(uri, fileName, segmentCount, false, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadFileInfo"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="segmentCount">The segment count.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public DownloadFileInfo(Uri uri, string fileName, int segmentCount, string userName, string password)
            : this(uri, fileName, segmentCount, true, userName, password)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadFileInfo"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="segmentCount">The segment count.</param>
        /// <param name="authenticate">if set to <c>true</c> [authenticate].</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public DownloadFileInfo(Uri uri, string fileName, int segmentCount, bool authenticate, string userName, string password)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            Uri = uri;
            FileName = fileName;
            SegmentCount = segmentCount;
            Authenticate = authenticate;
            UserName = userName;
            Password = password;

            DownloadLocation = Path.GetDirectoryName(fileName);
        }
    }
}
