using LinkDev.Talabat.Core.Domain.Entities.Identity;
using LinkDev.Talabat.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity;

namespace LinkDev.Talabat.Apis.Extensions
{
    public static class IdentityExtensions
    {

        public static IServiceCollection AddIdentityServices(this IServiceCollection services) {

           services.AddIdentity<ApplicationUser, IdentityRole>(identityoptions =>
            {
                identityoptions.User.RequireUniqueEmail = true;

                identityoptions.SignIn.RequireConfirmedPhoneNumber = true;
                identityoptions.SignIn.RequireConfirmedEmail = true;
                identityoptions.SignIn.RequireConfirmedAccount = true;

                identityoptions.Password.RequireNonAlphanumeric = true;
                identityoptions.Password.RequiredUniqueChars = 2;
                identityoptions.Password.RequiredLength = 6;
                identityoptions.Password.RequireDigit = true;
                identityoptions.Password.RequireLowercase = true;
                identityoptions.Password.RequireUppercase = true;

                identityoptions.Lockout.AllowedForNewUsers = true;
                identityoptions.Lockout.MaxFailedAccessAttempts = 10;
                identityoptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);


            }).AddEntityFrameworkStores<StoreIdentityDbContext>();


            return services;
        }
    }
}
