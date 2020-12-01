using Jobsity.Chat.Contracts.Messaging;
using System;

namespace Jobsity.Chat.MessageBroker.Messaging
{
    public interface IConsumer<T>
    {
        void StartListeningMessages();
        event EventHandler<MessageEventArgs<T>> MessageReceived;
    }
}