using System.Collections.Generic;

namespace Labo.DownloadManager.EventAggregator
{
    internal sealed class EventPublisher : IEventPublisher
    {
        private readonly IEventSubscriptionProvider m_SubscriptionProvider;

        public EventPublisher(IEventSubscriptionProvider subscriptionService)
        {
            m_SubscriptionProvider = subscriptionService;
        }

        public void Publish<TEventMessage>(TEventMessage eventMessage)
        {
            IList<IEventConsumer<TEventMessage>> subscriptions = m_SubscriptionProvider.GetSubscriptions<TEventMessage>();
            for (int i = 0; i < subscriptions.Count; i++)
            {
                IEventConsumer<TEventMessage> eventConsumer = subscriptions[i];
                PublishToConsumer(eventConsumer, eventMessage);
            }
        }

        private static void PublishToConsumer<TEventMessage>(IEventConsumer<TEventMessage> x, TEventMessage eventMessage)
        {
            x.HandleEvent(eventMessage);
        }
    }
}
