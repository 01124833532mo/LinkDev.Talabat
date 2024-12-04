using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Basket;
using LinkDev.Talabat.Core.Application.Exeptions;
using LinkDev.Talabat.Core.Domain.Contracts.Infrastructure;
using LinkDev.Talabat.Core.Domain.Entities.Basket;
using LinkDev.Talabat.Shared.Models.Basket;
using Microsoft.Extensions.Configuration;

namespace LinkDev.Talabat.Core.Application.Services.Basket
{
	internal class BasketService(IBasketRepository basketRepository,IMapper mapper,IConfiguration configuration) : IBasketService
    {

        public async Task<CustomerBasketDto> GetCustomerBasketAsync(string basketId)
        {
            var basket = await basketRepository.GetAsync(basketId);
            if (basket is null) throw new NotFoundExeption(nameof(CustomerBasket), basketId);

            return mapper.Map<CustomerBasketDto>(basket);
        }
        public async Task<CustomerBasketDto> UpdateCustomerBasketAsync(CustomerBasketDto basketDto)
        {
            var basket = mapper.Map<CustomerBasket>(basketDto);

            var timeToLive = TimeSpan.FromDays(double.Parse(configuration.GetSection("RedisSetting")["TimeToLiveInDays"]!));

            var updatedBasket=await basketRepository.Updatesync(basket, timeToLive);
            if (updatedBasket is null) throw new BadRequestExeption("can not update , there is a problem with your basket");

            return basketDto;
        }
        public async Task DeleteCustomerBasketAsync(string id)
        {
          var deleted=  await basketRepository.DeleteAsync(id);

            if (!deleted) throw new BadRequestExeption("unable to delete this basket");
        }

       
    }
}
