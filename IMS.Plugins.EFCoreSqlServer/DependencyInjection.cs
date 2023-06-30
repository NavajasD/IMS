using IMS.Plugins.EFCoreSqlServer.Repositories;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.EFCoreSqlServer
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDatabaseContext(this IServiceCollection service, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SQLServer");
            service.AddDbContextFactory<IMSContext>(dbOption => dbOption.UseSqlServer(connectionString));
            return service;
        }
        public static IServiceCollection AddEfCoreRepositories(this IServiceCollection services)
        {
            services.AddTransient<IInventoryRepository, InventoryRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IInventoryTransactionRepository, InventoryTransactionRepository>();
            services.AddTransient<IProductTransactionRepository, ProductTransactionRepository>();
            return services;
        }
    }
}
