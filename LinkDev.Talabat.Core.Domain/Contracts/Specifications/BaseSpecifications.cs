using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Core.Domain.Contracts.Specifications
{
    public abstract class  BaseSpecifications<TEntity, Tkey> : ISpecifications<TEntity, Tkey>
        where TEntity : BaseEntity<Tkey> where Tkey : IEquatable<Tkey>
    {
        public Expression<Func<TEntity,bool>>? Criteria { get; set; } = null;
        public List<Expression<Func<TEntity, object>>> Includes { get; set; }= new List<Expression<Func<TEntity, object>>>();
        public Expression<Func<TEntity, object>>? OrderBy { get; set; }= null;
        public Expression<Func<TEntity, object>>? OrderByDesc { get; set; }=null;
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }

        protected BaseSpecifications()
        {
            
        }

        protected BaseSpecifications(Expression<Func<TEntity, bool>> Criteriaexpression)
        {
            Criteria = Criteriaexpression;
            //Includes = new List<Expression<Func<TEntity, object>>> ();
        }

        protected BaseSpecifications(Tkey id)
        {
            Criteria = E => E.Id.Equals(id);
            //Includes = new List<Expression<Func<TEntity, object>>>();
        }

        private protected virtual void AddIncludes()
        {
         
        }

        private  protected virtual void AddOrderBy(Expression<Func<TEntity, object>> orderByExpresstion)
        {
            OrderBy= orderByExpresstion;
        }

        private protected virtual void AddOrderByDesc(Expression<Func<TEntity, object>> orderByExpresstionDesc)
        {
            OrderByDesc = orderByExpresstionDesc;
        }

        private protected void ApplyPagination(int skip,int take)
        {
            IsPaginationEnabled = true;

            Skip = skip;
            Take = take;
        }
    }
}
