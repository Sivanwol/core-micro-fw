using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Common;
namespace Domain.Entities;

[Table("user-preferences")]
public class ApplicationUserPreferences : BaseEntity {
    [ForeignKey("ApplicationUser")]
    [Column("UserId")]
    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; }

    [StringLength(255)]
    public string PreferenceKey { get; set; }

    [StringLength(500)]
    public string PreferenceValue { get; set; }
}