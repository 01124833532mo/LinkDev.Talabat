using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.Common.Contract.Infrastructure;
using LinkDev.Talabat.Core.Application.Abstraction.Services;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Orders;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Products;
using LinkDev.Talabat.Core.Application.Mapping;
using LinkDev.Talabat.Core.Application.Services;
using LinkDev.Talabat.Core.Application.Services.Basket;
using LinkDev.Talabat.Core.Application.Services.Orders;
using LinkDev.Talabat.Core.Application.Services.Products;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(mapper => mapper.AddProfile(new MappingProfile()));

            services.AddAutoMapper(typeof(MappingProfile));
            //services.AddScoped(typeof(IProductService), typeof(ProductService));
            services.AddScoped(typeof(IServiceManager), typeof(ServiceManager));

			//services.AddScoped(typeof(Func<IBasketService>), typeof(Func<BasketService>));
			services.AddScoped(typeof(IBasketService), typeof(BasketService));

            #region Get Requerd Services
            //services.AddScoped(typeof(Func<IBasketService>), (serverprovider) =>
            //         {
            //             //var mapper = serverprovider.GetRequiredService<IMapper>();
            //             //var cinfiguration = serverprovider.GetRequiredService<IConfiguration>();
            //             //var basketRepository = serverprovider.GetRequiredService<IBasketRepository>();


            //             //return () => new BasketService(basketRepository, mapper, cinfiguration);

            //             return ()=> serverprovider.GetRequiredService<IBasketService>();

            //         }); 
            #endregion

            services.AddAutoMapper(mapper => mapper.AddProfile<MappingProfile>());


            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped(typeof(Func< IOrderService>), (serviceprovider) =>
            {
                return () => serviceprovider.GetRequiredService<IOrderService>();
            });

            return services;
        }

    }
}
