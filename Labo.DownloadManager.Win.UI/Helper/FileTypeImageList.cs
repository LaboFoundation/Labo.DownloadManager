namespace Labo.DownloadManager.Win.UI.Helper
{
    using System.Drawing;
    using System.Windows.Forms;

    public sealed class FileTypeImageList
    {
        private const string OPEN_FOLDER_KEY = "OpenFolderKey";
        private const string CLOSE_FOLDER_KEY = "OpenFolderKey";

        private static ImageList m_ImageList;

        public static ImageList GetSharedInstance()
        {
            if (m_ImageList == null)
            {
                m_ImageList = new ImageList();
                m_ImageList.TransparentColor = Color.Black;
                m_ImageList.TransparentColor = Color.Transparent;
                m_ImageList.ColorDepth = ColorDepth.Depth32Bit;
                m_ImageList.ImageSize = new Size(16, 16);
            }

            return m_ImageList;
        }

        public static int GetImageIndexByExtention(string ext)
        {
            GetSharedInstance();

            ext = ext.ToLower();

            if (!m_ImageList.Images.ContainsKey(ext))
            {
                Icon iconForFile = IconReader.GetFileIconByExt(ext, IconReader.EnumIconSize.Small, false);

                m_ImageList.Images.Add(ext, iconForFile);
            }

            return m_ImageList.Images.IndexOfKey(ext);
        }

        public static int GetImageIndexFromFolder(bool open)
        {
            string key;

            GetSharedInstance();

            key = open ? OPEN_FOLDER_KEY : CLOSE_FOLDER_KEY;

            if (!m_ImageList.Images.ContainsKey(key))
            {
                Icon iconForFile = IconReader.GetFolderIcon(IconReader.EnumIconSize.Small, (open ? IconReader.EnumFolderType.Open : IconReader.EnumFolderType.Closed));

                m_ImageList.Images.Add(key, iconForFile);
            }

            return m_ImageList.Images.IndexOfKey(key);
        }
    }
}