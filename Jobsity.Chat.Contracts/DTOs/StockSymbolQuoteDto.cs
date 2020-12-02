using System;
using System.Collections.Generic;

namespace Jobsity.Chat.Contracts.DTOs
{
    public class StockSymbolQuoteDto
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public double Volume { get; set; }
    }

    public class StockSymbolQuoteRootDto
    {
        public List<StockSymbolQuoteDto> Symbols { get; set; }
    }
}
