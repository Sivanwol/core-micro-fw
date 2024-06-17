using System.Linq.Expressions;
using System.Reflection;
using Infrastructure.Enums;
namespace Domain.Filters.Fields;

public class EnumFilterField<F> :  BaseFilterField<FilterEnumOperation, F>  where F : Enum{

    public EnumFilterField(F filterField, FilterEnumOperation operation) : base(filterField.ToString(), operation) { }
    public EnumFilterField(F filterField, FilterEnumOperation operation, bool isDisabled) : base(filterField.ToString(), operation) {
        IsFilterDisabled = isDisabled;
    }

    public override Expression<Func<T, bool>> ApplyFilter<T>() {
        if (IsFilterDisabled) {
            return null;
        }
        Expression valuesExpression = null;
        Expression filterExpression = null;
        Expression valueExpression = null;
        if (FilterOperation is FilterEnumOperation.In or FilterEnumOperation.NotIn) {
            if (FilterValues == null || FilterValues.Count == 0) {
                throw new InvalidOperationException("Filter values are not provided.");
            }
            valuesExpression = Expression.Constant(FilterValues, typeof(ICollection<int>));
        } else {
            if (FilterValue == null) {
                throw new InvalidOperationException("Filter value is not provided.");
            }
            valueExpression = Expression.Constant(FilterValue, typeof(int));
        }
        // Accessing the property with provided field name
        var parameter = Expression.Parameter(typeof(T), "t");
        var property = Expression.Property(parameter, FilterField); // Creating filter expression based on operation and type of TFilterValue

        valueExpression = Expression.Constant(FilterValue, typeof(bool));
        MethodCallExpression? compareWithOrdinal;
        MethodInfo? methodInfo;
        switch (FilterOperation) {
            case FilterEnumOperation.In:
                methodInfo = typeof(Enumerable).GetMethods()
                    .Where(m => m.Name == nameof(Enumerable.Contains))
                    .Select(m => new {
                        M = m,
                        P = m.GetParameters(),
                        G = m.GetGenericArguments()
                    })
                    .Where(x => x.P.Length == 2
                                && x.G.Length == 1
                                && x.P[0].ParameterType == typeof(IEnumerable<>).MakeGenericType(x.G[0])
                                && x.P[1].ParameterType == x.G[0])
                    .Select(x => x.M)
                    .First()
                    .MakeGenericMethod(typeof(F));

                filterExpression = Expression.Call(methodInfo, valuesExpression, property);
                break;
            case FilterEnumOperation.NotIn:
                methodInfo = typeof(Enumerable).GetMethods()
                    .Where(m => m.Name == nameof(Enumerable.Contains))
                    .Select(m => new {
                        M = m,
                        P = m.GetParameters(),
                        G = m.GetGenericArguments()
                    })
                    .Where(x => x.P.Length == 2
                                && x.G.Length == 1
                                && x.P[0].ParameterType == typeof(IEnumerable<>).MakeGenericType(x.G[0])
                                && x.P[1].ParameterType == x.G[0])
                    .Select(x => x.M)
                    .First()
                    .MakeGenericMethod(typeof(F));

                filterExpression = Expression.Call(methodInfo, valuesExpression, property);
                filterExpression = Expression.Not(filterExpression);
                break;
            case FilterEnumOperation.Equal:
                filterExpression = Expression.Equal(property, valueExpression!);
                break;
            case FilterEnumOperation.NotEqual:
                filterExpression = Expression.NotEqual(property, valueExpression!);
                break;
            default:
                throw new InvalidOperationException("Filter operation is not supported.");
        }

        return Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
    }
}