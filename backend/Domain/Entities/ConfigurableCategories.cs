using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
namespace Domain.Entities;

[Table("configurable-categories")]
public class ConfigurableCategories : BaseEntity {
    [Column(TypeName = "varchar(100)")]
    public string Name { get; set; }

    [Column(TypeName = "text")]
    public string Description { get; set; }

    public int? ParentCategoryId { get; set; }
    public string? Icon { get; set; }
    public int Position { get; set; }
}