using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Application.Utils;

public class UppercaseEnumConverter<TEnum> : ValueConverter<TEnum, string>
    where TEnum : struct, Enum {
    public UppercaseEnumConverter(ConverterMappingHints mappingHints = null)
        : base(
            v => v.ToString().ToUpper(),
            v => (TEnum)Enum.Parse(typeof(TEnum), v, true),
            mappingHints) { }
}