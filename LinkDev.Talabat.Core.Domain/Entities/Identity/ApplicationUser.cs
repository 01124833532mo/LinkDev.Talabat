using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public required  string DisplayName { get; set; }

        public virtual Address?  Address { get; set; }
    }
}
