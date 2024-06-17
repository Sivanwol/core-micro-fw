using System.Linq.Expressions;
namespace Domain.Filters.Fields;

public abstract class BaseFilterField<TFilterOperation, TFilterValue> : IFilterField<TFilterValue>
    where TFilterOperation : Enum {
    public BaseFilterField(string filterField, TFilterOperation operation) {
        FilterField = filterField;
        FilterOperation = operation;
    }
    public string FilterField { get; }
    public TFilterOperation FilterOperation { get; set; }

    public TFilterValue? FilterValue { get; set; }

    public ICollection<TFilterValue>? FilterValues { get; set; }

    public bool IsFilterDisabled { get; set; } = false;

    public abstract Expression<Func<T, bool>> ApplyFilter<T>();
}