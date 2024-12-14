using LinkDev.Talabat.Apis.Controllers.Base;
using LinkDev.Talabat.Core.Application.Abstraction.Common.Contract.Infrastructure;
using LinkDev.Talabat.Shared.Models.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.Talabat.Apis.Controllers.Controllers.Payment
{
	
	public class PaymentController(IPaymentService paymentService) : BaseApiController
	{
		[Authorize]
		[HttpPost("{basketId}")]
		public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
		{
			var result = await paymentService.CreateOrUpdatePaymentIntent(basketId);
			return Ok(result);
		}

		[HttpPost("webhook")]
		public async Task<IActionResult> WebHook()
		{


			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

			await paymentService.UpdateOrderPaymentStatus(json, Request.Headers["Stripe-Signature"]!);
			return Ok();
		}

	}
}
