using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Jobsity.Chat.UI.Areas.Identity.IdentityHostingStartup))]
namespace Jobsity.Chat.UI.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}