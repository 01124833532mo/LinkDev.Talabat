using LinkDev.Talabat.Shared.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LinkDev.Talabat.Shared
{
    public static class DependancyInjection
    {
        public static IServiceCollection AddSharedDependances(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<mailSettings>(configuration.GetSection("MailSettings"));




            return services;

        }

    }
}
