using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Enums;
using Infrastructure.GQL;
namespace Domain.Entities;

public enum ActivityOperationType {
    SYSTEM,
    USER
}

[Table("activity-logs")]
public class ActivityLog {


    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [StringLength(36)]
    public string? OwnerUserId { get; set; }

    public ApplicationUser? OwnerUser { get; set; }

    [StringLength(36)]
    public string? UserId { get; set; }

    public ApplicationUser? User { get; set; }

    public string? EntityId { get; set; }

    [Required]
    [MaxLength(100)]
    public string EntityType { get; set; }

    [Required]
    [MaxLength(20)]
    public string OperationType { get; set; } = nameof(ActivityOperationType.USER);

    [Required]
    [MaxLength(500)]
    public string Activity { get; set; }

    [DataType(DataType.Text)]
    public string Details { get; set; }

    [Required]
    [MaxLength(100)]
    public string Status { get; set; }

    [MaxLength(30)]
    public string IpAddress { get; set; }

    [MaxLength(100)]
    public string UserAgent { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }

    public Activities ToGql() {
        return new() {
            Id = Id,
            OwnerUser = OwnerUser?.ToGql(),
            User = User?.ToGql(),
            EntityId = EntityId,
            EntityType = EntityType,
            OperationType = OperationType,
            Activity = Activity,
            Details = Details,
            Status = Enum.Parse<ActivityStatus>(Status),
            IpAddress = IpAddress,
            UserAgent = UserAgent,
            CreatedAt = CreatedAt
        };
    }
}