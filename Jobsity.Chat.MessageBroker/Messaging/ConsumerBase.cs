using System;

namespace Jobsity.Chat.MessageBroker.Messaging
{
    public abstract class ConsumerBase<T> : IConsumer<T>
        where T : class
    {
        public ConsumerBase()
        {
        }

        #region IConsumer<T> members
        public event EventHandler<MessageEventArgs<T>> MessageReceived;
        public abstract void StartListeningMessages();
        #endregion
        protected virtual void OnMessageReceived(MessageEventArgs<T> args)
        {
            MessageReceived?.Invoke(this, args);
        }
    }
}
