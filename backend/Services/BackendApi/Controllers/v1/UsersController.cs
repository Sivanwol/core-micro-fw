using System.Net;
using Application.Configs;
using Application.Utils;
using Application.Utils.Service;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Requests.Account.Backoffice;
using Infrastructure.Requests.Processor.Services.User;
using Infrastructure.Services.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
namespace BackendApi.Controllers.v1;

[ApiVersion("1.0")]
// [Authorize]
public class UsersController : BaseApiController {
    private readonly IValidator<ForgetPasswordRequest> _forgetPasswordValidator;
    private readonly IJwtAuthManager _jwtAuthManager;
    private readonly ILogger _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(
        IMediator mediator,
        ILoggerFactory loggerFactory,
        IJwtAuthManager jwtAuthManager,
        BackendApplicationConfig configuration) : base(mediator, configuration) {
        _logger = loggerFactory.CreateLogger<UsersController>();
        _jwtAuthManager = jwtAuthManager;
    }
    [HttpGet("profile/{userId}")]
    [SwaggerOperation("Request User profile", "Profile of user", Tags = new[] {
        "Users"
    })]
    public async Task<IActionResult> Profile(int userId) {
        try {
            _logger.LogInformation($"Request User Profile: {userId}");
            var result = await Mediator.Send(new GetUserProfileRequest {
                UserId = userId
            });
            return ResponseHelper.CreateResponse(result);
        }
        catch (Exception e) {
            _logger.LogError(e, "Error in Profile");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                e.Message
            }, HttpStatusCode.InternalServerError);

        }
    }
}