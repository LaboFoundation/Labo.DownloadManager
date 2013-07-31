using System;

namespace Labo.DownloadManager.EventAggregator
{
    public sealed class ActionEventConsumer<TEventMessage> : IEventConsumer<TEventMessage>
    {
        private readonly Action<TEventMessage> m_Action;

        public ActionEventConsumer(Action<TEventMessage> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            m_Action = action;
        }

        public void HandleEvent(TEventMessage eventMessage)
        {
            m_Action(eventMessage);
        }
    }
}