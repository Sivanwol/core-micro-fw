using System.Linq.Expressions;
using Domain.Filters.Common;
using Infrastructure.Enums;
using Infrastructure.GQL;
using Microsoft.EntityFrameworkCore;
using Serilog;
namespace Domain.Filters;

public class RecordFilterPagination<TFilter> where TFilter : IFilter {

    public RecordFilterPagination() {
        Filters = (TFilter)Activator.CreateInstance(typeof(TFilter));
    }
    public int Page { get; set; } = 1;
    public IEnumerable<ViewColumn> SelectedColumns { get; set; }
    public int PageSize { get; set; } = 10;
    public TFilter Filters { get; set; }
    public FilterCollectionOperation FilterCollectionOperation { get; set; } = FilterCollectionOperation.Or;
    public SortDirection SortDirection { get; set; }
    public string SortByField { get; set; } = "CreatedAt";

    public Dictionary<string, string> MarcoData = new();
    
    public IQueryable<T> ApplyQueryWithoutPagination<T>(IQueryable<T> query) where T : class {
        query = Filters.ApplyFilters(query, FilterCollectionOperation, SelectedColumns);
        var type = typeof(T);
        var parameter = Expression.Parameter(typeof(T), "p");
        var propertyAccess = Expression.PropertyOrField(parameter, SortByField);
        var orderByExp = Expression.Lambda(propertyAccess, parameter);
        var typeArguments = new[] {
            type, propertyAccess.Type
        };

        if (SortDirection == SortDirection.Ascending) {
            var orderByCall = Expression.Call(
                typeof(Queryable),
                "OrderBy",
                typeArguments,
                query.Expression,
                Expression.Quote(orderByExp));

            query = query.Provider.CreateQuery<T>(orderByCall);
        } else {
            var orderByDescendingCall = Expression.Call(
                typeof(Queryable),
                "OrderByDescending",
                typeArguments,
                query.Expression,
                Expression.Quote(orderByExp));

            query = query.Provider.CreateQuery<T>(orderByDescendingCall);
        }
        Log.Information("Query without pagination");
        Log.Information("Filters: {@Filters}", Filters);
        Log.Verbose(query.ToQueryString());
        return query;
    }

    public IQueryable<T> ApplyQuery<T>(IQueryable<T> query) where T : class {
        query = Filters.ApplyFilters(query, FilterCollectionOperation, SelectedColumns);
        if (SelectedColumns.Any(c => c.ColumnName == SortByField && !c.IsSortable)) {
            SortByField = "CreatedAt"; // default sort
        }
        var type = typeof(T);
        var parameter = Expression.Parameter(typeof(T), "p");
        var propertyAccess = Expression.PropertyOrField(parameter, SortByField);
        var orderByExp = Expression.Lambda(propertyAccess, parameter);
        var typeArguments = new[] {
            type, propertyAccess.Type
        };

        if (SortDirection == SortDirection.Ascending) {
            var orderByCall = Expression.Call(
                typeof(Queryable),
                "OrderBy",
                typeArguments,
                query.Expression,
                Expression.Quote(orderByExp));

            query = query.Provider.CreateQuery<T>(orderByCall);
        } else {
            var orderByDescendingCall = Expression.Call(
                typeof(Queryable),
                "OrderByDescending",
                typeArguments,
                query.Expression,
                Expression.Quote(orderByExp));

            query = query.Provider.CreateQuery<T>(orderByDescendingCall);
        }
        query = query.Skip((Page - 1) * PageSize).Take(PageSize);
        Log.Information("Query with pagination");
        Log.Information("Page: {Page}, PageSize: {PageSize}", Page, PageSize);
        Log.Information("SortDirection: {SortDirection}, SortByField: {SortByField}", SortDirection, SortByField);
        Log.Information("Filters: {@Filters}", Filters);
        return query;
    }
}