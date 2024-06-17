using System.Linq.Expressions;
using System.Reflection;
using Infrastructure.Enums;
namespace Domain.Filters.Fields;

public class StringFilterField : BaseFilterField<FilterStringOperation, string> {

    public StringFilterField(string filterField, FilterStringOperation operation) : base(filterField, operation) { }
    public StringFilterField(string filterField, FilterStringOperation operation, bool isDisabled) : base(filterField, operation) {
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
        if (FilterOperation is FilterStringOperation.In or FilterStringOperation.NotIn) {
            if (FilterValues == null || FilterValues.Count == 0) {
                throw new InvalidOperationException("Filter values are not provided.");
            }
            valuesExpression = Expression.Constant(FilterValues, typeof(ICollection<string>));
        } else {
            if (FilterValue == null) {
                throw new InvalidOperationException("Filter value is not provided.");
            }
            valueExpression = Expression.Constant(FilterValue, typeof(string));
        }

        MethodInfo compareInfo = typeof(string).GetMethod("IndexOf", new[] {
            typeof(string), typeof(StringComparison)
        })!;
        MethodCallExpression? compareWithOrdinal;
        MethodInfo? methodInfo;
        switch (FilterOperation) {
            case FilterStringOperation.In:
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
                    .MakeGenericMethod(typeof(string));

                filterExpression = Expression.Call(methodInfo, valuesExpression, property);
                break;
            case FilterStringOperation.NotIn:
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
                    .MakeGenericMethod(typeof(string));

                filterExpression = Expression.Call(methodInfo, valuesExpression, property);
                filterExpression = Expression.Not(filterExpression);
                break;
            case FilterStringOperation.Equal:
                filterExpression = Expression.Equal(property, valueExpression!);
                break;
            case FilterStringOperation.NotEqual:
                filterExpression = Expression.NotEqual(property, valueExpression!);
                break;
            case FilterStringOperation.Contains:
                compareWithOrdinal = Expression.Call(property, compareInfo, valueExpression, Expression.Constant(StringComparison.OrdinalIgnoreCase));
                filterExpression = Expression.GreaterThanOrEqual(compareWithOrdinal, Expression.Constant(0));
                break;
            case FilterStringOperation.NotContains:
                compareWithOrdinal = Expression.Call(property, compareInfo, valueExpression, Expression.Constant(StringComparison.OrdinalIgnoreCase));
                filterExpression = Expression.Not(Expression.GreaterThanOrEqual(compareWithOrdinal, Expression.Constant(0)));
                break;
            case FilterStringOperation.StartsWith:
                filterExpression = Expression.Call(property, typeof(string).GetMethod(nameof(string.StartsWith), new[] {
                    typeof(string)
                })!, valueExpression!);
                break;
            case FilterStringOperation.NotStartsWith:
                filterExpression = Expression.Not(Expression.Call(property, typeof(string).GetMethod(nameof(string.StartsWith), new[] {
                    typeof(string)
                })!, valueExpression!));
                break;
            case FilterStringOperation.EndsWith:
                filterExpression = Expression.Call(property, typeof(string).GetMethod(nameof(string.EndsWith), new[] {
                    typeof(string)
                })!, valueExpression!);
                break;
            case FilterStringOperation.NotEndsWith:
                filterExpression = Expression.Not(Expression.Call(property, typeof(string).GetMethod(nameof(string.EndsWith), new[] {
                    typeof(string)
                })!, valueExpression!));
                break;

            default:
                throw new InvalidOperationException("Filter operation is not supported.");
        }
        return Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
    }
}