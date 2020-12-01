using IdentityServer4.EntityFramework.Options;
using Jobsity.Chat.DataContext.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Jobsity.Chat.DataContext.IdentityData
{
    public class IdentityDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public IdentityDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
    }
}
