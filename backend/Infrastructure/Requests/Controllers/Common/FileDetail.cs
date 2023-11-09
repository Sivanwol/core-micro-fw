using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Common;

[SwaggerSchema(Required = new[] {
    "Register new user update user info stage 2: update user info request"
})]
[SwaggerTag("Files")]
public class FileDetail {
    [Required]
    [SwaggerSchema("Media Image Width")]
    public int MediaImageWidth { get; set; }

    [Required]
    [SwaggerSchema("Media Image Height")]
    public int MediaImageHeight { get; set; }

    [Required]
    [SwaggerSchema("Media Is Main Selection")]
    public bool MediaIsMain { get; set; }

    [Required]
    [SwaggerSchema("Media File Raw")]
    public IFormFile MediaRaw { get; set; }
}