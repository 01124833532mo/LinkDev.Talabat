using LinkDev.Talabat.Core.Application.Abstraction.Common.Contract.Infrastructure;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using LinkDev.Talabat.Infrastructure.Basket_Repository;
using LinkDev.Talabat.Infrastructure.Payment_Service;
using LinkDev.Talabat.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration) {

            services.Configure<RedisSettings>(configuration.GetSection("RedisSetting"));
			services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));


			services.AddSingleton(typeof(IConnectionMultiplexer), (serviceProvider) => {
                var connectionString = configuration.GetConnectionString("Redis");

                var connectionMultiplexerObj = ConnectionMultiplexer.Connect(connectionString!);

                return connectionMultiplexerObj;
            
            });
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));

            return services;
        
        }
    }
}
