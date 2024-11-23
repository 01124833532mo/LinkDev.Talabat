using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Orders;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Basket;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Orders;
using LinkDev.Talabat.Core.Application.Exeptions;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Services.Orders
{
	internal class OrderService(IBasketService basketService, IUnitOfWork unitOfWork, IMapper mapper) : IOrderService
	{
		public async Task<OrderToReturnDto> CreateOrderAsync(string buyerEmail, OrderToCreateDto order)
		{
			// 1 Get Basket from basket repo
			var basket = await basketService.GetCustomerBasketAsync(order.BasketId);

			// 2 Get selected items at basket from products repo
			var orderitems = new List<OrderItem>();
			if (basket.Items.Count() > 0)
			{
				var productrepo = unitOfWork.GetRepository<Product, int>();
				foreach (var item in basket.Items)
				{

					var product = await productrepo.GetAsync(item.Id);

					if (product is not null)
					{
						var productItemOrdered = new ProductItemOrdered()
						{
							ProductId = product.Id,
							ProductName = product.Name,
							PictureUrl = product.PictureUrl ?? "",

						};
						var orderitem = new OrderItem()
						{
							Product = productItemOrdered,
							Price = product.Price,
							Quantity = item.Quantity,
						};
						orderitems.Add(orderitem);
					}

				}


			}

			//  3 calculate subtotal 
		var subtotal = orderitems.Sum(item=>item.Price * item.Quantity);

			// 4 mapping address
			var address = mapper.Map<Address>(order.ShippingAddress);

			//4 create order

			var OrderToCreate = new Order()
			{
				BuyerEmail = buyerEmail,
				ShippingAddress = address,
				Items = orderitems,
				Subtotal= subtotal,

			};

			await unitOfWork.GetRepository<Order,int>().AddAsync(OrderToCreate);

			// 5 save at database 
			var created = await unitOfWork.CompleteAsync() > 0;

			if (!created) throw new BadRequestExeption("an error has occured during creating the order");

			return mapper.Map<OrderToReturnDto>(OrderToCreate);

		}

			public Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
			{
				throw new NotImplementedException();
			}

			public Task<OrderToReturnDto> GetOrderByIdAsync(string buyerEmail, int orderId)
			{
				throw new NotImplementedException();
			}

			public Task<IEnumerable<OrderToReturnDto>> GetOrdersForUserAsync(string buyerEmail)
			{
				throw new NotImplementedException();
			}
		
	}
}

