namespace Jobsity.Chat.MessageBroker.Messaging
{
    public interface IProducer<T> where T : class
    {
        void ProduceMessage(T message);
    }
}