using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
namespace Domain.Entities;

[Table("configurable-user-view-filter-macros")]
public class ConfigurableUserViewsFilterMacros: BaseEntity {
    public string Identifier { get; set; }
    public string Provider { get; set; }
    public string Description { get; set; }
}