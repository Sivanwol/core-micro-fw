using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
using Infrastructure.Enums;
namespace Domain.Entities;

[Table("configurable-entity-column-definitions")]
public class ConfigurableEntityColumnDefinition : BaseEntity {
    [Column(TypeName = "varchar(100)")]
    public string EntityName { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string? ColumnName { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string DisplayName { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string? FieldFormatter { get; set; }

    [Column(TypeName = "varchar(100)")]
    public string FieldAlias { get; set; }

    [Column(TypeName = "varchar(500)")]
    public string FieldPath { get; set; }


    [Column(TypeName = "text")]
    public string? Description { get; set; }

    [ForeignKey("CategoryId")]
    public int? CategoryId { get; set; }

    public ConfigurableCategories Category { get; set; }
    public EntityColumnDataType DataType { get; set; } = EntityColumnDataType.String;
    public EntityColumnOperationType FilterOperationType { get; set; } = EntityColumnOperationType.StringFilterField;
    public bool IsSortable { get; set; }
    public bool IsFilterable { get; set; }
    public DateTime? DisabledAt { get; set; }

    [Column(TypeName = "json")]
    public ICollection<string> Permissions { get; set; }

    public bool IsVirtualColumn { get; set; }

    [Column(TypeName = "json")]
    public string MetaData { get; set; }

    public IEnumerable<ConfigurableUserViewHasConfigurableEntityColumnDefinition>? Views { get; set; }
}