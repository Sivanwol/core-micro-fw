using GraphQL.AspNet.Attributes;
using Infrastructure.Enums;
namespace Infrastructure.GQL;

public class ViewColumn {
    public int Id { get; set; }
    public string DisplayName { get; set; }

    [GraphSkip]
    public string EntityName { get; set; }

    public string FieldAlias { get; set; }
    public string FieldPath { get; set; }
    public string? FieldFormatter { get; set; }
    public string ColumnName { get; set; }
    public string? Description { get; set; }
    public EntityColumnDataType DataType { get; set; }
    public EntityColumnOperationType FilterOperationType { get; set; }
    public bool IsHidden { get; set; } // will it display in the grid
    public bool IsSortable { get; set; }
    public bool IsFilterable { get; set; }
    public bool IsFixed { get; set; }
    public int Order { get; set; }
}