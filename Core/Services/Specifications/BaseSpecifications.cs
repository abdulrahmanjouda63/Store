using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;

namespace Services.Specifications
{
    public class BaseSpecifications<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; set; }
        public List<Expression<Func<TEntity, object>>> IncludeExpression { get; set; } = new List<Expression<Func<TEntity, object>>>();
        public Expression<Func<TEntity, object>>? OrderBy { get; set; }
        public Expression<Func<TEntity, object>>? OrderByDescending { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPagination { get; set; }

        public BaseSpecifications(Expression<Func<TEntity, bool>>? expression)
        {
            Criteria = expression;
        }

        protected void AddInclude(Expression<Func<TEntity, object>> expression)
        {
            IncludeExpression.Add(expression);
        }
        protected void AddOrderBy(Expression<Func<TEntity, object>> expression)
        {
            OrderBy = expression;
        }
        protected void AddOrderByDescending(Expression<Func<TEntity, object>> expression)
        {
            OrderByDescending = expression;
        }
        protected void ApplyPagination(int pageIndex, int pageSize)
        {
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
            IsPagination = true;
        }
    }
}
