using AutoMapper;
using LinkDev.Talabat.Core.Application.Abstraction.Common;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Products;
using LinkDev.Talabat.Core.Application.Abstraction.Products;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Products;
using LinkDev.Talabat.Core.Application.Exeptions;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Contracts.Specifications;
using LinkDev.Talabat.Core.Domain.Contracts.Specifications.Products;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Application.Services.Products
{
    internal class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<Pagination<ProductToReturnDto>> GetProductAsync( ProductSpecParams specParams)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(specParams.Sort, specParams.BrandId,specParams.CategoryId,specParams.PageSize,specParams.PageIndex,specParams.Search);


            var products = await unitOfWork.GetRepository<Product, int>().GetAllWithSpecAsync(spec);

            var data = mapper.Map<IEnumerable<ProductToReturnDto>>(products);
            var countSpec = new ProductWithFilterationForCountSpecifications(specParams.BrandId, specParams.CategoryId,specParams.Search);
            var count = await unitOfWork.GetRepository<Product, int>().GetCountAsync(countSpec);

            return new Pagination<ProductToReturnDto>(specParams.PageIndex, specParams.PageSize,count) {Data= data };
        }

        public async Task<ProductToReturnDto> GetProductAsync(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(id);

            var product = await unitOfWork.GetRepository<Product, int>().GetWithSpecAsync(spec);
            if(product is null)
            {
                throw new NotFoundExeption(nameof(product), id);
            }
            var productToReturn = mapper.Map<ProductToReturnDto>(product);
            return productToReturn;

        }
        public async Task<IEnumerable<BrandDto>> GetBrandsAsync()
        {
            var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();

            var brandsToReturn = mapper.Map<IEnumerable<BrandDto>>(brands);

            return brandsToReturn;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await unitOfWork.GetRepository<ProductCategory, int>().GetAllAsync();

            var categoriesToReturn = mapper.Map<IEnumerable<CategoryDto>>(categories);

            return categoriesToReturn;
        }


        

    }
}
