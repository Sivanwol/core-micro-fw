using Application.Configs;
using GraphQL.AspNet.Controllers;
using MediatR;
namespace Application.Utils.Service;

public class BaseGraphController : GraphController {
    protected BackendApplicationConfig Configuration;
    protected IMediator Mediator;

    public BaseGraphController(IMediator _mediator, BackendApplicationConfig _configuration) {
        Mediator = _mediator;
        Configuration = _configuration;
    }

    public bool IsMaintenanceMode() {
        return Configuration.MaintenanceMode;
    }
}