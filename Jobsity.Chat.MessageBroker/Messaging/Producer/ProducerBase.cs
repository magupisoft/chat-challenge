namespace Jobsity.Chat.MessageBroker.Messaging
{
    public abstract class ProducerBase<T> : IProducer<T>
        where T : class
    {
        public ProducerBase()
        {
        }

        #region IProducer<T>  members
        public void ProduceMessage(T message)
        {
            var serializedMessage = System.Text.Json.JsonSerializer.Serialize(message);

            this.ProduceMessage(serializedMessage);
        }
        #endregion
        protected abstract void ProduceMessage(string message);

        
    }
}
