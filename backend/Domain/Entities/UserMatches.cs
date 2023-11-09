using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Infrastructure.Enums;
namespace Domain.Entities;

[Table("UserMatches")]
public class UserMatches : BaseEntity {
    public int UserId { get; set; }
    public int MatchedUserId { get; set; }
    public int SessionId { get; set; }
    public UserMatchingStatus Status { get; set; }
    public int Score { get; set; }
}