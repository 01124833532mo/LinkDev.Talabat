
using LinkDev.Talabat.Infrastructure.Persistence;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace LinkDev.Talabat.Apis
{
    public class Program
    {
        public static void Main(string[] args)
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
