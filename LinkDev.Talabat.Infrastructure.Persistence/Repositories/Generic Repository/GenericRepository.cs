using LinkDev.Talabat.Core.Domain.Common;
using LinkDev.Talabat.Core.Domain.Contracts;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Repositories.Generic_Repository
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
           


            return WithTraching ? await _dbContext.Set<TEntity>().ToListAsync() : await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();

        }
        ///{
        ///    if (WithTraching) return await _dbContext.Set<TEntity>().ToListAsync();
        ///
        ///    return await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        ///
        ///}


        public async Task<TEntity?> GetAsync(Tkey id)
        {
            

            return await _dbContext.Set<TEntity>().FindAsync(id);

        }

        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, Tkey> spec, bool WithTraching = false)
        {
            return await ApplySpecifications(spec).ToListAsync();
        }

        public async Task<int> GetCountAsync(ISpecifications<TEntity, Tkey> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }


        public async Task<TEntity?> GetWithSpecAsync(ISpecifications<TEntity, Tkey> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }


        public async Task AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }


        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }


        private IQueryable<TEntity> ApplySpecifications(ISpecifications<TEntity,Tkey> spec)
        {
            return SpecificationsEvaluator<TEntity, Tkey>.GetQuery(_dbContext.Set<TEntity>(), spec);
        }

     
    }
}
