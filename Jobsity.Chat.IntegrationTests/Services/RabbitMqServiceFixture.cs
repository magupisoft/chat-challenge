using Jobsity.Chat.Contracts.Commands;
using Jobsity.Chat.Contracts.Interfaces;
using Jobsity.Chat.Contracts.Messaging;
using Jobsity.Chat.Service.MessageBrokerService;
using Xunit;
using Xunit.Abstractions;

namespace Jobsity.Chat.IntegrationTests.Services
{
    public class RabbitMqServiceFixture
    {
        ITestOutputHelper _testOutput;
        IRabbitMqService _rabbitMeService;

        public RabbitMqServiceFixture(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
            _rabbitMeService = new RabbitMqService(ConfigurationSingletonFactory.GetMessageBrokerSettings(), null);
        }

        [Theory]
        [InlineData("AAPL.US")]
        public void ShouldAskForQuoteData(string symbol)
        {
            // Arrange
            _rabbitMeService.OnQuoteDataReceived -= new NotifyCallerDelegate(OnQuoteReceived);
            _rabbitMeService.OnQuoteDataReceived += new NotifyCallerDelegate(OnQuoteReceived);

            // Act
            _rabbitMeService.AskForQuote(symbol);
        }

        protected void OnQuoteReceived(MessageEventArgs<StockQuoteCommand> args)
        {
            // Assert
            Assert.True(args.Message != null);
            Assert.True(args.Message.Price > 0m);
            _testOutput.WriteLine($"{args.Message.Symbol} quote is ${args.Message.Price} per share");
        }
    }
}
