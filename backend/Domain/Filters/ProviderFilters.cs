using System.Collections.Generic;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Filters.Common;
using Domain.Filters.Fields;
using Infrastructure.Enums;
using Infrastructure.GQL;
using LinqKit;
namespace Domain.Filters;

public class ProviderFilters : BaseFilters
{
    public ProviderFilters()
    {
        IdFilters = new List<IdFilterField>();
        NameFilters = new List<StringFilterField>();
        CountryFilters = new List<IdFilterField>();
        ProviderTypeFilters = new List<EnumFilterField<ProviderType>>();
        CreatedAtFilters = new List<DateFilterField>();
    }
    public ICollection<IdFilterField> IdFilters { get; }
    public ICollection<StringFilterField> NameFilters { get; }
    public ICollection<IdFilterField> CountryFilters { get; }
    public ICollection<EnumFilterField<ProviderType>> ProviderTypeFilters { get; }
    public ICollection<UserIdFilterField> UserFilters { get; }
    public ICollection<DateFilterField> CreatedAtFilters { get; }

    private bool HasAnyFilter()
    {
        return IdFilters.Any() || UserFilters.Any() || NameFilters.Any() || CountryFilters.Any() || CreatedAtFilters.Any() || ProviderTypeFilters.Any();
    }
    public override IQueryable<T> ApplyFilters<T>(IQueryable<T> query, FilterCollectionOperation filterCollectionOperation,
        IEnumerable<ViewColumn> SelectedColumns)
    {

        if (!HasAnyFilter())
        {
            return query;
        }
        var predicate = PredicateBuilder.New<T>(false);
        Func<Expression<Func<T, bool>>, Expression<Func<T, bool>>, Expression<Func<T, bool>>> operatorFunc = filterCollectionOperation == FilterCollectionOperation.Or
            ? (Func<Expression<Func<T, bool>>, Expression<Func<T, bool>>, Expression<Func<T, bool>>>)((p1, p2) => p1.Or(p2))
            : (p1, p2) =>
            {
                if (p1.ToString() == "f => False" && p2.ToString() != "f => False")
                {
                    return p2;
                }
                return p1.And(p2);
            };

        if (HasAllowedColumnFilter(SelectedColumns, nameof(Providers.Id)))
        {
            predicate = ApplyFiltersPredicate<T, int, IdFilterField>(IdFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(Providers.Name)))
        {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(NameFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(Providers.CountryId)))
        {
            predicate = ApplyFiltersPredicate<T, int, IdFilterField>(CountryFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(Providers.ProviderType)))
        {
            predicate = ApplyFiltersPredicate<T, ProviderType, EnumFilterField<ProviderType>>(ProviderTypeFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(Providers.UserId))) {
            predicate = ApplyFiltersPredicate<T, string, UserIdFilterField>(UserFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(Providers.CreatedAt)))
        {
            predicate = ApplyFiltersPredicate<T, DateTime, DateFilterField>(CreatedAtFilters, predicate, operatorFunc);
        }

        return query.Where(predicate);
    }
    public override void ResetFilters()
    {
        IdFilters.Clear();
        NameFilters.Clear();
        CountryFilters.Clear();
        CreatedAtFilters.Clear();
        UserFilters.Clear();
        ProviderTypeFilters.Clear();
    }
}