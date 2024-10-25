using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstraction.Models.Auth
{
    public class RegisterDto
    {
        [Required]
        public required string DisplayName { get; set; }
        [Required]

        public required string UserName { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]

        public required string PhoneNumber { get; set; }
        [Required]

        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt:,&lt;,])(?!.*\\s).*$",
            ErrorMessage ="Password Must have 1 UpperCase,1 Lowercase,1 number, 1 non alphanumeric and at least 6 characters")]
        public required string Password  { get; set; }

    }
}
