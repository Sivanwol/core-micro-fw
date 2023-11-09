using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Session;

[SwaggerSchema(Required = new[] {
    "Start user Sesstion request"
})]
[SwaggerTag("Session")]
public class StartSessionRequest {

    [Required]
    [SwaggerSchema("list of user id that matching with owner user id")]
    public IEnumerable<int> MatchingUserId { get; set; }

    public Infrastructure.Requests.Processor.Services.Session.StartSessionRequest ToProcessorEntity(int userId) {
        return new Infrastructure.Requests.Processor.Services.Session.StartSessionRequest {
            UserId = userId,
            MatchingUserIds = MatchingUserId
        };
    }
}