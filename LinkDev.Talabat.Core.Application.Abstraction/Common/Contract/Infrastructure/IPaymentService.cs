using LinkDev.Talabat.Shared.Models.Basket;

namespace LinkDev.Talabat.Core.Application.Abstraction.Common.Contract.Infrastructure
{ 
	public interface IPaymentService
	{
		Task<CustomerBasketDto> CreateOrUpdatePaymentIntent(string basketId);

		Task UpdateOrderPaymentStatus(string requestBody, string header);
	}
}
