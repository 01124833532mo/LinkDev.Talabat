
using Hangfire;
using LinkDev.Talabat.Apis.Controllers.Errors;
using LinkDev.Talabat.Apis.Extensions;
using LinkDev.Talabat.Apis.Middlewares;
using LinkDev.Talabat.Apis.Services;
using LinkDev.Talabat.Core.Application;
using LinkDev.Talabat.Core.Application.Abstraction;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Emails;
using LinkDev.Talabat.Infrastructure;
using LinkDev.Talabat.Infrastructure.Persistence;
using LinkDev.Talabat.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace LinkDev.Talabat.Apis
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region Configure Services
            var WebApplicationBuilder = WebApplication.CreateBuilder(args);


            // Add services to the container.

            WebApplicationBuilder.Services.AddControllers().AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            }).ConfigureApiBehaviorOptions(options =>
            {
                //options.SuppressModelStateInvalidFilter=true; // disapple for action

                options.SuppressModelStateInvalidFilter = false; // enable filter
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var Errors = actionContext.ModelState.Where(p => p.Value!.Errors.Count > 0).SelectMany(p => p.Value!.Errors).Select(p => p.ErrorMessage);
                    return new BadRequestObjectResult(new ApiValidationErrorResponse() { Errors = Errors });
                };

            }).AddApplicationPart(typeof(Controllers.AssemblyInformation).Assembly);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            WebApplicationBuilder.Services.AddEndpointsApiExplorer();
            WebApplicationBuilder.Services.AddSwaggerGen();

            WebApplicationBuilder.Services.AddHttpContextAccessor();
            WebApplicationBuilder.Services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("TalabatPolicy", policyBuilder =>
                {
                    policyBuilder.AllowAnyHeader().AllowAnyMethod().WithOrigins(WebApplicationBuilder.Configuration["Urls:FrontBaseUrl"]!);
                });
            });


            WebApplicationBuilder.Services.AddScoped(typeof(ILoggedInUserService), typeof(LoggedInUserService));

            WebApplicationBuilder.Services.AddPersistenceServices(WebApplicationBuilder.Configuration);
            WebApplicationBuilder.Services.AddApplicationServices();

            WebApplicationBuilder.Services.AddInfrastructureServices(WebApplicationBuilder.Configuration);
            WebApplicationBuilder.Services.AddIdentityServices(WebApplicationBuilder.Configuration);
            WebApplicationBuilder.Services.AddSharedDependances(WebApplicationBuilder.Configuration);

            #endregion
            var app = WebApplicationBuilder.Build();

            #region Databases initilization

            await app.InitializeDbAsync();

            #endregion


            app.UseHangfireDashboard();
            RecurringJob.AddOrUpdate<IEmailNotificationService>(
           "monthly-email-job", // 
             service => service.SendMonthlyEmails(),
            Cron.Monthly(1),
                new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.Utc
                });





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




            app.UseStaticFiles();

            app.UseCors("TalabatPolicy");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            #endregion
            app.Run();
        }
    }
}
