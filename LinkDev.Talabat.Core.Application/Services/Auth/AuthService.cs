using LinkDev.Talabat.Core.Application.Abstraction.Models.Auth;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Auth;
using LinkDev.Talabat.Core.Application.Exeptions;
using LinkDev.Talabat.Core.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Services.Auth
{
    public class AuthService(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager) : IAuthService
    {
        public async Task<UserDto> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
                throw new UnAuthorizedExeption("Invalid Login");

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password,lockoutOnFailure:true);

            if (result.IsNotAllowed)
                throw new UnAuthorizedExeption("Account Not Confermed Yet");

            if(result.IsLockedOut) new UnAuthorizedExeption("Account is loucked ");

            //if (result.RequiresTwoFactor) new UnAuthorizedExeption("Requerd Two-Factor Authentcation ");

            if(!result.Succeeded) new UnAuthorizedExeption("Invalid Login. ");

            var response = new UserDto()
            {
                Id=user.Id,
                DisplayName = user.DisplayName,
                Email =user.Email!,
                Token = "this will be token"
            };

            return response;
        }

        public async Task<UserDto> RegisterAsync(RegisterDto model)
        {
            var user = new ApplicationUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,

            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) throw new ValidationExeption() { Errors = result.Errors.Select(p => p.Description) };

            var response = new UserDto()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = "this will be token"
            };

            return response;

        }
    }
}
