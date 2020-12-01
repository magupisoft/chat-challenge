using Jobsity.Chat.Contracts.Interfaces;
using Jobsity.Chat.Repository.IdentityRepository;
using Jobsity.Chat.Service.IdentityService;
using Jobsity.Chat.Service.MessageBrokerService;
using Microsoft.Extensions.DependencyInjection;

namespace Jobsity.Chat.UI.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void InjectDependencies(this IServiceCollection services)
        {
            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddSingleton<IRabbitMqService, RabbitMqService>();
        }
    }
}
