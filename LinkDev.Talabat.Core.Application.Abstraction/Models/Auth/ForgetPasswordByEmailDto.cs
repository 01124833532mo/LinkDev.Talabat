using System.ComponentModel.DataAnnotations;

namespace LinkDev.Talabat.Core.Application.Abstraction.Models.Auth
{
    public class ForgetPasswordByEmailDto
    {
        [Required]
        public required string Email { get; set; }
    }
}
