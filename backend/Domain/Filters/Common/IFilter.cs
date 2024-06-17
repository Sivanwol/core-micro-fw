using Infrastructure.Enums;
using Infrastructure.GQL;
namespace Domain.Filters.Common;

public interface IFilter {
    public IQueryable<T> ApplyFilters<T>(IQueryable<T> query, FilterCollectionOperation filterCollectionOperation, IEnumerable<ViewColumn> SelectedColumns)
        where T : class;
    public void ResetFilters();
}