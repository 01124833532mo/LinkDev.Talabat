using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using LinkDev.Talabat.Infrastructure.Persistence.Data.Interceptors;
using LinkDev.Talabat.Infrastructure.Persistence.Identity;
using LinkDev.Talabat.Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore.Diagnostics;
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

            #region StroeContext
            services.AddDbContext<StoreDbContext>((optionsBuilder) =>
            {

                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("StoreContext"));
            });

            services.AddScoped(typeof(IStoreContextInitializer), typeof(StoreDbInitializer));

            services.AddScoped(typeof(ISaveChangesInterceptor), typeof(CustomSaveChangesInterceptor));
            #endregion

            #region IdentityContext

            services.AddDbContext<StoreIdentityDbContext>((optionsBuilder) =>
            {

                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("IdentityContext"));
            });

            #endregion


            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork.UnitOfWork));

            return services;
        }

    }
}
