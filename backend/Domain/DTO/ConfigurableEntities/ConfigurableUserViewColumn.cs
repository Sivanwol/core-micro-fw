using Infrastructure.Enums;
namespace Domain.DTO.ConfigurableEntities;

public class ConfigurableUserViewColumn {
    public string ColumnName { get; set; }
    public EntityColumnDataType DataType { get; set; }
    public EntityColumnOperationType FilterOperationType { get; set; }
    public ICollection<string> Permissions { get; set; }
    public bool IsHidden { get; set; }
    public bool IsFilterable { get; set; }
    public bool IsSortable { get; set; }
    public bool IsGroupable { get; set; } // not in use
    public bool IsEditable { get; set; } // not in use
    public bool IsFixed { get; set; } // not in use
    public int Order { get; set; }
}