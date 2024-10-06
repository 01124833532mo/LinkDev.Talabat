using LinkDev.Talabat.Core.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Contracts
{
    public interface IUnitOfWork : IAsyncDisposable
    { 
        IGenericRepository<TEntity,TKey> GetRepository<TEntity, TKey>()
            where TEntity : BaseEntity<TKey> where TKey : IEquatable<TKey>;

        public Task<int> CompleteAsync();

    }
}
