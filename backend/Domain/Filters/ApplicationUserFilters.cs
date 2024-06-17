using System.Linq.Expressions;
using Domain.Entities;
using Domain.Filters.Common;
using Domain.Filters.Fields;
using Infrastructure.Enums;
using Infrastructure.GQL;
using LinqKit;
namespace Domain.Filters;

public class ApplicationUserFilters : BaseFilters {
    public ApplicationUserFilters() {
        FirstNameFilters = new List<StringFilterField>();
        LastNameFilters = new List<StringFilterField>();
        IdFilters = new List<UserIdFilterField>();
        Addressilters = new List<StringFilterField>();
        EmailFilters = new List<StringFilterField>();
        RegisterCompletedAtFilters = new List<DateFilterField>();
    }
    public ICollection<UserIdFilterField> IdFilters { get; }
    public ICollection<StringFilterField> FirstNameFilters { get; }
    public ICollection<StringFilterField> LastNameFilters { get; }
    public ICollection<StringFilterField> Addressilters { get; }
    public ICollection<StringFilterField> EmailFilters { get; }
    public ICollection<DateFilterField> RegisterCompletedAtFilters { get; }

    private bool HasAnyFilter() {
        return FirstNameFilters.Any() ||
               LastNameFilters.Any() ||
               IdFilters.Any() ||
               EmailFilters.Any() ||
               RegisterCompletedAtFilters.Any() ||
               Addressilters.Any();
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

        if (HasAllowedColumnFilter(SelectedColumns, nameof(ApplicationUser.FirstName))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(FirstNameFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ApplicationUser.LastName))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(LastNameFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ApplicationUser.Id))) {
            predicate = ApplyFiltersPredicate<T, string, UserIdFilterField>(IdFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ApplicationUser.Address))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(Addressilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ApplicationUser.Email))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(EmailFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ApplicationUser.RegisterCompletedAt))) {
            predicate = ApplyFiltersPredicate<T, DateTime, DateFilterField>(RegisterCompletedAtFilters, predicate, operatorFunc);
        }

        return query.Where(predicate);
    }
    public override void ResetFilters() {
        Addressilters.Clear();
        LastNameFilters.Clear();
        IdFilters.Clear();
        FirstNameFilters.Clear();
        EmailFilters.Clear();
        RegisterCompletedAtFilters.Clear();
    }
}