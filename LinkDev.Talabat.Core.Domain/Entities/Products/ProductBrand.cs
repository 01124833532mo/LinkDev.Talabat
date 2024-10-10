namespace LinkDev.Talabat.Core.Domain.Entities.Products
{
    public class ProductBrand : BaseAuditableEntity<int>
    {
        public required string Name { get; set; }

    }
}
