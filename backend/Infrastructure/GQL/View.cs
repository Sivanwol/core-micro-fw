namespace Infrastructure.GQL;

public class View {
    public Guid ClientKey { get; set; }
    public User User { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsShareAble { get; set; }
    public string? Color { get; set; }
    public bool IsPredefined { get; set; }
    public string EntityType { get; set; }
    public IEnumerable<ViewColumn>? Columns { get; set; }
    public IEnumerable<ViewFilter>? Filters { get; set; }
}