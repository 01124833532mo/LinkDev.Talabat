
using LinkDev.Talabat.Apis.Extensions;
using LinkDev.Talabat.Apis.Services;
using LinkDev.Talabat.Core.Application.Abstraction;
using LinkDev.Talabat.Infrastructure.Persistence;
using LinkDev.Talabat.Core.Application;
using static System.Net.Mime.MediaTypeNames;

namespace LinkDev.Talabat.Apis
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region Configure Services
            var WebApplicationBuilder = WebApplication.CreateBuilder(args);

            
            // Add services to the container.

            WebApplicationBuilder.Services.AddControllers().AddApplicationPart(typeof(Controllers.AssemblyInformation).Assembly);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            WebApplicationBuilder.Services.AddEndpointsApiExplorer();
            WebApplicationBuilder.Services.AddSwaggerGen();

           WebApplicationBuilder.Services.AddHttpContextAccessor();
            WebApplicationBuilder.Services.AddScoped(typeof(ILoggedInUserService), typeof(LoggedInUserService));

            WebApplicationBuilder.Services.AddPersistenceServices(WebApplicationBuilder.Configuration);
            WebApplicationBuilder.Services.AddApplicationServices();

            
            #endregion
                  var app = WebApplicationBuilder.Build();

            #region Databases initilization

       await app.InitializeStoreContextAsync();

            #endregion




            #region Configure Kestrel Middlewares
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(); 
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles(); ;

            app.MapControllers();

            #endregion
            app.Run();
        }
    }
}
