using Jobsity.Chat.Contracts.Interfaces;
using Jobsity.Chat.Repository.ChatConversationRepository;
using Jobsity.Chat.Repository.IdentityRepository;
using Jobsity.Chat.Service.ConversationService;
using Jobsity.Chat.Service.IdentityService;
using Jobsity.Chat.Service.MessageBrokerService;
using Jobsity.Chat.Service.StockMarketService;
using Microsoft.Extensions.DependencyInjection;

namespace Jobsity.Chat.UI.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void InjectDependencies(this IServiceCollection services)
        {
            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IChatConversationRepository, ChatConversationRepository>();
            services.AddScoped<IConversationService, ConversationService>();
            services.AddSingleton<IRabbitMqService, RabbitMqService>();
            services.AddHttpClient<IStockMarketService, StockMarketService>();            
        }
    }
}
