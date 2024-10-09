using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Infrastructure.Persistence.Data;

namespace LinkDev.Talabat.Apis.Extensions
{
    public static class InitializerExtensions
    { 
        public static async Task<WebApplication> InitializeStoreContextAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateAsyncScope();
            var services = scope.ServiceProvider;
            var storeContextInitializer = services.GetRequiredService<IStoreContextInitializer>();
            // ask runtime enviroment for an object from "storeContext" service Explicity

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();



            try
            {
                await storeContextInitializer.InitializeAsync();

                await storeContextInitializer.SeedAsunc();

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error has been oucerd during applying the migrations or data seeding ");
            }
            return app;
        }

    }
}
