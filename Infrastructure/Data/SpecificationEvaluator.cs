using System;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
    {

        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria); // Apply the criteria to filter the query; x => x.Brand == brand
        
        }

        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy); // Apply the OrderBy to sort the query; x => x.Price
        }
        else if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending); // Apply the OrderByDescending to sort the query; x => x.Price
        }

        if (spec.IsDistinct)
        {
            query = query.Distinct(); // Apply Distinct to remove duplicate results
        }

        return query;
    }

    public static IQueryable<TResult> GetQuery<TSpec, TResult>(IQueryable<T> query, ISpecification<T, TResult> spec)
    {

        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria); // Apply the criteria to filter the query; x => x.Brand == brand
        
        }

        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy); // Apply the OrderBy to sort the query; x => x.Price
        }
        else if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending); // Apply the OrderByDescending to sort the query; x => x.Price
        }

        var selectQuery = query as IQueryable<TResult>;

        if (spec.Select != null)
        {
            selectQuery = query.Select(spec.Select); // Apply the Select to project the query; x => new ProductDto { Name = x.Name, Price = x.Price }
        }

        if (spec.IsDistinct)
        {
            selectQuery = selectQuery?.Distinct(); // Apply Distinct to remove duplicate results
        }

        return selectQuery ?? query.Cast<TResult>(); // Cast the query to TResult if Select is not provided; this assumes that T and TResult are the same type

    }

}
