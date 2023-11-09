using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Infrastructure.Enums;
namespace Domain.Entities;

[Table("UserSession")]
public class UserSession : BaseEntity {
    public int UserId { get; set; }
    public int SessionId { get; set; }
    public bool? IsMatch { get; set; }
    public UnmatchReasonStatus? UnmatchReason { get; set; }
}