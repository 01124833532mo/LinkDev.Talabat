﻿using LinkDev.Talabat.Core.Domain.Contracts;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceServices (this IServiceCollection services , IConfiguration configuration) {

          services.AddDbContext<StoreContext>((optionsBuilder) => {

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("StoreContext"));
            });

            services.AddScoped<IStoreContextInitializer, StoreContextInitializer>();
            services.AddScoped(typeof(IStoreContextInitializer),typeof(StoreContextInitializer));


            return services;
        }

    }
}