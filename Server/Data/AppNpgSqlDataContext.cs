using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Tomi.Calendar.Mono.Server.Models;
using Tomi.Calendar.Mono.Shared;

namespace Tomi.Calendar.Mono.Server.Data
{
    public class AppNpgSqlDataContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<CalendarItem> CalendarItems { get; set; }

        public AppNpgSqlDataContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
    }
}
