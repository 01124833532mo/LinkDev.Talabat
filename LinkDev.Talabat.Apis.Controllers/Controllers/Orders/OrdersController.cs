using LinkDev.Talabat.Apis.Controllers.Base;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Orders;
using LinkDev.Talabat.Core.Application.Abstraction.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Apis.Controllers.Controllers.Orders
{
	[Authorize]
	public class OrdersController(IServiceManager serviceManager) : BaseApiController
	{
		[HttpPost]
		public async Task<ActionResult< OrderToReturnDto>> CreateOrder(OrderToCreateDto orderDto)
		{
			var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

			var result =await serviceManager.OrderService.CreateOrderAsync(buyerEmail!, orderDto);
				
			return Ok(result);
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<OrderToReturnDto>>> GetOrdersForUser()
		{
			var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

			var result = await serviceManager.OrderService.GetOrdersForUserAsync(buyerEmail!);

			return Ok(result);
		}


		[HttpGet("{id}")] // GET: /api/Orders/{id}
		public async Task<ActionResult<OrderToReturnDto>> GetOrder(int id)
		{
			var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
			var result = await serviceManager.OrderService.GetOrderByIdAsync(buyerEmail!, id);
			return Ok(result);
		}


		[HttpGet("deliveryMethods")]
		public async Task<ActionResult<IEnumerable<DeliveryMethodDto>>> GetDeliveryMethods()
		{
			var result = await serviceManager.OrderService.GetDeliveryMethodsAsync();
			return Ok(result);
		}
	}
}
