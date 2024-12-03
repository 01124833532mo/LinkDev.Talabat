using LinkDev.Talabat.Core.Application.Abstraction.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Abstraction.Models.Orders
{
	public class OrderToCreateDto
	{
        public required string BasketId { get; set; }

        public int DeliveryMethodId { get; set; }

        public required AddressDto   ShippingAddress { get; set; }

    }
}
