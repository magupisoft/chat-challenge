using Jobsity.Chat.Contracts.Interfaces;
using Jobsity.Chat.Service.StockMarketService;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Jobsity.Chat.IntegrationTests.Services
{
    public class StockMarketServiceFixture
    {
        ITestOutputHelper _testOutput;
        IStockMarketService _stockMarketServiceCsv;
        IStockMarketService _stockMarketServiceJson;

        public StockMarketServiceFixture(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
            var settings = ConfigurationSingletonFactory.GetStockMarketSettings();
            _stockMarketServiceCsv = new StockMarketService(new System.Net.Http.HttpClient(), settings);

            settings.Value.Format = "json";
            _stockMarketServiceJson = new StockMarketService(new System.Net.Http.HttpClient(), settings);
        }

        [Theory]
        [InlineData("AAPL.US")]
        [InlineData("NIO.US")]
        public async Task ShouldAskForQuoteData_WhenFormIsCsv(string symbol)
        {
            // Arrange, Act
            var stock = await _stockMarketServiceCsv.GetQuoteAsync(symbol);

            // Assert
            Assert.NotNull(stock);
            Assert.Equal(stock.Symbol, symbol);
            Assert.True(stock.Close > 0);
            _testOutput.WriteLine($"{stock.Symbol} quote is ${stock.Close} per share");
        }

        [Theory]
        [InlineData("XXX.US")]
        [InlineData("")]
        public async Task ShouldReturnNullDataWhenStockSymbolsDoesNotExist_WhenFormatIsCsv(string symbol)
        {
            // Arrange, Act
            var stock = await _stockMarketServiceCsv.GetQuoteAsync(symbol);

            // Assert
            Assert.Null(stock);
        }

        [Theory]
        [InlineData("AAPL.US")]
        [InlineData("NIO.US")]
        public async Task ShouldAskForQuoteData_WhenFormIsJson(string symbol)
        {
            // Arrange, Act
            var stock = await _stockMarketServiceJson.GetQuoteAsync(symbol);

            // Assert
            Assert.NotNull(stock);
            Assert.Equal(stock.Symbol, symbol);
            Assert.True(stock.Close > 0);
            _testOutput.WriteLine($"{stock.Symbol} quote is ${stock.Close} per share");
        }

        [Theory]
        [InlineData("XXX.US")]
        [InlineData("")]
        public async Task ShouldReturnNullDataWhenStockSymbolsDoesNotExist_WhenFormatIsJson(string symbol)
        {
            // Arrange, Act
            var stock = await _stockMarketServiceJson.GetQuoteAsync(symbol);

            // Assert
            Assert.Null(stock);
        }

    }
}
