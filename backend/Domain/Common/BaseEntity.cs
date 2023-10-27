namespace Domain.Common; 

public abstract class BaseEntity {
    public int ID { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}