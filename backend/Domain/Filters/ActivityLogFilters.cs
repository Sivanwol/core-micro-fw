using System.Linq.Expressions;
using Domain.Entities;
using Domain.Filters.Common;
using Domain.Filters.Fields;
using Infrastructure.Enums;
using Infrastructure.GQL;
using LinqKit;
namespace Domain.Filters;

public class ActivityLogFilters : BaseFilters {
    public ActivityLogFilters() {
        IpAddressFilters = new List<StringFilterField>();
        UserAgentFilters = new List<StringFilterField>();
        IdFilters = new List<IdFilterField>();
        UserIdFilters = new List<StringFilterField>();
        EntityIdFilters = new List<StringFilterField>();
        OperationTypeFilters = new List<StringFilterField>();
        ActivityFilters = new List<StringFilterField>();
        EntityTypeFilters = new List<StringFilterField>();
        StatusFilters = new List<StringFilterField>();
        CreatedAtFilters = new List<DateFilterField>();
    }
    public ICollection<StringFilterField> IpAddressFilters { get; }
    public ICollection<StringFilterField> UserAgentFilters { get; }
    public ICollection<IdFilterField> IdFilters { get; }
    public ICollection<StringFilterField> UserIdFilters { get; }
    public ICollection<StringFilterField> EntityIdFilters { get; }
    public ICollection<StringFilterField> OperationTypeFilters { get; }
    public ICollection<StringFilterField> ActivityFilters { get; }
    public ICollection<StringFilterField> EntityTypeFilters { get; }
    public ICollection<StringFilterField> StatusFilters { get; }
    public ICollection<DateFilterField> CreatedAtFilters { get; }

    private bool HasAnyFilter() {
        return IpAddressFilters.Any() ||
               UserAgentFilters.Any() ||
               IdFilters.Any() ||
               UserIdFilters.Any() ||
               EntityIdFilters.Any() ||
               OperationTypeFilters.Any() ||
               ActivityFilters.Any() ||
               EntityTypeFilters.Any() ||
               StatusFilters.Any() ||
               CreatedAtFilters.Any();
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

        if (HasAllowedColumnFilter(SelectedColumns, nameof(ActivityLog.IpAddress))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(IpAddressFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ActivityLog.UserAgent))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(UserAgentFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ActivityLog.Id))) {
            predicate = ApplyFiltersPredicate<T, int, IdFilterField>(IdFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ActivityLog.UserId))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(UserIdFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ActivityLog.EntityId))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(EntityIdFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ActivityLog.OperationType))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(OperationTypeFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ActivityLog.Activity))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(ActivityFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ActivityLog.EntityType))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(EntityTypeFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ActivityLog.Status))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(StatusFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ActivityLog.CreatedAt))) {
            predicate = ApplyFiltersPredicate<T, DateTime, DateFilterField>(CreatedAtFilters, predicate, operatorFunc);
        }

        return query.Where(predicate);
    }
    public override void ResetFilters() {
        IpAddressFilters.Clear();
        UserAgentFilters.Clear();
        IdFilters.Clear();
        UserIdFilters.Clear();
        EntityIdFilters.Clear();
        OperationTypeFilters.Clear();
        ActivityFilters.Clear();
        EntityTypeFilters.Clear();
        StatusFilters.Clear();
        CreatedAtFilters.Clear();
    }
}