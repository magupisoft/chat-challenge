namespace Jobsity.Chat.Contracts.Interfaces.MessageBroker
{
    public interface IQueueSettings
    {
        string RoutingKey { get; set; }
        string ExchangeName { get; set; }
        string QueueName { get; set; }
    }
}
