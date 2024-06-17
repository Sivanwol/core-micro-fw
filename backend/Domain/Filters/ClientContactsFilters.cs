using System.Linq.Expressions;
using Domain.Entities;
using Domain.Filters.Common;
using Domain.Filters.Fields;
using Infrastructure.Enums;
using Infrastructure.GQL;
using LinqKit;
namespace Domain.Filters;

public class ClientContactsFilters : BaseFilters {
    public ClientContactsFilters() {
        IdFilters = new List<IdFilterField>();
        FirstNameFilters = new List<StringFilterField>();
        LastNameFilters = new List<StringFilterField>();
        CompanyNameFilters = new List<StringFilterField>();
        DepartmentNameFilters = new List<StringFilterField>();
        Phone1Filters = new List<StringFilterField>();
        Phone2Filters = new List<StringFilterField>();
        CountryFilters = new List<IdFilterField>();
        CityFilters = new List<StringFilterField>();
        AddressFilters = new List<StringFilterField>();
        PostalCodeFilters = new List<StringFilterField>();
        StateFilters = new List<StringFilterField>();
        CreateAtFilters = new List<DateFilterField>();
    }
    public ICollection<IdFilterField> IdFilters { get; }
    public ICollection<StringFilterField> FirstNameFilters { get; }
    public ICollection<StringFilterField> LastNameFilters { get; }
    public ICollection<StringFilterField> CompanyNameFilters { get; }
    public ICollection<StringFilterField> DepartmentNameFilters { get; }
    public ICollection<StringFilterField> Phone1Filters { get; }
    public ICollection<StringFilterField> Phone2Filters { get; }
    public ICollection<IdFilterField> CountryFilters { get; }
    public ICollection<StringFilterField> CityFilters { get; }
    public ICollection<StringFilterField> AddressFilters { get; }
    public ICollection<StringFilterField> PostalCodeFilters { get; }
    public ICollection<StringFilterField> StateFilters { get; }
    public ICollection<DateFilterField> CreateAtFilters { get; }

    private bool HasAnyFilter() {
        return IdFilters.Any() ||
               FirstNameFilters.Any() ||
               LastNameFilters.Any() ||
               CompanyNameFilters.Any() ||
               DepartmentNameFilters.Any() ||
               Phone1Filters.Any() ||
               Phone2Filters.Any() ||
               CountryFilters.Any() ||
               CityFilters.Any() ||
               AddressFilters.Any() ||
               PostalCodeFilters.Any() ||
               StateFilters.Any() ||
               CreateAtFilters.Any();
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

        if (HasAllowedColumnFilter(SelectedColumns, nameof(ClientContacts.Id))) {
            predicate = ApplyFiltersPredicate<T, int, IdFilterField>(IdFilters, predicate, operatorFunc);
        }

        if (HasAllowedColumnFilter(SelectedColumns, nameof(ClientContacts.FirstName))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(FirstNameFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ClientContacts.LastName))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(LastNameFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ClientContacts.Company))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(CompanyNameFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ClientContacts.Department))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(DepartmentNameFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ClientContacts.Phone1))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(Phone1Filters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ClientContacts.Phone2))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(Phone2Filters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ClientContacts.CountryId))) {
            predicate = ApplyFiltersPredicate<T, int, IdFilterField>(CountryFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ClientContacts.City))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(CityFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ClientContacts.Address))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(AddressFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ClientContacts.PostalCode))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(PostalCodeFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ClientContacts.State))) {
            predicate = ApplyFiltersPredicate<T, string, StringFilterField>(StateFilters, predicate, operatorFunc);
        }
        if (HasAllowedColumnFilter(SelectedColumns, nameof(ClientContacts.CreatedAt))) {
            predicate = ApplyFiltersPredicate<T, DateTime, DateFilterField>(CreateAtFilters, predicate, operatorFunc);
        }

        return query.Where(predicate);
    }
    public override void ResetFilters() {
        IdFilters.Clear();
        FirstNameFilters.Clear();
        LastNameFilters.Clear();
        CompanyNameFilters.Clear();
        DepartmentNameFilters.Clear();
        Phone1Filters.Clear();
        Phone2Filters.Clear();
        CountryFilters.Clear();
        CityFilters.Clear();
        AddressFilters.Clear();
        PostalCodeFilters.Clear();
        StateFilters.Clear();
        CreateAtFilters.Clear();
    }
}