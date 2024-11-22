using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Basket;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Common;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Employees;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Orders;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Products;
using LinkDev.Talabat.Core.Domain.Entities.Basket;
using LinkDev.Talabat.Core.Domain.Entities.Employees;
using LinkDev.Talabat.Core.Domain.Entities.Orders;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace LinkDev.Talabat.Core.Application.Mapping
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDto>()
               .ForMember(p => p.Brand, o => o.MapFrom(src => src.Brand!.Name))
               .ForMember(p => p.Category, o => o.MapFrom(src => src.Category!.Name))
               //.ForMember(p => p.PictureUrl, o => o.MapFrom(src => $"{"https://localhost:7004"}{src.PictureUrl}"));
               .ForMember(p => p.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());

            CreateMap<ProductBrand, BrandDto>();

            CreateMap<ProductCategory, CategoryDto>();

            CreateMap<Employee, EmployeeToReturnDto>().ForMember(e => e.Department, o => o.MapFrom(src => src.Department!.Name));
            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(dest => dest.DeliveryMethod, option => option.MapFrom(scr => scr.DeliveryMethod!.ShortName));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductId, option => option.MapFrom(src => src.Product.ProductId))
				.ForMember(dest => dest.ProductName, option => option.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.PictureUrl, option => option.MapFrom<OrderItemPictureUrlResolver>());



			CreateMap<Address, AddressDto>();

			CreateMap<DeliveryMethod, DeliveryMethodDto>();


		}
	}
}
