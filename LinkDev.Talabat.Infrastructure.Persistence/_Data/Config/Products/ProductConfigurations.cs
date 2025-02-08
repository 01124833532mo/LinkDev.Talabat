using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Infrastructure.Persistence.Data.Config.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Config.Products
{
    internal class ProductConfigurations : BaseAuditableEntityConfigurations<Product, int>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);

            builder.Property(p => p.NormalizedName).IsRequired().HasMaxLength(100);


            builder.Property(p => p.Description).IsRequired();

            builder.Property(p => p.Price).HasColumnType("decimal(9,2)");


            builder.HasOne(p => p.Brand).WithMany().HasForeignKey(p => p.BrandId).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(p => p.Category).WithMany().HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.SetNull);

        }
    }
}
