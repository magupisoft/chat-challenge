using Jobsity.Chat.Contracts.Interfaces.MessageBroker;
using System;
using System.Collections.Generic;

namespace Jobsity.Chat.MessageBroker.Messaging
{
    public interface IMessageBrokerManager<T> : IDisposable
         where T : class
    {
        Dictionary<string, IConsumer<T>> Consumers { get; }
        Dictionary<string, IProducer<T>> Producers { get; }
        void StartResilientConnection();
        void AddProducer(string producerName, IMessagingSettings queueSettings);
        void AddConsumer(string consumerName, IMessagingSettings queueSettings);
    }
}
