using LinkDev.Talabat.Core.Domain.Entities.Products;

namespace LinkDev.Talabat.Core.Domain.Contracts.Specifications.Products
{
    public class ProductWithFilterationForCountSpecifications : BaseSpecifications<Product, int>
    {

        public ProductWithFilterationForCountSpecifications(int? brandId, int? categoryId, string? search)
            : base(
                   p =>
                                     (string.IsNullOrEmpty(search) || p.NormalizedName.Contains(search))
                                &&
                   (!brandId.HasValue || p.BrandId == brandId.Value)
                            &&
                        (!categoryId.HasValue || p.CategoryId == categoryId.Value)
                 )
        {

        }

    }
}
