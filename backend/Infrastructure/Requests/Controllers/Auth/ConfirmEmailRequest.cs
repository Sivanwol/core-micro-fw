using Elastic.Apm.Api.Constraints;
using Swashbuckle.AspNetCore.Annotations;
namespace Infrastructure.Requests.Controllers.Auth;

public class ConfirmEmailRequest {
    [SwaggerSchema("User Id of the user")]
    [Required]
    public Guid UserId { get; set; }

    [SwaggerSchema("User Token of the user")]
    [Required]
    public string UserToken { get; set; }

    [SwaggerSchema("Code of the from the email")]
    public string Code { get; set; }


    public Processor.Services.User.ConfirmEmailRequest ToProcessorEntity(Guid loggedUserId, string ipAddress, string userAgent) {
        return new Processor.Services.User.ConfirmEmailRequest {
            LoggedUserId = loggedUserId,
            UserId = UserId,
            UserToken = UserToken,
            Code = Code,
            IpAddress = ipAddress,
            UserAgent = userAgent
        };
    }
}