using LinkDev.Talabat.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext dbContext)
        {

            if (!dbContext.Brands.Any()) {

                var brandData = await File.ReadAllTextAsync("../LinkDev.Talabat.Infrastructure.Persistence/Data/Seeds/brands.json");

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                if (brands?.Count() > 0) {

                    await dbContext.Set<ProductBrand>().AddRangeAsync(brands);
                    await dbContext.SaveChangesAsync();

                }
            }

            if (!dbContext.Categories.Any())
            {

                var CategoryData = await File.ReadAllTextAsync("../LinkDev.Talabat.Infrastructure.Persistence/Data/Seeds/categories.json");

                var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoryData);

                if (Categories?.Count() > 0)
                {

                    await dbContext.Set<ProductCategory>().AddRangeAsync(Categories);
                    await dbContext.SaveChangesAsync();

                }
            }



            if (!dbContext.Products.Any())
            {

                var ProductsData = await File.ReadAllTextAsync("../LinkDev.Talabat.Infrastructure.Persistence/Data/Seeds/products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

                if (products?.Count() > 0)
                {

                    await dbContext.Set<Product>().AddRangeAsync(products);
                    await dbContext.SaveChangesAsync();

                }
            }

        }

    }
}
