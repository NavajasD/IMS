using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.WebApp.Identity
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityDatabaseContext(this IServiceCollection service, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SQLServer");
            service.AddDbContext<AccountDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            return service;
        }

        public static IServiceCollection AddIdentity(this IServiceCollection service)
        {
            service.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
            }).AddEntityFrameworkStores<AccountDbContext>();
            return service;
        }
    }
}
