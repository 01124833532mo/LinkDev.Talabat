
using LinkDev.Talabat.Apis.Extensions;
using LinkDev.Talabat.Apis.Services;
using LinkDev.Talabat.Core.Application.Abstraction;
using LinkDev.Talabat.Infrastructure.Persistence;
using LinkDev.Talabat.Core.Application;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Mvc;
using LinkDev.Talabat.Apis.Controllers.Errors;
using LinkDev.Talabat.Apis.Middlewares;

namespace LinkDev.Talabat.Apis
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region Configure Services
            var WebApplicationBuilder = WebApplication.CreateBuilder(args);


            // Add services to the container.

            WebApplicationBuilder.Services.AddControllers().ConfigureApiBehaviorOptions(options => {
                //options.SuppressModelStateInvalidFilter=true; // disapple for action

                options.SuppressModelStateInvalidFilter = false; // enable filter
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value!.Errors.Count() > 0)
                                                         .Select(p => new ApiValidationErrorResponse.ValidationError()
                                                         {
                                                             Field = p.Key,
                                                             Errors = p.Value!.Errors.Select(e => e.ErrorMessage)
                                                         });
                    return new BadRequestObjectResult(new ApiValidationErrorResponse()
                    {
                        Errors = errors

                    });
                };

            }).AddApplicationPart(typeof(Controllers.AssemblyInformation).Assembly);
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
            app.UseMiddleware<ExeptionHandlerMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(); 
                app.UseSwaggerUI();
            }

            
            app.UseHttpsRedirection();

            app.UseStatusCodePagesWithReExecute("/Errors/{0}");

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseStaticFiles(); ;

            app.MapControllers();

            #endregion
            app.Run();
        }
    }
}
