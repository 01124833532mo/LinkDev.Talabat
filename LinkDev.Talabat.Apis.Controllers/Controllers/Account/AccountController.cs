using LinkDev.Talabat.Apis.Controllers.Base;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Auth;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Common;
using LinkDev.Talabat.Core.Application.Abstraction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.Talabat.Apis.Controllers.Controllers.Account
{
    public class AccountController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpPost("login")]

        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var result = await serviceManager.AuthService.LoginAsync(model);
            return Ok(result);
        }


        [HttpPost("Register")]

        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var result = await serviceManager.AuthService.RegisterAsync(model);
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {

            var result = await serviceManager.AuthService.GetCurrentUser(User);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {

            var result = await serviceManager.AuthService.GetUserAddress(User);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {

            var result = await serviceManager.AuthService.UpdateUserAddress(User, address);
            return Ok(result);
        }

        // for front end

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
        {
            return Ok(await serviceManager.AuthService.EmailExists(email!));
        }

        [HttpPost("Get-Refresh-Token")]

        public async Task<ActionResult<UserDto>> RefreshToken([FromBody] RefreshDto model)
        {
            var result = await serviceManager.AuthService.GetRefreshToken(model);
            return Ok(result);
        }

        [HttpPost("Revoke-Refresh-Token")]
        public async Task<ActionResult> RevokeRefreshToken([FromBody] RefreshDto model)
        {
            var result = await serviceManager.AuthService.RevokeRefreshTokenAsync(model);
            return result is false ? BadRequest("Operation Not Successed") : Ok(result);

        }

        [HttpPost("ForgetPasswordEmail")]
        public async Task<ActionResult> ForgetPasswordEmail(ForgetPasswordByEmailDto forgetPasswordDto)
        {
            var result = await serviceManager.AuthService.ForgetPasswordByEmailasync(forgetPasswordDto);
            return Ok(result);
        }
        [HttpPost("VerfiyCodeEmail")]
        public async Task<ActionResult> VerfiyCodeEmail(ResetCodeConfirmationByEmailDto resetCode)
        {
            var result = await serviceManager.AuthService.VerifyCodeByEmailAsync(resetCode);
            return Ok(result);
        }


        [HttpPut("ResetPasswordEmail")]
        public async Task<ActionResult> ResetPasswordEmail(ResetPasswordByEmailDto resetPassword)
        {
            var result = await serviceManager.AuthService.ResetPasswordByEmailAsync(resetPassword);
            return Ok(result);
        }
    }
}
