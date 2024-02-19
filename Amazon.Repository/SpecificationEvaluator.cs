using Amazon.Core.Entities;
using Amazon.Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Repository
{
    public static class SpecificationEvaluator<TEntity> where TEntity: BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> InputQuery,ISpecification<TEntity> spec)
        {
            var query = InputQuery; //_dbContext.Set<Product>
            if(spec.Criteria is not null)  
            query = query.Where(spec.Criteria);

            if(spec.OrderBy is not null )
                query = query.OrderBy(spec.OrderBy);


            if(spec.OrderByDescending is not null ) 
                query = query.OrderByDescending(spec.OrderByDescending);

            if(spec.IsPaginationEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);    

            query = spec.Includes.Aggregate(query, (currentQuery, IncludeExpression) => currentQuery.Include(IncludeExpression));

            return query;

        }
    }
}
