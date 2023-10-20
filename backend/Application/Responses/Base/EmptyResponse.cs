using Swashbuckle.AspNetCore.Annotations;
namespace Application.Responses.Base;

[SwaggerSchema(Required = new[] {
    "Empty Response Object"
})]
[SwaggerTag("General")]
public class EmptyResponse { }