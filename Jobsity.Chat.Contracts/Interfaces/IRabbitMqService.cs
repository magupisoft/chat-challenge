
using Jobsity.Chat.Contracts.Commands;
using Jobsity.Chat.Contracts.Messaging;

namespace Jobsity.Chat.Contracts.Interfaces
{
    public delegate void NotifyCallerDelegate(MessageEventArgs<StockQuoteCommand> cmd);

    public interface IRabbitMqService
    {
        event NotifyCallerDelegate OnQuoteDataReceived;

        void AskForQuote(string symbol);
    }
}
