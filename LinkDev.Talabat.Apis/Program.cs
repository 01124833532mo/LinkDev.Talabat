
using LinkDev.Talabat.Infrastructure.Persistence;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace LinkDev.Talabat.Apis
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region Configure Services
            var WebApplicationBuilder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            WebApplicationBuilder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            WebApplicationBuilder.Services.AddEndpointsApiExplorer();
            WebApplicationBuilder.Services.AddSwaggerGen();

            WebApplicationBuilder.Services.AddPersistenceServices(WebApplicationBuilder.Configuration);

            #endregion
                  var app = WebApplicationBuilder.Build();

            #region Update database and Data seeding

            using var scope = app.Services.CreateAsyncScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<StoreContext>();
            // ask runtime enviroment for an object from "storeContext" service Explicity

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();



            try
            {
                var pendingMigrations = dbContext.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                    await dbContext.Database.MigrateAsync();

                await StoreContextSeed.SeedAsync(dbContext);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "an error has been oucerd during applying the migrations or data seeding ");
            }

            #endregion




            #region Configure Kestrel Middlewares
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(); 
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();



            app.MapControllers();

            #endregion
            app.Run();
        }
    }
}
