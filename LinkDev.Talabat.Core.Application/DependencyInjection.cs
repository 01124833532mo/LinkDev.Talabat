using LinkDev.Talabat.Core.Application.Abstraction.Services;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Products;
using LinkDev.Talabat.Core.Application.Mapping;
using LinkDev.Talabat.Core.Application.Services;
using LinkDev.Talabat.Core.Application.Services.Products;
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

            //services.AddAutoMapper(mapper => mapper.AddProfile<MappingProfile>());

            return services;
        }

    }
}
