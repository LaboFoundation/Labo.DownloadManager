using System.Collections.Generic;
using System.Linq;

namespace Labo.DownloadManager.Tests.FileServer
{
    public sealed class LimittedCollection<TItem>
    {
        private readonly Stack<TItem> m_Items;
        private readonly int m_Capacity;

        public LimittedCollection(int capacity)
        {
            m_Capacity = capacity;
            m_Items = new Stack<TItem>(m_Capacity);
        }

        public void Add(TItem item)
        {
            if (m_Items.Count == m_Capacity)
            {
                m_Items.Pop();
            }
            m_Items.Push(item);
        }

        public IList<TItem> Items
        {
            get { return m_Items.ToList().AsReadOnly(); }
        }
    }
}
