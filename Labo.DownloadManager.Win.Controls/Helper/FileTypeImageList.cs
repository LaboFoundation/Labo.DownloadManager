namespace Labo.DownloadManager.Win.Controls.Helper
{
    using System;
    using System.Collections.Concurrent;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;

    /// <summary>
    /// The file type image list class.
    /// </summary>
    public sealed class FileTypeImageList
    {
        /// <summary>
        /// The open folder key
        /// </summary>
        private const string OPEN_FOLDER_KEY = "OpenFolderKey";

        /// <summary>
        /// The close folder key
        /// </summary>
        private const string CLOSE_FOLDER_KEY = "OpenFolderKey";

        /// <summary>
        /// The lock object
        /// </summary>
        private static readonly object s_LockObject = new object();

        /// <summary>
        /// The file type icons dictionary
        /// </summary>
        private static readonly ConcurrentDictionary<string, Icon> s_FileTypeIconsDictionary = new ConcurrentDictionary<string, Icon>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The image list
        /// </summary>
        private static ImageList s_ImageList;

        /// <summary>
        /// Gets the image list.
        /// </summary>
        /// <returns>The image list.</returns>
        public static ImageList GetImageList()
        {
            if (s_ImageList == null)
            {
                lock (s_LockObject)
                {
                    s_ImageList = new ImageList();
                    s_ImageList.TransparentColor = Color.Black;
                    s_ImageList.TransparentColor = Color.Transparent;
                    s_ImageList.ColorDepth = ColorDepth.Depth32Bit;
                    s_ImageList.ImageSize = new Size(16, 16);
                }
            }

            return s_ImageList;
        }

        /// <summary>
        /// Gets the file type icon.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <param name="iconSize">Size of the icon.</param>
        /// <returns>The icon.</returns>
        public static Icon GetFileTypeIcon(string extension, IconReader.EnumIconSize iconSize)
        {
            return s_FileTypeIconsDictionary.GetOrAdd(string.Format(CultureInfo.InvariantCulture, "{0}-{1}", extension, iconSize), s => IconReader.GetFileIconByExt(extension, iconSize, false));
        }

        /// <summary>
        /// Gets the image index by extension.
        /// </summary>
        /// <param name="ext">The ext.</param>
        /// <returns>The image index.</returns>
        public static int GetImageIndexByExtension(string ext)
        {
            ImageList imageList = GetImageList();

            ext = ext.ToLowerInvariant();

            if (!imageList.Images.ContainsKey(ext))
            {
                Icon iconForFile = IconReader.GetFileIconByExt(ext, IconReader.EnumIconSize.Small, false);

                imageList.Images.Add(ext, iconForFile);
            }

            return imageList.Images.IndexOfKey(ext);
        }

        /// <summary>
        /// Gets the image index from folder.
        /// </summary>
        /// <param name="open">if set to <c>true</c> [open].</param>
        /// <returns>The image index.</returns>
        public static int GetImageIndexFromFolder(bool open)
        {
            ImageList imageList = GetImageList();

            string key = open ? OPEN_FOLDER_KEY : CLOSE_FOLDER_KEY;

            if (!imageList.Images.ContainsKey(key))
            {
                Icon iconForFile = IconReader.GetFolderIcon(IconReader.EnumIconSize.Small, open ? IconReader.EnumFolderType.Open : IconReader.EnumFolderType.Closed);

                imageList.Images.Add(key, iconForFile);
            }

            return imageList.Images.IndexOfKey(key);
        }
    }
}