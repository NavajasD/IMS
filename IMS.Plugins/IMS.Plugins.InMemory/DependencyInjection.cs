using IMS.UseCases.PluginInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.InMemory
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInMemoryRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IInventoryRepository, InventoryRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IInventoryTransactionRepository, InventoryTransactionRepository>();
            services.AddSingleton<IProductTransactionRepository, ProductTransactionRepository>();
            return services;
        }
    }
}
