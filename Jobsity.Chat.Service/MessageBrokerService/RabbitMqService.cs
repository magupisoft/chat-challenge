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
        private readonly IStockMarketService _stockMarketService;

        public event NotifyCallerDelegate OnQuoteDataReceived;

        public RabbitMqService(
            IOptions<MessageBrokerSettings> messageBrokerSettings, IStockMarketService stockMarketService)
        {
            this._messageBrokerManager = new RabbitMqMessageBrokerManager<StockQuoteCommand>(messageBrokerSettings);
            _stockMarketService = stockMarketService;

            Init();
        }

        public void AskForQuote(string symbol)
        {
            symbol = symbol.ToLowerInvariant().Trim();
            this._messageBrokerManager.AddProducer($"{AppConstants.ProducerRequestQuoteName}_{symbol}",
               new RabbitMqMessagingSettings
               {
                   ExchangeName = "ask.quote",
                   RoutingKey = "bot.chat.quotes"
               });

            this._messageBrokerManager.AddProducer($"{AppConstants.ProducerResultQuoteName}_{symbol}",
               new RabbitMqMessagingSettings
               {
                   ExchangeName = "resulting.quote",
                   RoutingKey = "bot.chat.quotes"
               });

            this._messageBrokerManager.AddConsumer($"{AppConstants.ConsumerRequestQuoteName}_{symbol}",
               new RabbitMqMessagingSettings
               {
                   RoutingKey = "bot.chat.quotes",
                   ExchangeName = "ask.quote",
                   QueueName = "quotes_queue"
               });

            this._messageBrokerManager.AddConsumer($"{AppConstants.ConsumerResultQuoteName}_{symbol}",
                new RabbitMqMessagingSettings
                {
                    RoutingKey = "bot.chat.quotes",
                    ExchangeName = "resulting.quote",
                    QueueName = "quotes_queue"
                });


            this._messageBrokerManager
              .Consumers[$"{AppConstants.ConsumerRequestQuoteName}_{symbol}"].MessageReceived -= OnRequestForSymbolQuoteReceived;
            this._messageBrokerManager
                .Consumers[$"{AppConstants.ConsumerRequestQuoteName}_{symbol}"].MessageReceived += OnRequestForSymbolQuoteReceived;

            this._messageBrokerManager
                .Consumers[$"{AppConstants.ConsumerResultQuoteName}_{symbol}"].MessageReceived -= OnResponseForSymbolQuoteReceived;
            this._messageBrokerManager
                .Consumers[$"{AppConstants.ConsumerResultQuoteName}_{symbol}"].MessageReceived += OnResponseForSymbolQuoteReceived;

            this._messageBrokerManager.Producers[$"{AppConstants.ProducerRequestQuoteName}_{symbol}"].ProduceMessage(new StockQuoteCommand() { Symbol = symbol });
        }

        private void Init()
        {
            this._messageBrokerManager.StartResilientConnection();
           
        }

        private void OnRequestForSymbolQuoteReceived(object sender, MessageEventArgs<StockQuoteCommand> args)
        {
            if (args.Message == null) return;

            var quote = _stockMarketService.GetQuoteAsync(args.Message.Symbol).GetAwaiter().GetResult();
            if(quote == null)
            {
                this._messageBrokerManager.Producers[$"{AppConstants.ProducerResultQuoteName}_{args.Message.Symbol.ToLowerInvariant()}"].ProduceMessage(new StockQuoteCommand() { Symbol = "error", Price = -1 });
                return;
            }

            this._messageBrokerManager.Producers[$"{AppConstants.ProducerResultQuoteName}_{args.Message.Symbol.ToLowerInvariant()}"].ProduceMessage(new StockQuoteCommand() { Symbol = quote.Symbol, Price = quote.Close });
        }

        private void OnResponseForSymbolQuoteReceived(object sender, MessageEventArgs<StockQuoteCommand> args)
        {
            if (args.Message == null) return;

            OnQuoteDataReceived(args);
        }
    }
}
