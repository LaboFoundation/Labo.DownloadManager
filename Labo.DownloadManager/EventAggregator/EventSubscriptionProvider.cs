using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Labo.DownloadManager.EventAggregator
{
    internal sealed class EventSubscriptionProvider : IEventSubscriptionProvider
    {
        private readonly ConcurrentDictionary<Type, IList<object>> m_Events = new ConcurrentDictionary<Type, IList<object>>();

        public IList<IEventConsumer<TEventMessage>> GetSubscriptions<TEventMessage>()
        {
            return m_Events.GetOrAdd(typeof(IEventConsumer<TEventMessage>), x => new List<object>()).Cast<IEventConsumer<TEventMessage>>().ToList();
        }

        public void RegisterConsumer<TEventMessage>(IEventConsumer<TEventMessage> consumer)
        {
            m_Events.AddOrUpdate(typeof(IEventConsumer<TEventMessage>), x => new List<object> { consumer }, (x, y) =>
                {
                    y.Add(consumer);
                    return y;
                });
        }
    }
}
