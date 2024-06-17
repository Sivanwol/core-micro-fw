namespace Application.Utils;

public static class Enums {
    public static T MapEnum<T>(dynamic source) {
        var sourceName = Enum.GetName(source.GetType(), source);
        if (!Enum.IsDefined(typeof(T), sourceName))
            throw new ArgumentException($"Cannot map {sourceName} to {typeof(T).Name}");

        return (T)Enum.Parse(typeof(T), sourceName);
    }
}