using System;

namespace Jobsity.Chat.MessageBroker.Messaging
{
    public class MessageEventArgs<T> : EventArgs
    {
        public T Message { get; set; }
    }
}
