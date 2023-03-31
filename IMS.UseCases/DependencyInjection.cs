﻿using IMS.UseCases.Inventories;
using IMS.UseCases.Inventories.Interfaces;
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
            return services;
        }
    }
}
