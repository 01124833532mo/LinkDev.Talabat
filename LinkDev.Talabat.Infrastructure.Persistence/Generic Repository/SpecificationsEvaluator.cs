using LinkDev.Talabat.Core.Domain.Common;
using LinkDev.Talabat.Core.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Infrastructure.Persistence.Generic_Repository
{
    internal static class SpecificationsEvaluator<TEntity,Tkey> where TEntity : BaseEntity<Tkey> where Tkey : IEquatable<Tkey>
    {

        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> InputQuery , ISpecifications<TEntity,Tkey> spec)
        {
            var query = InputQuery;
            

            if(spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }

            if(spec.OrderByDesc is not null)
             query=   query.OrderByDescending(spec.OrderByDesc);

            else if (spec.OrderBy is not null)
               query = query.OrderBy(spec.OrderBy);


            if (spec.IsPaginationEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take); 

            // query = _dbcontext.set<product>().Where(p=> p.Id.Equals(id))
            // includeexprestion 
            // 1. p=>p.brand
            // w. p=> p.category

            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include( includeExpression));


             return query;
        }

    }
}
