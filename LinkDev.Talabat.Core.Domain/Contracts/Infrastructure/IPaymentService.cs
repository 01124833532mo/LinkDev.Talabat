using LinkDev.Talabat.Core.Domain.Entities.Basket;
using LinkDev.Talabat.Shared.Models.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Contracts.Infrastructure
{
	public interface IPaymentService
	{
		Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(string basketId);

		Task UpdateOrderPaymentStatus(string requestBody, string header);
	}
}
