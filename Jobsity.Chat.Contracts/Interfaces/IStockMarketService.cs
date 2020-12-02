using Jobsity.Chat.Contracts.DTOs;
using System.Threading.Tasks;

namespace Jobsity.Chat.Contracts.Interfaces
{
    public interface IStockMarketService
    {
        Task<StockSymbolQuoteDto> GetQuoteAsync(string symbol);
    }
}