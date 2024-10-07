using LinkDev.Talabat.Core.Domain.Contracts;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using System.Text.Json;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data
{
    public class StoreContextInitializer(StoreContext _dbContext) : IStoreContextInitializer
    {
       

        public async Task InitializeAsync()
        {
            var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
                await _dbContext.Database.MigrateAsync();
        }

        public async Task SeedAsunc()
        {
            if (!_dbContext.Brands.Any())
            {

                var brandData = await File.ReadAllTextAsync("../LinkDev.Talabat.Infrastructure.Persistence/Data/Seeds/brands.json");

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);

                if (brands?.Count() > 0)
                {

                    await _dbContext.Set<ProductBrand>().AddRangeAsync(brands);
                    await _dbContext.SaveChangesAsync();

                }
            }

            if (!_dbContext.Categories.Any())
            {

                var CategoryData = await File.ReadAllTextAsync("../LinkDev.Talabat.Infrastructure.Persistence/Data/Seeds/categories.json");

                var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoryData);

                if (Categories?.Count() > 0)
                {

                    await _dbContext.Set<ProductCategory>().AddRangeAsync(Categories);
                    await _dbContext.SaveChangesAsync();

                }
            }



            if (!_dbContext.Products.Any())
            {

                var ProductsData = await File.ReadAllTextAsync("../LinkDev.Talabat.Infrastructure.Persistence/Data/Seeds/products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

                if (products?.Count() > 0)
                {

                    await _dbContext.Set<Product>().AddRangeAsync(products);
                    await _dbContext.SaveChangesAsync();

                }
            }
        }
    }
}
