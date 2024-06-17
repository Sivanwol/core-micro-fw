using System.Linq.Expressions;
using Domain.Entities;
using Domain.Filters.Common;
using Domain.Filters.Fields;
using Infrastructure.Enums;
using Infrastructure.GQL;
using LinqKit;
namespace Domain.Filters;

public class ClientFilters : BaseFilters {
    public ClientFilters() {
        IdFilters = new List<IdFilterField>();
        OwnerUserFilters = new List<UserIdFilterField>();
        NameFilters = new List<StringFilterField>();
        CountryFilters = new List<IdFilterField>();
        CityFilters = new List<StringFilterField>();
        AddressFilters = new List<StringFilterField>();
        CreatedAtFilters = new List<DateFilterField>();
    }
    public ICollection<IdFilterField> IdFilters { get; }
    public ICollection<UserIdFilterField> OwnerUserFilters { get; }
    public ICollection<StringFilterField> NameFilters { get; }
    public ICollection<IdFilterField> CountryFilters { get; }
    public ICollection<StringFilterField> CityFilters { get; }
    public ICollection<StringFilterField> AddressFilters { get; }
    public ICollection<DateFilterField> CreatedAtFilters { get; }

    private bool HasAnyFilter() {
        return IdFilters.Any() || OwnerUserFilters.Any() || NameFilters.Any() || CountryFilters.Any() || CityFilters.Any() || AddressFilters.Any() || CreatedAtFilters.Any();
    }
    public override IQueryable<T> ApplyFilters<T>(IQueryable<T> query, FilterCollectionOperation filterCollectionOperation,
        IEnumerable<ViewColumn> SelectedColumns) {

        if (!HasAnyFilter()) {
            return query;
        }
        var predicate = PredicateBuilder.New<T>(false);
        Func<Expression<Func<T, bool>>, Expression<Func<T, bool>>, Expression<Func<T, bool>>> operatorFunc = filterCollectionOperation == FilterCollectionOperation.Or
            ? (Func<Expression<Func<T, bool>>, Expression<Func<T, bool>>, Expression<Func<T, bool>>>)((p1, p2) => p1.Or(p2))
            : (p1, p2) => {
                if (p1.ToString() == "f => False" && p2.ToString() != "f => False") {
                    return p2;
                }
                return p1.And(p2);
            };

        if (HasAllowedColumnFilter(SelectedColumns, nameof(Clients.Id))) {
            predicate = ApplyFiltersPredicate<T, int, IdFilterField>(IdFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(Clients.OwnerUserId))) {
            predicate = ApplyFiltersPredicate<T, string, UserIdFilterField>(OwnerUserFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(Clients.Name))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(NameFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(Clients.CountryId))) {
            predicate = ApplyFiltersPredicate<T, int, IdFilterField>(CountryFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(Clients.City))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(CityFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(Clients.Address))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(AddressFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(Clients.CreatedAt))) {
            predicate = ApplyFiltersPredicate<T, DateTime, DateFilterField>(CreatedAtFilters, predicate, operatorFunc);
        }

        return query.Where(predicate);
    }
    public override void ResetFilters() {
        IdFilters.Clear();
        OwnerUserFilters.Clear();
        NameFilters.Clear();
        CountryFilters.Clear();
        CityFilters.Clear();
        AddressFilters.Clear();
        CreatedAtFilters.Clear();
    }
}