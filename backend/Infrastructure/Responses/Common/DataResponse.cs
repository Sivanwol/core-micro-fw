namespace Infrastructure.Responses.Common;

public class DataResponse<TObject> {
    public bool Status { get; set; }

    public string? Error { get; set; }
    public TObject? Data { get; set; }
}