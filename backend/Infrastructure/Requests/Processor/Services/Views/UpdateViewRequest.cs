namespace Infrastructure.Requests.Processor.Services.Views;

public class UpdateViewRequest : CreateViewRequest {
    public string UserId { get; set; }
    public Guid ViewClientKey { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsShareAble { get; set; }
    public string? Color { get; set; }
}