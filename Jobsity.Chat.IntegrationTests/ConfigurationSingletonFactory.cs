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
            var messageBrokerSettings = new MessageBrokerSettings();
            config.GetSection("MessageBroker").Bind(messageBrokerSettings);
            var settingsOptions = Options.Create(messageBrokerSettings);
            return settingsOptions;
        }
    }
}
