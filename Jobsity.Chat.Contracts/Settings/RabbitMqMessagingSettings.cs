using Jobsity.Chat.Contracts.Interfaces.MessageBroker;

namespace Jobsity.Chat.Contracts.Settings
{
    public class RabbitMqMessagingSettings : IMessagingSettings
    {
        public string RoutingKey { get; set; }
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }
    }
}
