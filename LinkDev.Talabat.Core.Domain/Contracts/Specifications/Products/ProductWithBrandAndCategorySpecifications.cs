using LinkDev.Talabat.Core.Domain.Entities.Products;

namespace LinkDev.Talabat.Core.Domain.Contracts.Specifications.Products
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product, int>
    {

        public ProductWithBrandAndCategorySpecifications(string? sort, int? brandId, int? categoryId, int pageSize, int pageIndex, string? search)
            : base(


                  p =>
                  (string.IsNullOrEmpty(search) || p.NormalizedName.Contains(search))
                  &&
                  (!brandId.HasValue || p.BrandId == brandId.Value)
                            &&
                        (!categoryId.HasValue || p.CategoryId == categoryId.Value)
                  )
        {
            AddIncludes();



            switch (sort)
            {
                case "nameDesc":
                    AddOrderByDesc(p => p.Name);
                    break;

                case "priceAsc":
                    AddOrderBy(p => p.Price);
                    break;

                case "priceDesc":
                    AddOrderByDesc(p => p.Price);
                    break;

                default:
                    AddOrderBy(p => p.Name);
                    break;
            }

            // totalproducts 18 ~ 20
            //page size = 5
            //page index = 3

            ApplyPagination((pageIndex - 1) * pageSize, pageSize);


        }



        public ProductWithBrandAndCategorySpecifications(int id) : base(id)
        {
            AddIncludes();

        }

        private protected override void AddIncludes()
        {
            base.AddIncludes();
            Includes.Add(p => p.Brand!);
            Includes.Add(p => p.Category!);
        }
    }
}
