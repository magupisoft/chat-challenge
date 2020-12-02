namespace Jobsity.Chat.Contracts.Commands
{
    public class StockQuoteCommand
    {
        public string Symbol { get; set; }
        public decimal Price { get; set; }
    }
}
