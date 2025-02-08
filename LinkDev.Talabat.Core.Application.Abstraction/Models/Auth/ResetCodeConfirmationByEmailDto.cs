using System.ComponentModel.DataAnnotations;

namespace LinkDev.Talabat.Core.Application.Abstraction.Models.Auth
{
    public class ResetCodeConfirmationByEmailDto : ForgetPasswordByEmailDto
    {
        [Required]
        public required int ResetCode { get; set; }
    }
}
