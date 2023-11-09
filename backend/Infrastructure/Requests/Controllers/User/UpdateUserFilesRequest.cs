using System.ComponentModel.DataAnnotations;
using Infrastructure.Requests.Controllers.Common;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.User;

[SwaggerSchema(Required = new[] {
    "Register new user update user info stage 2: update user info request"
})]
[SwaggerTag("Controllers")]
public class UpdateUserFilesRequest {
    [Required]
    [SwaggerSchema("Files")]
    public IEnumerable<FileDetail> Files { get; set; }

    [SwaggerSchema("Files to delete")]
    public IEnumerable<int> FilesToDelete { get; set; }
}