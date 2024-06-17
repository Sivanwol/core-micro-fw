using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.DTO.ConfigurableEntities;
using Domain.Entities.Common;
namespace Domain.Entities;

[Table("configurable-user-views")]
public class ConfigurableUserView : BaseEntity {
    [ForeignKey("User")]
    [StringLength(100)]
    public string UserId { get; set; }

    public ApplicationUser User { get; set; }
    public int? ParentId { get; set; }
    public ConfigurableUserView? Parent { get; set; }
    public Guid ClientUniqueKey { get; set; } = Guid.NewGuid();
    public string EntityType { get; set; }

    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(200)]
    public string? Description { get; set; }

    public bool IsShareAble { get; set; }

    [StringLength(8)]
    public string? Color { get; set; }

    [Column(TypeName = "json")]
    public ICollection<ConfigurableEntityMetaData> Settings { get; set; }

    [Column(TypeName = "json")]
    public ICollection<string> Permissions { get; set; }

    public bool IsPredefined { get; set; } // Indicates if column is predefined
    public DateTime? DeletedAt { get; set; }

    public IEnumerable<ConfigurableUserViewHasConfigurableUserViewTags>? Tags { get; set; }
    public IEnumerable<ConfigurableUserViewHasConfigurableEntityColumnDefinition>? Columns { get; set; }
}