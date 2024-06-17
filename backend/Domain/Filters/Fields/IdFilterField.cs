using System.Linq.Expressions;
using System.Reflection;
using Infrastructure.Enums;
namespace Domain.Filters.Fields;

public class IdFilterField : BaseFilterField<FilterIDOperation, int> {

    public IdFilterField(string filterField, FilterIDOperation operation) : base(filterField, operation) { }
    public IdFilterField(string filterField, FilterIDOperation operation, bool isDisabled) : base(filterField, operation) {
        IsFilterDisabled = isDisabled;
    }

    public override Expression<Func<T, bool>> ApplyFilter<T>() {
        if (IsFilterDisabled) {
            return null;
        }
        Expression valuesExpression = null;
        Expression filterExpression = null;
        Expression valueExpression = null;
        // Accessing the property with provided field name
        var parameter = Expression.Parameter(typeof(T), "t");
        var property = Expression.Property(parameter, FilterField); // Creating filter expression based on operation and type of TFilterValue
        if (FilterOperation is FilterIDOperation.In or FilterIDOperation.NotIn) {
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
        MethodCallExpression? compareWithOrdinal;
        MethodInfo? methodInfo;
        switch (FilterOperation) {
            case FilterIDOperation.In:
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
                    .MakeGenericMethod(typeof(int));

                filterExpression = Expression.Call(methodInfo, valuesExpression, property);
                break;
            case FilterIDOperation.NotIn:
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
                    .MakeGenericMethod(typeof(int));

                filterExpression = Expression.Call(methodInfo, valuesExpression, property);
                filterExpression = Expression.Not(filterExpression);
                break;
            case FilterIDOperation.Equal:
                filterExpression = Expression.Equal(property, valueExpression!);
                break;
            case FilterIDOperation.NotEqual:
                filterExpression = Expression.NotEqual(property, valueExpression!);
                break;
            default:
                throw new InvalidOperationException("Filter operation is not supported.");
        }

        return Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
    }
}