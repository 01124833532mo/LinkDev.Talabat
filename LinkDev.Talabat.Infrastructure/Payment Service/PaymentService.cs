using LinkDev.Talabat.Core.Application.Exeptions;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Basket;
using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Shared.Models;
using Microsoft.Extensions.Options;
using Stripe;
using Product = LinkDev.Talabat.Core.Domain.Entities.Products.Product;

namespace LinkDev.Talabat.Infrastructure.Payment_Service
{
	public class PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork,IOptions<RedisSettings> redisSettings) : IPaymentService
	{

		private readonly RedisSettings _redisSettings = redisSettings.Value;
		public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
		{
			var basket = await basketRepository.GetAsync(basketId);

			if (basket is null) throw new NotFoundExeption(nameof(CustomerBasket), basketId);

			if (basket.DeliveryMethodId.HasValue)
			{
				var deliverymethode= await unitOfWork.GetRepository<DeliveryMethod,int>().GetAsync(basket.DeliveryMethodId.Value);
				if (deliverymethode is null) throw new NotFoundExeption(nameof(DeliveryMethod), basket.DeliveryMethodId.Value);
				basket.ShippingPrice = deliverymethode.Cost;
			}

			if (basket.Items.Count() > 0)
			{
				var productRepo =  unitOfWork.GetRepository<Product, int>();

				foreach (var item in basket.Items)
				{
					var product = await productRepo.GetAsync(item.Id);
					if (product is null) throw new NotFoundExeption(nameof(Product), item.Id);
					if (item.Price != product.Price)
						item.Price = product.Price;

				}
			}


			PaymentIntent? paymentIntent = null;
			PaymentIntentService paymentIntentService = new PaymentIntentService(); 

			if (string.IsNullOrEmpty(basket.PaymentIntentId)) {// create New PaymentIntent

				var options = new PaymentIntentCreateOptions()
				{
					Amount =(long) basket.Items.Sum(items => items.Price*100 * items.Quantity) + (long)basket.ShippingPrice*100,
					Currency="USD",
					PaymentMethodTypes = new List<string>() { "Card" }
				};

				paymentIntent = await paymentIntentService.CreateAsync(options);	 // integration with Stripe
				basket.PaymentIntentId = paymentIntent.Id;
				basket.ClientSecret = paymentIntent.ClientSecret;

			}

			else  // Update PaymentIntent
			{
				var options = new PaymentIntentUpdateOptions()
				{
					Amount = (long)basket.Items.Sum(items => items.Price * 100 * items.Quantity) + (long)basket.ShippingPrice * 100,
				};

				await paymentIntentService.UpdateAsync(basket.PaymentIntentId, options);      // integration with Stripe

			}

			await basketRepository.Updatesync(basket, TimeSpan.FromDays(_redisSettings.TimeToLiveInDays));
			return basket;


		}
	}
}
