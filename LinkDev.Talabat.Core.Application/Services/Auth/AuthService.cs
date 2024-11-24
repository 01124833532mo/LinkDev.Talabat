using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Auth;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Common;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Auth;
using LinkDev.Talabat.Core.Application.Exeptions;
using LinkDev.Talabat.Core.Application.Extenstions;
using LinkDev.Talabat.Core.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Services.Auth
{
    public class AuthService(
        IMapper mapper,
        IOptions<JwtSettings> jwtSettings,
        UserManager<ApplicationUser> _userManager,
        SignInManager<ApplicationUser> _signInManager) : IAuthService
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;

		public async Task<UserDto> GetCurrentUser(ClaimsPrincipal claimsPrincipal)
		{
            var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email!);

            return new UserDto()
            {
                Id = user!.Id,
                Email = user!.Email!,
                DisplayName = user.DisplayName,
                Token = await GenerateTokenAsync(user)

            };

		}

		public async Task<AddressDto> GetUserAddress(ClaimsPrincipal claimsPrincipal)
		{
            var user = await _userManager.FindUserWithAddress(claimsPrincipal!);

          var address=  mapper.Map<AddressDto>(user!.Address);

            return address;
		}

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

            if(!result.Succeeded) throw new UnAuthorizedExeption("Invalid Login. ");

            var response = new UserDto()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token =await GenerateTokenAsync(user),
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
                Token = await GenerateTokenAsync(user),
            };

            return response;

        }


        private async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var userclaims = await _userManager.GetClaimsAsync(user);
            var rolesclaims = new List<Claim>();

            var roles = await _userManager.GetRolesAsync(user);

            foreach(var role in roles)
            {
                rolesclaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.PrimarySid,user.Id),
                new Claim(ClaimTypes.Email,user.Email!),
            new Claim(ClaimTypes.GivenName,user.DisplayName),



            }.Union(userclaims)
            .Union(rolesclaims);

            var authkey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signinCredintal = new SigningCredentials(authkey, SecurityAlgorithms.HmacSha256);

            var tokenObj = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                claims: Claims,
                signingCredentials: signinCredintal

                );
          
            return new JwtSecurityTokenHandler().WriteToken(tokenObj);

        }
    }
}
