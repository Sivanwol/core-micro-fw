using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
using Infrastructure.Enums;
namespace Domain.Entities;

[Table("configurable-user-view-filters")]
public class ConfigurableUserViewsFilters : BaseEntity {
    [ForeignKey("User")]
    [StringLength(100)]
    public string? UserId { get; set; }

    public ApplicationUser? User { get; set; }
    
    
    [ForeignKey("View")]
    public int ViewId { get; set; }
    public bool Global { get; set; }
    
    public ConfigurableUserView View { get; set; }
    public string FilterFieldName { get; set; }
    public EntityColumnDataType FilterDateType { get; set; }
    public EntityColumnOperationType FilterFieldType { get; set; }
    public FilterCollectionOperation FilterCollectionOperation { get; set; }
    public FilterOperations FilterOperations { get; set; }
    
    public int? FilterMacroValueId { get; set; }
    public ConfigurableUserViewsFilterMacros? FilterMacroValue { get; set; }
    public string? FilterValues { get; set; }
}