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
        public async Task <ActionResult<IEnumerable<ProductToReturnDto>>> GetProducts()
        {
            var products = await serviceManager.ProductService.GetProductAsync();
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

    }
}
