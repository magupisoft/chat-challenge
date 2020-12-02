using Jobsity.Chat.Contracts.Interfaces.MessageBroker;
using RabbitMQ.Client;
using System.Text;

namespace Jobsity.Chat.MessageBroker.Messaging
{
    public class RabbitMqProducer<T> : ProducerBase<T>, IRabbitMqMessaging
         where T : class
    {
        public RabbitMqProducer(IModel channel)
            : base()
        {
            this.Channel = channel;
        }

        #region IRabbitMqMessaging members
        public IModel Channel { get; private set; }
        public string ExchangeType { get; set; }
        public IMessagingSettings MessagingSettings { get; set; }
        #endregion

        #region ProducerBase<T> overriden members
        protected override void ProduceMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            this.Channel.ExchangeDeclare(exchange: this.MessagingSettings.ExchangeName,
                    type: this.ExchangeType);
            this.Channel.BasicPublish(exchange: this.MessagingSettings.ExchangeName,
                routingKey: this.MessagingSettings.RoutingKey,
                basicProperties: null,
                body: body);
        }
        #endregion

        
    }
}
