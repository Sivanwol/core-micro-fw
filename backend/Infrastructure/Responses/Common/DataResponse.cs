namespace Infrastructure.Responses.Common;

public class DataResponse<TObject> {
    public DataResponse() {
        Errors = new List<string>();
    }

    public bool Status { get; set; }

    public IList<string> Errors { get; set; }
    public TObject? Data { get; set; }
}