using System.ComponentModel;
using System.Security.Claims;
using Application.Configs;
using Application.Exceptions;
using Application.Utils;
using Application.Utils.Service;
using FluentValidation;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Interfaces.Controllers;
using Infrastructure.GQL;
using Infrastructure.GQL.Common;
using Infrastructure.GQL.Inputs.Common;
using Infrastructure.Requests.Processor.Services.ActivitiesLogs;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Serilog;
namespace Backend.api.Controllers.v1.GQL;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[GraphRoute("activitiesLogs")]
public class ActivitiesLogsController : BaseGraphController {
    private readonly IValidator<EntityPage> _pageValidator;

    public ActivitiesLogsController(
        IMediator _mediator,
        IValidator<EntityPage> pageValidator,
        BackendApplicationConfig _configuration) : base(_mediator, _configuration) {
        _pageValidator = pageValidator;
    }

    [Query("getActivities", typeof(EntityPage<Activities>))]
    [Description("Will return the user activities")]
    public async Task<IGraphActionResult> GetActivities(
        [FromGraphQL("page")] [Description("Entity Page Data( paging , sorting , filtering )")]
        EntityPage page) {
        if (IsMaintenanceMode()) {
            return BadRequest("Maintenance Mode");
        }
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try {
            var loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var validator = await _pageValidator.ValidateAsync(page);
            if (!validator.IsValid) {
                return BadRequest(string.Join(",", ResponseHelper.HandlerErrorResponse(validator)));
            }
            var activities = await Mediator.Send(new GetActivityRequest {
                LoggedInUserId = Guid.Parse(loggedUserId!),
                PageControl = page
            });
            return Ok(activities);
        }
        catch (EntityNotFoundException e) {
            Log.Logger.Error(e, e.Message);
            return Error("Failed to find user");
        }
        catch (Exception e) {
            Log.Logger.Error(e, e.Message);
            return Error(e.Message);
        }
    }
}