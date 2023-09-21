using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Processor.Handlers.User.Create;
using Processor.Handlers.User.List;

namespace FrontApi.Controllers;

[Route("api/[controller]")]
public class UsersController : ControllerBase {
    private readonly IMediator _mediator;
    private readonly ILogger<UsersController> _logger;


    public UsersController(IMediator mediator,ILogger<UsersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
   
    [HttpPost(Name = "create")]  
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest request) {
        IValidator<CreateUserRequest> _validator = new InlineValidator<CreateUserRequest>();
        ValidationResult result = await _validator.ValidateAsync(request);
        if (result.IsValid == false) return BadRequest(result.Errors);
        var HandlerResult = await _mediator.Send(request);
        return Ok(HandlerResult);
    }
    
    [HttpGet("list")]
    public async Task<IActionResult> ListUsersAsync() {
        var HandlerResult = await _mediator.Send(new ListUsersRequest());
        return Ok(HandlerResult);
    }
}