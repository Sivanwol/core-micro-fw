using Application.Configs;
using Application.Utils;
using Application.Utils.Service;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Processor.Services.User.Create;
using Processor.Services.User.List;
namespace Backend.Controllers.v1;

[ApiVersion("1.0")]
public class UsersController : BaseApiController {
    private readonly ILogger<UsersController> _logger;

    public UsersController(IMediator mediator, BackendApplicationConfig configuration, ILogger<UsersController> logger) : base(mediator, configuration) {
        _logger = logger;
    }

    [HttpPost(Name = "create")]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest request) {
        IValidator<CreateUserRequest> _validator = new InlineValidator<CreateUserRequest>();
        ValidationResult result = await _validator.ValidateAsync(request);
        if (result.IsValid == false) return BadRequest(result.Errors);
        var HandlerResult = await Mediator.Send(request);
        return ResponseHelper.CreateResponse(HandlerResult);
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListUsersAsync() {
        var HandlerResult = await Mediator.Send(new ListUsersRequest());
        return ResponseHelper.CreateResponse(HandlerResult);
    }
}