using LinkDev.Talabat.Core.Application.Abstraction.Models.Auth;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Auth;
using LinkDev.Talabat.Core.Application.Services.Auth;
using LinkDev.Talabat.Core.Domain.Entities.Identity;
using LinkDev.Talabat.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LinkDev.Talabat.Apis.Extensions
{
    public static class IdentityExtensions
    {

        public static IServiceCollection AddIdentityServices(this IServiceCollection services,IConfiguration configuration) {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));



           services.AddIdentity<ApplicationUser, IdentityRole>(identityoptions =>
            {

                identityoptions.User.RequireUniqueEmail = true;

                identityoptions.SignIn.RequireConfirmedPhoneNumber = true;
                identityoptions.SignIn.RequireConfirmedEmail = true;
                //identityoptions.SignIn.RequireConfirmedAccount = true;

                //identityoptions.Password.RequireNonAlphanumeric = true;
                //identityoptions.Password.RequiredUniqueChars = 2;
                //identityoptions.Password.RequiredLength = 6;
                //identityoptions.Password.RequireDigit = true;
                //identityoptions.Password.RequireLowercase = true;
                //identityoptions.Password.RequireUppercase = true;

                identityoptions.Lockout.AllowedForNewUsers = true;
                identityoptions.Lockout.MaxFailedAccessAttempts = 10;
                identityoptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);


            }).AddEntityFrameworkStores<StoreIdentityDbContext>();

            services.AddScoped(typeof(IAuthService), typeof(AuthService));

            services.AddScoped(typeof(Func<IAuthService>), (serviceprovider) =>
            {
               return ()=> serviceprovider.GetService<IAuthService>();
            });

            services.AddAuthentication((authenticationoptions) =>
            {
                authenticationoptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authenticationoptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer((configurationoption) =>
              {
                  configurationoption.TokenValidationParameters = new TokenValidationParameters()
                  {
                      ValidateAudience = true,
                      ValidateIssuer = true,
                      ValidateIssuerSigningKey = true,
                      ValidateLifetime =true,


                      ClockSkew=TimeSpan.FromMinutes(0),
                      ValidIssuer = configuration["JwtSettings:Issuer"],
                      ValidAudience = configuration["JwtSettings:Audience"],
                      IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!)),

                  };
              });
            return services;
        }
    }
}
