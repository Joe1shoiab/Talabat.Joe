using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    internal static class SpecificationsEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecifications<T> spec)
        {
            var query = inputQuery;
            // use .where(criteria)
            if(spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            // use .orderBy(orderBy)
            if(spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);

            // use .orderByDescending(orderByDescending)
            if(spec.OrderByDescending is not null)
                query = query.OrderByDescending(spec.OrderByDescending);

            // .include( T => T.obj).include(T => T.obj). ....
            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression)=>  currentQuery.Include(includeExpression));

            // use .skip(skip).take(take)
            if(spec.IsPagingEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);
            return query;
        }
    }
}
