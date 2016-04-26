namespace Labo.DownloadManager
{
    using System;

    public sealed class DownloadFileRequestInfo
    {
        private DownloadFileInfo m_File;

        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        /// <value>
        /// The URI.
        /// </value>
        public Uri Uri { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether [authenticate].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [authenticate]; otherwise, <c>false</c>.
        /// </value>
        public bool Authenticate { get; private set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadFileRequestInfo"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <exception cref="System.ArgumentNullException">
        /// uri
        /// or
        /// userName
        /// or
        /// password
        /// </exception>
        public DownloadFileRequestInfo(Uri uri, string userName, string password)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }

            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            Uri = uri;

            UserName = userName;
            Password = password;
            Authenticate = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadFileRequestInfo"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <exception cref="System.ArgumentNullException">uri</exception>
        public DownloadFileRequestInfo(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            Uri = uri;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadFileRequestInfo"/> class.
        /// </summary>
        /// <param name="fileInfo">The file info.</param>
        public DownloadFileRequestInfo(DownloadFileInfo fileInfo)
        {
            Uri = fileInfo.Uri;
            Authenticate = fileInfo.Authenticate;
            UserName = fileInfo.UserName;
            Password = fileInfo.Password;
        }
    }
}