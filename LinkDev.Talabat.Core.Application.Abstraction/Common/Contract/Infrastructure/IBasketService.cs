using LinkDev.Talabat.Shared.Models.Basket;

namespace LinkDev.Talabat.Core.Application.Abstraction.Common.Contract.Infrastructure
{
    public interface IBasketService
    {
        Task<CustomerBasketDto> GetCustomerBasketAsync(string basketId);

        Task<CustomerBasketDto> UpdateCustomerBasketAsync(CustomerBasketDto basketDto);

        Task DeleteCustomerBasketAsync(string id);

    }
}
