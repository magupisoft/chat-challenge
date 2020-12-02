using Jobsity.Chat.Contracts.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace Jobsity.Chat.IntegrationTests
{
    public sealed class ConfigurationSingletonFactory
    {
        private static readonly Lazy<IConfiguration> lazyConfiguration = new Lazy<IConfiguration>(() => GetConfiguration());

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            IConfiguration config = builder.Build();
            return config;
        }

        public static IConfiguration InstanceConfiguration { get { return lazyConfiguration.Value;  } }

        public static IOptions<MessageBrokerSettings> GetMessageBrokerSettings()
        {
            var config = ConfigurationSingletonFactory.InstanceConfiguration;
            var settings = new MessageBrokerSettings();
            config.GetSection("MessageBroker").Bind(settings);
            var settingsOptions = Options.Create(settings);
            return settingsOptions;
        }

        public static IOptions<StockMarketSettings> GetStockMarketSettings()
        {
            var config = ConfigurationSingletonFactory.InstanceConfiguration;
            var settings = new StockMarketSettings();
            config.GetSection("StockMarket").Bind(settings);
            var settingsOptions = Options.Create(settings);
            return settingsOptions;
        }
    }
}
