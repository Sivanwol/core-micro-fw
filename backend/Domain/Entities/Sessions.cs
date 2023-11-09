using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;
using Infrastructure.Enums;
namespace Domain.Entities;

[Table("Sessions")]
public class Sessions : BaseEntity {
    public int FeedbackRating { get; set; }
    public SessionStatus Status { get; set; }
}