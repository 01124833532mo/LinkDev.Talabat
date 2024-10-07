using LinkDev.Talabat.Core.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data
{
    public class StoreContext  : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyInformtion).Assembly);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> Brands { get; set; }

        public DbSet<ProductCategory> Categories { get; set; }


    }
}
