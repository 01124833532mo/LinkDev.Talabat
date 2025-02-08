using System.ComponentModel.DataAnnotations;

namespace LinkDev.Talabat.Core.Application.Abstraction.Models.Auth
{
    public class ResetPasswordByEmailDto : ForgetPasswordByEmailDto
    {
        [Required]
        public required string NewPassword { get; set; }

    }
}
