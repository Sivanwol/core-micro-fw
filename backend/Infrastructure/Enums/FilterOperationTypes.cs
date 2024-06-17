namespace Infrastructure.Enums;

public enum FilterGuidOperation {
    Equal,
    NotEqual,
    In,
    NotIn
}

public enum FilterNumberOperation {
    Equal,
    NotEqual,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,
    Between,
    NotBetween,
    In,
    NotIn
}

public enum FilterIDOperation {
    Equal,
    NotEqual,
    In,
    NotIn
}

public enum FilterUserIDOperation {
    Equal,
    NotEqual,
    In,
    NotIn
}

public enum FilterStringOperation {
    Equal,
    NotEqual,
    Contains,
    NotContains,
    StartsWith,
    NotStartsWith,
    EndsWith,
    NotEndsWith,
    In,
    NotIn
}

public enum FilterDateOperation {
    Equal,
    NotEqual,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,
    Between,
    NotBetween,
    In,
    NotIn
}

public enum FilterBooleanOperation {
    Equal,
    NotEqual
}

public enum FilterEnumOperation
{
    Equal,
    NotEqual,
    In,
    NotIn
}