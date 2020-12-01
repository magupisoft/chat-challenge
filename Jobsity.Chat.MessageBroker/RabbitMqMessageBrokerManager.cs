using Jobsity.Chat.Contracts.Interfaces;
using Jobsity.Chat.Contracts.Interfaces.MessageBroker;
using Jobsity.Chat.MessageBroker.Messaging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Threading;

namespace Jobsity.Chat.MessageBroker
{
    public class RabbitMqMessageBrokerManager<T> : MessageBrokerManagerBase<T>
        where T : class
    {
        protected readonly ConnectionFactory connectionFactory;
        protected IModel channel;
        protected IConnection connection;

        public RabbitMqMessageBrokerManager(IOptions<IMessageBrokerSettings> settings) : base()
        {
            connectionFactory = new ConnectionFactory
            {
                HostName = settings?.Value.HostName,
                UserName = settings?.Value.Username,
                Password = settings?.Value.Password,
                AutomaticRecoveryEnabled = true,
            };
            if (settings.Value.RecoveryInterval != null && settings.Value.RecoveryInterval.TotalSeconds > 0)
                connectionFactory.NetworkRecoveryInterval = settings.Value.RecoveryInterval;

        }

        protected void CreateConnectionAndChannel()
        {
            connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
        }

        #region MessageBrokerManagerBase<T> overriden members
        protected override IProducer<T> CreateProducer(IQueueSettings queueSettings)
        {
            return new RabbitMqProducer<T>(channel)
            {
                ExchangeType = ExchangeType.Topic,
                QueueSettings = queueSettings
            };
        }
        protected override IConsumer<T> CreateConsumer(IQueueSettings queueSettings)
        {
            return new RabbitMqConsumer<T>(channel)
            {
                ExchangeType = ExchangeType.Topic,
                QueueSettings = queueSettings
            };
        }
        public override void StartResilientConnection()
        {
            var semaphore = new ManualResetEventSlim(false); // state is initially false

            while (!semaphore.Wait(3000)) // loop until state is true, checking every 3s
            {
                try
                {
                    CreateConnectionAndChannel();
                    semaphore.Set(); // state set to true - breaks out of loop
                }
                catch
                {
                    Thread.Sleep(3000);
                }
            }
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                channel.Close();
                connection.Close();
                channel?.Dispose();
                connection?.Dispose();
            }
        }
    }
}
