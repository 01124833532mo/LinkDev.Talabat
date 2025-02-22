using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.Models._Common;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Auth;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Common;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Auth;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Emails;
using LinkDev.Talabat.Core.Application.Exeptions;
using LinkDev.Talabat.Core.Application.Extenstions;
using LinkDev.Talabat.Core.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LinkDev.Talabat.Core.Application.Services.Auth
{
    public class AuthService(
        IMapper mapper,
        IOptions<JwtSettings> jwtSettings,
        UserManager<ApplicationUser> _userManager,
        SignInManager<ApplicationUser> _signInManager,
        IEmailSettings emailSettings) : IAuthService
    {
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;
        private const int _JWTRefreshTokenExpire = 14;

        public async Task<bool> EmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }

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

        public async Task<AddressDto?> GetUserAddress(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.FindUserWithAddress(claimsPrincipal!);

            var address = mapper.Map<AddressDto>(user!.Address);

            return address;
        }

        public async Task<UserDto> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
                throw new UnAuthorizedExeption("Invalid Login");

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);

            //if (result.IsNotAllowed)
            //    throw new UnAuthorizedExeption("Account Not Confermed Yet");

            if (result.IsLockedOut) new UnAuthorizedExeption("Account is loucked ");

            //if (result.RequiresTwoFactor) new UnAuthorizedExeption("Requerd Two-Factor Authentcation ");

            if (!result.Succeeded) throw new UnAuthorizedExeption("Invalid Login. ");

            var response = new UserDto()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = await GenerateTokenAsync(user),
            };

            if (user.RefreshTokens.Any(t => t.IsActice))
            {
                var acticetoken = user.RefreshTokens.FirstOrDefault(x => x.IsActice);
                response.RefreshToken = acticetoken!.Token;
                response.RefreshTokenExpirationDate = acticetoken.ExpireOn;
            }
            else
            {

                var refreshtoken = GenerateRefreshToken();
                response.RefreshToken = refreshtoken.Token;
                response.RefreshTokenExpirationDate = refreshtoken.ExpireOn;

                user.RefreshTokens.Add(new RefreshToken()
                {
                    Token = refreshtoken.Token,
                    ExpireOn = refreshtoken.ExpireOn,
                });
                await _userManager.UpdateAsync(user);
            }



            return response;
        }

        public async Task<UserDto> RegisterAsync(RegisterDto model)
        {
            //if (EmailExists(model.Email).Result)
            //    throw new BadRequestExeption("This Email Is Already in User");
            var email = _userManager.Users.Where(e => e.Email == model.Email).FirstOrDefault();
            if (email is not null)
                throw new BadRequestExeption("Email Already Exsist");

            var user = new ApplicationUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,

            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) throw new ValidationExeption() { Errors = result.Errors.Select(p => p.Description) };

            var refresktoken = GenerateRefreshToken();

            user.RefreshTokens.Add(new RefreshToken()
            {
                Token = refresktoken.Token,
                ExpireOn = refresktoken.ExpireOn
            });

            await _userManager.UpdateAsync(user);



            var response = new UserDto()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = await GenerateTokenAsync(user),
                RefreshToken = refresktoken.Token,
                RefreshTokenExpirationDate = refresktoken.ExpireOn,
            };

            return response;

        }

        public async Task<AddressDto> UpdateUserAddress(ClaimsPrincipal claimsPrincipal, AddressDto addressDto)
        {
            var updatedaddres = mapper.Map<Address>(addressDto);

            var user = await _userManager.FindUserWithAddress(claimsPrincipal!);

            if (user?.Address is not null)
                updatedaddres.Id = user.Address.Id;

            user!.Address = updatedaddres;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) throw new BadRequestExeption(result.Errors.Select(error => error.Description).Aggregate((x, y) => $"{x},{y}"));
            return addressDto;
        }


        public async Task<UserDto> GetRefreshToken(RefreshDto refreshDto, CancellationToken cancellationToken = default)
        {
            var userId = ValidateToken(refreshDto.Token);

            if (userId is null) throw new NotFoundExeption("User id Not Found", nameof(userId));

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) throw new NotFoundExeption("User Do Not Exists", nameof(user.Id));

            var UserRefreshToken = user!.RefreshTokens.SingleOrDefault(x => x.Token == refreshDto.RefreshToken && x.IsActice);

            if (UserRefreshToken is null) throw new NotFoundExeption("Invalid Token", nameof(userId));

            UserRefreshToken.RevokedOn = DateTime.UtcNow;

            var newtoken = await GenerateTokenAsync(user);

            var newrefreshtoken = GenerateRefreshToken();

            user.RefreshTokens.Add(new RefreshToken()
            {
                Token = newrefreshtoken.Token,
                ExpireOn = newrefreshtoken.ExpireOn
            });

            await _userManager.UpdateAsync(user);

            return new UserDto()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = newtoken,
                RefreshToken = newrefreshtoken.Token,
                RefreshTokenExpirationDate = newrefreshtoken.ExpireOn,


            };

        }

        public async Task<bool> RevokeRefreshTokenAsync(RefreshDto refreshDto, CancellationToken cancellationToken = default)
        {
            var userId = ValidateToken(refreshDto.Token);

            if (userId is null) return false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return false;

            var UserRefreshToken = user!.RefreshTokens.SingleOrDefault(x => x.Token == refreshDto.RefreshToken && x.IsActice);

            if (UserRefreshToken is null) return false;

            UserRefreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);
            return true;

        }



        private RefreshToken GenerateRefreshToken()
        {

            var randomNumber = new byte[32];

            var genrator = new RNGCryptoServiceProvider();

            genrator.GetBytes(randomNumber);

            return new RefreshToken()
            {
                Token = Convert.ToBase64String(randomNumber),
                CreatedOn = DateTime.UtcNow,
                ExpireOn = DateTime.UtcNow.AddDays(_JWTRefreshTokenExpire)


            };


        }

        private async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var userclaims = await _userManager.GetClaimsAsync(user);
            var rolesclaims = new List<Claim>();

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
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

            var authkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
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

        private string? ValidateToken(string token)
        {
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var TokenHandler = new JwtSecurityTokenHandler();

            try
            {
                TokenHandler.ValidateToken(token, new TokenValidationParameters()
                {
                    IssuerSigningKey = authKey,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero,

                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                return jwtToken.Claims.First(x => x.Type == ClaimTypes.PrimarySid).Value;

            }
            catch
            {
                return null;
            }
        }

        public async Task<SuccessDto> ForgetPasswordByEmailasync(ForgetPasswordByEmailDto emailDto)
        {
            var user = await _userManager.Users.Where(u => u.Email == emailDto.Email).FirstOrDefaultAsync();

            if (user is null)
                throw new BadRequestExeption("Invalid Email");

            var ResetCode = RandomNumberGenerator.GetInt32(100_000, 999_999);

            var ResetCodeExpire = DateTime.UtcNow.AddMinutes(15);

            user.EmailConfirmResetCode = ResetCode;
            user.EmailConfirmResetCodeExpiry = ResetCodeExpire;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new BadRequestExeption("Something Went Wrong While Sending Reset Code");

            var Email = new Email()
            {
                To = emailDto.Email,
                Subject = "Reset Code For CarCare Account",
                Body = $"We Have Recived Your Request For Reset Your Account Password, \nYour Reset Code Is ==> [ {ResetCode} ] <== \nNote: This Code Will Be Expired After 15 Minutes!",
            };

            await emailSettings.SendEmail(Email);

            var SuccessObj = new SuccessDto()
            {
                Status = "Success",
                Message = "We Have Sent You The Reset Code"
            };

            return SuccessObj;
        }

        public async Task<SuccessDto> VerifyCodeByEmailAsync(ResetCodeConfirmationByEmailDto resetCodeDto)
        {
            var user = await _userManager.Users.Where(u => u.Email == resetCodeDto.Email).FirstOrDefaultAsync();

            if (user is null)
                throw new BadRequestExeption("Invalid Email");

            if (user.EmailConfirmResetCode != resetCodeDto.ResetCode)
                throw new BadRequestExeption("The Provided Code Is Invalid");

            if (user.EmailConfirmResetCodeExpiry < DateTime.UtcNow)
                throw new BadRequestExeption("The Provided Code Has Been Expired");

            var SuccessObj = new SuccessDto()
            {
                Status = "Success",
                Message = "Reset Code Is Verified, Please Proceed To Change Your Password"
            };

            return SuccessObj;
        }

        public async Task<UserDto> ResetPasswordByEmailAsync(ResetPasswordByEmailDto resetCodeDto)
        {
            var user = await _userManager.Users.Where(u => u.Email == resetCodeDto.Email).FirstOrDefaultAsync();

            if (user is null)
                throw new BadRequestExeption("Invalid Email");

            var RemovePass = await _userManager.RemovePasswordAsync(user);

            if (!RemovePass.Succeeded)
                throw new BadRequestExeption("Something Went Wrong While Reseting Your Password");

            var newPass = await _userManager.AddPasswordAsync(user, resetCodeDto.NewPassword);

            if (!newPass.Succeeded)
                throw new BadRequestExeption("Something Went Wrong While Reseting Your Password");

            var mappedUser = new UserDto
            {
                DisplayName = user.DisplayName!,
                Id = user.Id,
                Email = user.Email!,
                Token = await GenerateTokenAsync(user),

            };

            if (user!.RefreshTokens.Any(t => t.IsActice))
            {
                var acticetoken = user.RefreshTokens.FirstOrDefault(x => x.IsActice);
                mappedUser.RefreshToken = acticetoken!.Token;
                mappedUser.RefreshTokenExpirationDate = acticetoken.ExpireOn;
            }
            else
            {

                var refreshtoken = GenerateRefreshToken();
                mappedUser.RefreshToken = refreshtoken.Token;
                mappedUser.RefreshTokenExpirationDate = refreshtoken.ExpireOn;

                user.RefreshTokens.Add(new RefreshToken()
                {
                    Token = refreshtoken.Token,
                    ExpireOn = refreshtoken.ExpireOn,
                });
                await _userManager.UpdateAsync(user);
            }

            return mappedUser;
        }

        public async Task SendMonthlyEmails(IEmailSettings emailSettings)
        {
            var emails = await _userManager.Users.Select(e => e.Email).ToListAsync();

            foreach (var email in emails)
            {
                var emailDto = new Email
                {
                    To = email ?? string.Empty,
                    Subject = "Monthly Newsletter",
                    Body = "Here is your monthly update!"
                };

                await emailSettings.SendEmail(emailDto);
            }
        }
        //private List<UserDto> GetUsersToEmail()
        //{
        //    var users = _userManager.Users
        //    // Replace this with your logic to fetch users from the database
        //    return new List<User>
        //{
        //    new User { Email = "user1@example.com" },
        //    new User { Email = "user2@example.com" }
        //};


    }
}
