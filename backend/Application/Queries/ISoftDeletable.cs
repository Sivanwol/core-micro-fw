namespace Application.Queries;

public interface ISoftDeletable {
    public DateTime? DeletedAt { get; set; }
}