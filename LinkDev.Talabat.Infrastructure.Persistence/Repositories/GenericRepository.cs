using LinkDev.Talabat.Core.Domain.Common;
using LinkDev.Talabat.Core.Domain.Contracts;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Repositories
{
    internal class GenericRepository<TEntity, Tkey>
        : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey> where Tkey : IEquatable<Tkey>
    {
        private readonly StoreContext _dbContext; 

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool WithTraching = false)
        {
            if(typeof(TEntity) == typeof(Product))
            {
                return WithTraching ? (IEnumerable<TEntity>)await _dbContext.Set<Product>().Include(p => p.Brand).Include(p => p.Category).ToListAsync()
                   : (IEnumerable<TEntity>)await _dbContext.Set<Product>().Include(p => p.Brand).Include(p => p.Category).AsNoTracking().ToListAsync();
                    
            }
                    return WithTraching? await _dbContext.Set<TEntity>().ToListAsync() : await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();

        }
        ///{
        ///    if (WithTraching) return await _dbContext.Set<TEntity>().ToListAsync();
        ///
        ///    return await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        ///
        ///}


        public async Task<TEntity?> GetAsync(Tkey id)
        {
            if(typeof(TEntity) == typeof(Product))
            {
                return await   _dbContext.Set<Product>().Where(p => p.Id.Equals(id)).Include(p => p.Brand).Include(p => p.Category).FirstOrDefaultAsync() as TEntity ;
            }

            return await _dbContext.Set<TEntity>().FindAsync(id);

        }


        public async Task AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public  void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

  
        public void Update(TEntity entity)
        {
_dbContext.Set<TEntity>().Update(entity);
        }
    }
}
