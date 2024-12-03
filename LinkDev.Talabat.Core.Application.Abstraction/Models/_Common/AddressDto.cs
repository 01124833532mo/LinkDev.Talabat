using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstraction.Models.Common
{
	public class AddressDto
	{
		public required string FirstName { get; set; }
		public required string LastName { get; set; }

		public required string Street { get; set; }
		public required string City { get; set; }

		public required string Country { get; set; }

	}
}
