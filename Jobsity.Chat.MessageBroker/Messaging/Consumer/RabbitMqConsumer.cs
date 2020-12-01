using Jobsity.Chat.Contracts.Interfaces.MessageBroker;
using Jobsity.Chat.Contracts.Messaging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Jobsity.Chat.MessageBroker.Messaging
{
    public class RabbitMqConsumer<T> : ConsumerBase<T>, IRabbitMqMessaging
       where T : class
    {
        public RabbitMqConsumer(IModel channel)
           : base()
        {
            this.Channel = channel;
            RabbitMqMessageReceived = OnRabbitMqMessageReceived;
        }

        #region IRabbitMqMessaging members
        public IModel Channel { get; private set; }
        public string ExchangeType { get; set; }
        public IMessagingSettings MessagingSettings { get; set; }
        #endregion

        #region RabbitMQ events
        private event EventHandler<BasicDeliverEventArgs> RabbitMqMessageReceived;
        private void OnRabbitMqMessageReceived(object sender, BasicDeliverEventArgs args)
        {
            byte[] body = args.Body.ToArray();
            string serializedMessage = Encoding.UTF8.GetString(body);

            var message = System.Text.Json.JsonSerializer.Deserialize<T>(serializedMessage);

            OnMessageReceived(new MessageEventArgs<T> { Message = message });
        }
        #endregion       

        protected override void OnMessageReceived(MessageEventArgs<T> args)
        {
            base.OnMessageReceived(args);
        }

        public override void StartListeningMessages()
        {
            this.Channel.ExchangeDeclare(exchange: this.MessagingSettings.ExchangeName, type: this.ExchangeType);
            this.Channel.QueueDeclare(this.MessagingSettings.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false);
            this.Channel.QueueBind(queue: this.MessagingSettings.QueueName,
                exchange: MessagingSettings.ExchangeName,
                routingKey: MessagingSettings.RoutingKey);

            var consumer = new EventingBasicConsumer(this.Channel);

            consumer.Received += RabbitMqMessageReceived;

            this.Channel.BasicConsume(queue: MessagingSettings.QueueName,
                autoAck: true,
                consumer: consumer);
        }
    }
}
