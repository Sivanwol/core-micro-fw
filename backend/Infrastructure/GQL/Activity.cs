using Infrastructure.Enums;
namespace Infrastructure.GQL;

public class Activities {
    public int Id { get; set; }
    public User? OwnerUser { get; set; }
    public User? User { get; set; }
    public string? EntityId { get; set; }
    public string EntityType { get; set; }
    public string OperationType { get; set; }
    public string Activity { get; set; }
    public string Details { get; set; }
    public ActivityStatus Status { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public DateTime CreatedAt { get; set; }
}