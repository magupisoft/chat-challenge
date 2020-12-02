using Jobsity.Chat.Contracts.Interfaces.MessageBroker;
using Jobsity.Chat.MessageBroker.Messaging;
using System.Collections.Generic;

namespace Jobsity.Chat.MessageBroker
{
    public abstract class MessageBrokerManagerBase<T> : IMessageBrokerManager<T>
         where T : class
    {
        public MessageBrokerManagerBase()
        {
            this.Consumers = new Dictionary<string, IConsumer<T>>();
            this.Producers = new Dictionary<string, IProducer<T>>();
        }

        #region abstract methods
        protected abstract IProducer<T> CreateProducer(IMessagingSettings queueSettings);
        protected abstract IConsumer<T> CreateConsumer(IMessagingSettings queueSettings);
        #endregion

        #region IMessageBrokerManager<T> members
        public Dictionary<string, IConsumer<T>> Consumers { get; protected set; }
        public Dictionary<string, IProducer<T>> Producers { get; protected set; }

        public void AddConsumer(string name, IMessagingSettings queueSettings)
        {
            if (!Consumers.ContainsKey(name))
            {
                var consumer = this.CreateConsumer(queueSettings);
                Consumers.Add(name, consumer);
            }

            Consumers[name].StartListeningMessages();
        }
        public void AddProducer(string name, IMessagingSettings queueSettings)
        {
            if (!Producers.ContainsKey(name))
            {
                var producer = this.CreateProducer(queueSettings);            
                Producers.Add(name, producer);
            }
        }

        public abstract void StartResilientConnection();

        public abstract void Dispose();

        #endregion

       
    }
}
