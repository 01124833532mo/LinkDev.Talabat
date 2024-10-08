using LinkDev.Talabat.Core.Domain.Common;
using LinkDev.Talabat.Core.Domain.Contracts;
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

            => WithTraching ? await _dbContext.Set<TEntity>().ToListAsync() : await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        ///{
        ///    if (WithTraching) return await _dbContext.Set<TEntity>().ToListAsync();
        ///
        ///    return await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        ///
        ///}


        public async Task<TEntity?> GetAsync(Tkey id)
        {
            return await _dbContext.Set<TEntity>().FirstAsync();

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
