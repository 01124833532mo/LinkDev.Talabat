using LinkDev.Talabat.Core.Domain.Common;
using LinkDev.Talabat.Core.Domain.Contracts.Persistence;
using LinkDev.Talabat.Core.Domain.Entities.Products;
using LinkDev.Talabat.Infrastructure.Persistence.Data;
using LinkDev.Talabat.Infrastructure.Persistence.Repositories;
using LinkDev.Talabat.Infrastructure.Persistence.Repositories.Generic_Repository;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbcontext;


        private readonly ConcurrentDictionary<string,object> _repositories;
      


        public UnitOfWork(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
            _repositories = new ConcurrentDictionary<string,object>();
        }



        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
           where TEntity : BaseEntity<TKey>
           where TKey : IEquatable<TKey>
        {
            //var typeName= typeof(TEntity).Name;

            // if (_properties.ContainsKey(typeName))
            // {
            //     return (IGenericRepository<TEntity, TKey>)_properties[typeName];
            // }

            // var repository = new GenericRepository<TEntity, TKey>(_dbcontext);

            // _properties.Add(typeName, repository);

            // return repository;

        return  (IGenericRepository<TEntity, TKey>)  _repositories
                .GetOrAdd(typeof(TEntity).Name, new GenericRepository<TEntity, TKey>(_dbcontext));
        }


        public async Task<int> CompleteAsync()
        {
            return await _dbcontext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
           await _dbcontext.DisposeAsync();
        }

       
    }



}
