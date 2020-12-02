namespace Jobsity.Chat.Contracts.Constants
{
    public static class AppConstants
    {
        public const string MessageBrokerSettingsSection = "MessageBroker";
        public const string StockMarketSettingsSection = "StockMarket";

        public const string ConsumerRequestQuoteName = "Consumer.Chat.Symbol.Quote.Request";
        public const string ConsumerResultQuoteName = "Consumer.Chat.Symbol.Quote.Results";
        public const string ProducerRequestQuoteName = "Producer.Chat.Symbol.Quote.Request";
        public const string ProducerResultQuoteName = "Producer.Chat.Symbol.Quote.Result";
    }
}
