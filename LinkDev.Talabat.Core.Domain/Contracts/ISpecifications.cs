using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Contracts
{
    public interface ISpecifications<TEntity,Tkey> where TEntity : BaseEntity<Tkey> where Tkey : IEquatable<Tkey>
    {
        public Expression<Func<TEntity,bool>>? Criteria { get; set; } // p=> p.id == 1


        public List<Expression <Func<TEntity,object>>> Includes { get; set; }

        public Expression<Func<TEntity,object>>? OrderBy  { get; set; }

        public Expression<Func <TEntity,object>>? OrderByDesc { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }

        public bool IsPaginationEnabled { get; set; }



    }
}
