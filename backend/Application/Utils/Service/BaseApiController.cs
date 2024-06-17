using Application.Configs;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
namespace Application.Utils.Service;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[EnableCors("AllowOrigin")]
[Produces("application/json")]
public abstract class BaseApiController : ControllerBase {
    protected BackendApplicationConfig Configuration;
    protected IMediator Mediator;

    public BaseApiController(IMediator _mediator, BackendApplicationConfig _configuration) {
        Mediator = _mediator;
        Configuration = _configuration;
    }
    protected bool IsMaintenanceMode() {
        return Configuration.MaintenanceMode;
    }
}