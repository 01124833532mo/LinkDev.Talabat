using LinkDev.Talabat.Apis.Controllers.Base;
using LinkDev.Talabat.Core.Application.Abstraction.Models.Products;
using LinkDev.Talabat.Core.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Apis.Controllers.Controllers.Products
{
    public class ProductsController(IServiceManager serviceManager) : BaseApiController
    {


        [HttpGet]
        public async Task <ActionResult<IEnumerable<ProductToReturnDto>>> GetProducts(string? sort)
        {
            var products = await serviceManager.ProductService.GetProductAsync(sort);
            return Ok(products);
        }

        [HttpGet("{id:int}")]

        public async Task <ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var product = await serviceManager.ProductService.GetProductAsync(id);

            if (product is null)
                return NotFound(new {StatusCode=404,message="Not Found." });

            return Ok(product);
        }


        [HttpGet("brand")]
         
        public async Task <ActionResult<IEnumerable<BrandDto>>> GetBrands()
        {
            var brands = await serviceManager.ProductService.GetBrandsAsync();

            return Ok(brands);
        }

        [HttpGet("categories")]

        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await serviceManager.ProductService.GetCategoriesAsync();

            return Ok(categories);
        }

    }
}
