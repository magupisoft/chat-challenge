using System;

namespace Jobsity.Chat.Contracts.Messaging
{
    public class MessageEventArgs<T> : EventArgs
    {
        public T Message { get; set; }
    }
}
