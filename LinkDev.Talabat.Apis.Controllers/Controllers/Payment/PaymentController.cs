using LinkDev.Talabat.Apis.Controllers.Base;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using LinkDev.Talabat.Shared.Models.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.Talabat.Apis.Controllers.Controllers.Payment
{
	[Authorize]
	public class PaymentController(IPaymentService paymentService) : BaseApiController
	{
		[HttpPost("{basketId}")]
		public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
		{
			var result = await paymentService.CreateOrUpdatePaymentIntent(basketId);
			return Ok(result);
		}
	}
}
