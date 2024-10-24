using LinkDev.Talabat.Core.Domain.Contracts.Persistence.DbInitializers;
using LinkDev.Talabat.Core.Domain.Entities.Identity;
using LinkDev.Talabat.Infrastructure.Persistence.Common;
using LinkDev.Talabat.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Identity
{
    internal class StoreIdentityDbInitializer(StoreIdentityDbContext _dbContext,UserManager<ApplicationUser> _userManager)
        : DbInitializer(_dbContext), IStoreIdentityInitializer
    {
     

        public override async Task SeedAsync()
        {
            if (!_userManager.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    DisplayName = "Ahmed Nasr",
                    UserName = "ahmed.nasr",
                    Email = "ahmed.nasr@linkdev.com",
                    PhoneNumber = "01122334455"
                };

                await _userManager.CreateAsync(user, "P@ssw0rd"); 
            }
        }
    }
}
