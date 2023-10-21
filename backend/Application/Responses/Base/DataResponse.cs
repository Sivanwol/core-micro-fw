using Swashbuckle.AspNetCore.Annotations;
namespace Application.Responses.Base;

[SwaggerSchema(Required = new[] {
    "General Response Object"
})]
[SwaggerTag("General")]
public class DataResponse<TObject> where TObject : class {
    public DataResponse() {
        Errors = new List<string>();
        Status = false;
        StatusCode = 400;
    }
    public DataResponse(List<string> errors) {
        Errors = errors;
        Status = false;
        StatusCode = 400;
    }
    public DataResponse(TObject data) {
        Errors = new List<string>();
        Data = data;
        StatusCode = 200;
        Status = true;
    }

    [SwaggerSchema("The Status Of response", ReadOnly = true)]
    public bool Status { get; set; }

    [SwaggerSchema("The Status code", ReadOnly = true)]
    public int StatusCode { get; set; }

    [SwaggerSchema("Error messages")]
    public List<string> Errors { get; set; }

    [SwaggerSchema("The data of response")]
    public TObject? Data { get; set; }
}