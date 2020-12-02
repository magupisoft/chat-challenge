using Jobsity.Chat.Contracts.Interfaces.MessageBroker;
using RabbitMQ.Client;

namespace Jobsity.Chat.MessageBroker.Messaging
{
    public interface IRabbitMqMessaging
    {
        IModel Channel { get; }
        string ExchangeType { get; set; }
        IMessagingSettings MessagingSettings { get; set; }
    }
}
