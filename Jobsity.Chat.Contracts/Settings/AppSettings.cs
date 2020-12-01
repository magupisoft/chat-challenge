namespace Jobsity.Chat.Contracts.Settings
{
    public class AppSettings
    {
        public MessageBrokerSettings MessageBrokerSettings { get; set; }
        public RabbitMqQueueSettings RabbitMqQueueSettings { get; set; }
    }
}
