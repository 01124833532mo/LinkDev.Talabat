using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Orders;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Basket;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Orders;
using LinkDev.Talabat.Core.Application.Exeptions;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Contracts.Specifications.Orders;
using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Services.Orders
{
	internal class OrderService(IBasketService basketService, IUnitOfWork unitOfWork, IMapper mapper,IPaymentService paymentService) : IOrderService
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

			// 5 get delivery methode 

			var deliverymethod = await unitOfWork.GetRepository<DeliveryMethod,int>().GetAsync(order.DeliveryMethodId);

			//6 create order
			var orderRepo = unitOfWork.GetRepository<Order, int>();
			var orderspec = new OrderByPaymentIntentSpecifications(basket.PaymentIntentId!);

			var existingOrder = await orderRepo.GetWithSpecAsync(orderspec);

			if(existingOrder is not null)
			{
				orderRepo.Delete(existingOrder);
				await paymentService.CreateOrUpdatePaymentIntent(basket.Id);
			}

			var OrderToCreate = new Order()
			{
				BuyerEmail = buyerEmail,
				ShippingAddress = address,
				Items = orderitems,
				Subtotal= subtotal,
				DeliveryMethod =deliverymethod,
				PaymentIntentId = basket.PaymentIntentId!
				//DeliveryMethodId = order.DeliveryMethodId,

			};

			await orderRepo.AddAsync(OrderToCreate);

			// 7 save at database 
			var created = await unitOfWork.CompleteAsync() > 0;

			if (!created) throw new BadRequestExeption("an error has occured during creating the order");

			return mapper.Map<OrderToReturnDto>(OrderToCreate);

		}

		public async Task<IEnumerable<OrderToReturnDto>> GetOrdersForUserAsync(string buyerEmail)
		{
			var ordersSpecs = new OrderSpecifications(buyerEmail);

			var orders= await unitOfWork.GetRepository<Order,int>().GetAllWithSpecAsync(ordersSpecs);
			return mapper.Map<IEnumerable<OrderToReturnDto>>(orders);
		}


		public async Task<OrderToReturnDto> GetOrderByIdAsync(string buyerEmail, int orderId)
		{
			var ordersSpecs = new OrderSpecifications(buyerEmail,orderId);

			var order = await unitOfWork.GetRepository<Order, int>().GetWithSpecAsync(ordersSpecs);

			if(order is  null) throw new NotFoundExeption(nameof(order), orderId);

			return mapper.Map<OrderToReturnDto>(order);
		}

		public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
			{
			var deliverymethod = await unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();

			return mapper.Map<IEnumerable< DeliveryMethodDto>>(deliverymethod);

			}

		

		
		
	}
}

