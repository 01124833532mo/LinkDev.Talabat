using LinkDev.Talabat.Core.Application.Abstraction.Models.Auth;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstraction.Services.Auth
{
    public interface IAuthService
    {
        Task<UserDto> LoginAsync(LoginDto loginDto);

        Task<UserDto> RegisterAsync(RegisterDto registerDto);


        Task<UserDto> GetCurrentUser(ClaimsPrincipal claimsPrincipal);

        Task<AddressDto?> GetUserAddress(ClaimsPrincipal claimsPrincipal);

		Task<AddressDto> UpdateUserAddress(ClaimsPrincipal claimsPrincipal,AddressDto addressDto);

        Task<bool> EmailExists(string email); 
	}
}
