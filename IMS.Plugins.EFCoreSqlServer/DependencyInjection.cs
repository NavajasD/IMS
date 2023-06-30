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
            service.AddDbContext<IMSContext>(dbOption => dbOption.UseSqlServer(connectionString));
            return service;
        }
        public static IServiceCollection AddEfCoreRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IInventoryRepository, InventoryRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IInventoryTransactionRepository, InventoryTransactionRepository>();
            services.AddSingleton<IProductTransactionRepository, ProductTransactionRepository>();
            return services;
        }
    }
}
