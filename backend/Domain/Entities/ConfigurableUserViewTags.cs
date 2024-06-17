using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
namespace Domain.Entities;


[Table("configurable-user-view-tags")]
public class ConfigurableUserViewTags : BaseEntity {
    [ForeignKey("User")]
    [StringLength(100)]
    public string UserId { get; set; }

    public ApplicationUser User { get; set; }

    [StringLength(50)]
    public string Name { get; set; }

    public IEnumerable<ConfigurableUserViewHasConfigurableUserViewTags>? Views { get; set; }
}