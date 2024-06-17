using System.Linq.Expressions;
using Infrastructure.Enums;
namespace Domain.Filters.Fields;

public class BoolFilterField : BaseFilterField<FilterBooleanOperation, bool> {

    public BoolFilterField(string filterField, FilterBooleanOperation operation) : base(filterField, operation) { }
    public BoolFilterField(string filterField, FilterBooleanOperation operation, bool isDisabled) : base(filterField, operation) {
        IsFilterDisabled = isDisabled;
    }

    public override Expression<Func<T, bool>> ApplyFilter<T>() {
        if (IsFilterDisabled) {
            return null;
        }
        Expression filterExpression = null;
        Expression valueExpression = null;
        // Accessing the property with provided field name
        var parameter = Expression.Parameter(typeof(T), "t");
        var property = Expression.Property(parameter, FilterField); // Creating filter expression based on operation and type of TFilterValue

        valueExpression = Expression.Constant(FilterValue, typeof(bool));
        switch (FilterOperation) {
            case FilterBooleanOperation.Equal:
                filterExpression = Expression.Equal(property, valueExpression!);
                break;
            case FilterBooleanOperation.NotEqual:
                filterExpression = Expression.NotEqual(property, valueExpression!);
                break;
            default:
                throw new InvalidOperationException("Filter operation is not supported.");
        }

        return Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
    }
}