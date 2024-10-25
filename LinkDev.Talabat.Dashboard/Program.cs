using LinkDev.Talabat.Core.Domain.Entities.Identity;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using LinkDev.Talabat.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LinkDev.Talabat.Dashboard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
           builder. Services.AddDbContext<StoreDbContext>((optionsBuilder) =>
            {

                optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("StoreContext"));
            });


           builder. Services.AddDbContext<StoreIdentityDbContext>((optionsBuilder) =>
            {

                optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("IdentityContext"));
            });


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(identityoptions =>
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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Admin}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
