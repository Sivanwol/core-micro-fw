using System.Net;
using Application.Configs;
using Application.Responses.Base;
using Application.Utils;
using Application.Utils.Service;
using Infrastructure.Requests.Processor.Services.General;
using Infrastructure.Responses.App;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ILogger = Microsoft.Extensions.Logging.ILogger;
namespace FrontApi.Controllers.v1;

[ApiVersion("1.0")]
// [Authorize]
public class GeneralController : BaseApiController {
    private readonly ILogger _logger;
    // private readonly IValidator<MobileLoginUserRequest> _loginValidator;

    public GeneralController(
        IMediator mediator,
        ILoggerFactory loggerFactory,
        BackendApplicationConfig configuration
        // IValidator<MobileLoginUserRequest> loginValidator
        ) : base(mediator, configuration) {
        _logger = loggerFactory.CreateLogger<GeneralController>();
        // _loginValidator = loginValidator;
    }

    [HttpGet("application/settings")]
    [SwaggerOperation("Fetch application settings", "Fetching call when user open the application / first register / login", Tags = new[] {
        "General"
    })]
    [SwaggerResponse(StatusCodes.Status200OK, "Application Settings", typeof(DataResponse<GeneralResponse>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(DataResponse<EmptyResponse>))]
    public async Task<IActionResult> GetApplicationSettings() {
        try {
            var result = await Mediator.Send(new GetApplicationSettingsRequest());
            return ResponseHelper.CreateResponse(result);
        }
        catch (Exception e) {
            _logger.LogError(e, "Error in GetApplicationSettings");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                e.Message
            }, HttpStatusCode.InternalServerError);

        }
    }
}