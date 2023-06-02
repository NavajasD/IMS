using IMS.UseCases.UseCases.Activities;
using IMS.UseCases.UseCases.Activities.Interfaces;
using IMS.UseCases.UseCases.Inventories;
using IMS.UseCases.UseCases.Inventories.Interfaces;
using IMS.UseCases.UseCases.Products;
using IMS.UseCases.UseCases.Products.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddTransient<IViewInventoriesByNameUseCase, ViewInventoriesByNameUseCase>();
            services.AddTransient<IAddInventoryUseCase, AddInventoryUseCase>();
            services.AddTransient<IEditInventoryUseCase, EditInventoryUseCase>();
            services.AddTransient<IViewInventoryByIdUseCase, ViewInventoryByIdUseCase>();

            services.AddTransient<IViewProductsByNameUseCase, ViewProductsByNameUseCase>();
            services.AddTransient<IAddProductUseCase, AddProductUseCase>();
            services.AddTransient<IEditProductUseCase, EditProductUseCase>();
            services.AddTransient<IViewProductByIdUseCase, ViewProductByIdUseCase>();

            services.AddTransient<IPurchaseInventoryUseCase, PurchaseInventoryUseCase>();
            services.AddTransient<IProduceProductUseCase, ProduceProductUseCase>();
            services.AddTransient<ISellProductUseCase, SellProductUseCase>();

            return services;
        }
    }
}
