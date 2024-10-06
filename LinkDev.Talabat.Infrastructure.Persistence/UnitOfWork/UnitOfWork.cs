using LinkDev.Talabat.Core.Domain.Contracts;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using LinkDev.Talabat.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbcontext;

        private readonly Lazy <IGenericRepository<Product,int>> _productRepository;
        private readonly Lazy<IGenericRepository<ProductBrand, int>> _brandRepository;
        private readonly Lazy<IGenericRepository<ProductCategory, int>> _categoryRepository;


        public UnitOfWork(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
            _productRepository = new Lazy<IGenericRepository<Product, int>>(()=>new GenericRepository<Product,int>(_dbcontext));
            _brandRepository = new Lazy<IGenericRepository<ProductBrand, int>>(() => new GenericRepository<ProductBrand, int>(_dbcontext));
            _categoryRepository = new Lazy<IGenericRepository<ProductCategory, int>>(() => new GenericRepository<ProductCategory, int>(_dbcontext));

        }

        public IGenericRepository<Product, int> ProductRepository  => _productRepository.Value;

        public IGenericRepository<ProductBrand, int> BrandsRepository => _brandRepository.Value;

        public IGenericRepository<ProductCategory, int> CategoriesRepository => _categoryRepository.Value;

    


        public Task<int> CompleteAsync()
        {
            throw new NotImplementedException();
        }

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }
    }



}
