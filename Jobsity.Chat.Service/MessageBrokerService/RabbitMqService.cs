using Jobsity.Chat.Contracts.Commands;
using Jobsity.Chat.Contracts.Constants;
using Jobsity.Chat.Contracts.Interfaces;
using Jobsity.Chat.Contracts.Messaging;
using Jobsity.Chat.Contracts.Settings;
using Jobsity.Chat.MessageBroker;
using Jobsity.Chat.MessageBroker.Messaging;
using Microsoft.Extensions.Options;

namespace Jobsity.Chat.Service.MessageBrokerService
{
    public class RabbitMqService: IRabbitMqService
    {
        private readonly IMessageBrokerManager<StockQuoteCommand> _messageBrokerManager;

        public event NotifyCallerDelegate OnQuoteDataReceived;

        public RabbitMqService(
            IOptions<MessageBrokerSettings> messageBrokerSettings)
        {
            this._messageBrokerManager = new RabbitMqMessageBrokerManager<StockQuoteCommand>(messageBrokerSettings);
            Init();
        }

        public void AskForQuote(string symbol)
        {
            this._messageBrokerManager.AddProducer(AppConstants.ProducerRequestQuoteName,
               new RabbitMqMessagingSettings
               {
                   ExchangeName = "ask.quote",
                   RoutingKey = "bot.chat.quotes"
               });

            this._messageBrokerManager.AddProducer(AppConstants.ProducerResultQuoteName,
               new RabbitMqMessagingSettings
               {
                   ExchangeName = "resulting.quote",
                   RoutingKey = "bot.chat.quotes"
               });

            this._messageBrokerManager.AddConsumer(AppConstants.ConsumerRequestQuoteName,
               new RabbitMqMessagingSettings
               {
                   RoutingKey = "bot.chat.quotes",
                   ExchangeName = "ask.quote",
                   QueueName = "quotes_queue"
               });

            this._messageBrokerManager.AddConsumer(AppConstants.ConsumerResultQuoteName,
                new RabbitMqMessagingSettings
                {
                    RoutingKey = "bot.chat.quotes",
                    ExchangeName = "resulting.quote",
                    QueueName = "quotes_queue"
                });

           
            this._messageBrokerManager
                .Consumers[AppConstants.ConsumerRequestQuoteName].MessageReceived += OnRequestForSymbolQuoteReceived;

            this._messageBrokerManager
                .Consumers[AppConstants.ConsumerResultQuoteName].MessageReceived += OnResponseForSymbolQuoteReceived;

            this._messageBrokerManager.Producers[AppConstants.ProducerRequestQuoteName].ProduceMessage(new StockQuoteCommand() { Symbol = symbol });
        }

        private void Init()
        {
            this._messageBrokerManager.StartResilientConnection();
           
        }

        private void OnRequestForSymbolQuoteReceived(object sender, MessageEventArgs<StockQuoteCommand> args)
        {
            if (args.Message == null) return;
            this._messageBrokerManager.Producers[AppConstants.ProducerResultQuoteName].ProduceMessage(new StockQuoteCommand() { Symbol = args.Message.Symbol, Price = 999 });
        }

        private void OnResponseForSymbolQuoteReceived(object sender, MessageEventArgs<StockQuoteCommand> args)
        {
            if (args.Message == null) return;

            OnQuoteDataReceived(args);
        }
    }
}
