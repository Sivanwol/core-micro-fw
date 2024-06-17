using System.ComponentModel;
using System.Security.Claims;
using Application.Configs;
using Application.Utils;
using Application.Utils.Service;
using FluentValidation;
using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Interfaces.Controllers;
using Infrastructure.Enums;
using Infrastructure.GQL;
using Infrastructure.GQL.Inputs.View;
using Infrastructure.Requests.Processor.Services.Countries;
using Infrastructure.Requests.Processor.Services.Home;
using Infrastructure.Requests.Processor.Services.Views;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Serilog;
namespace Backend.api.Controllers.v1.GQL;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[GraphRoute("general")]
public class GeneralController : BaseGraphController {
    private readonly IValidator<ViewCreateInput> _viewCreateValidator;
    private readonly IValidator<ViewUpdateColumnsInput> _viewUpdateColumnsValidator;
    private readonly IValidator<ViewUpdateInput> _viewUpdateValidator;
    private readonly IValidator<ViewFilterUpdateInput> _viewFilterUpdateValidator;
    public GeneralController(
        IMediator _mediator,
        IValidator<ViewCreateInput> viewCreateValidator,
        IValidator<ViewUpdateInput> viewUpdateValidator,
        IValidator<ViewUpdateColumnsInput> viewUpdateColumnsValidator,
        IValidator<ViewFilterUpdateInput> viewFilterUpdateValidator,
        BackendApplicationConfig _configuration) : base(_mediator, _configuration) {
        _viewCreateValidator = viewCreateValidator;
        _viewUpdateValidator = viewUpdateValidator;
        _viewUpdateColumnsValidator = viewUpdateColumnsValidator;
        _viewFilterUpdateValidator = viewFilterUpdateValidator;
    }
    [Query("maintenanceMode", typeof(bool))]
    [Description("Get maintenance mode status")]
    public IGraphActionResult MaintenanceMode() {
        return Ok(IsMaintenanceMode());
    }

    [Query("getCountries", typeof(IEnumerable<Country>))]
    [Description("Get all suported countries")]
    public async Task<IGraphActionResult> GetCountries() {
        if (IsMaintenanceMode()) {
            return BadRequest("Maintenance Mode");
        }
        try {
            return Ok(await Mediator.Send(new GetCountriesRequest()));
        }
        catch (Exception e) {
            Log.Logger.Error(e, e.Message);
            return Error(e.Message);
        }
    }

    [Query("getViews", typeof(IEnumerable<View>))]
    [Description("Get all views of the user and default views")]
    public async Task<IGraphActionResult> GetViews() {
        if (IsMaintenanceMode()) {
            return BadRequest("Maintenance Mode");
        }
        try {
            var loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var views = await Mediator.Send(new GetViewsRequest {
                UserId = loggedUserId!
            });
            return Ok(views);
        }
        catch (Exception e) {
            Log.Logger.Error(e, e.Message);
            return Error(e.Message);
        }
    }

    [Query("getDashboard", typeof(Dashboard))]
    [Description("Get Dashboard Data")]
    public async Task<IGraphActionResult> GetDashboard() {
        return Ok(await Mediator.Send(new GetDashboardRequest()));
    }
    
    [Query("getView", typeof(View))]
    [Description("Get all views of the user and default views")]
    public async Task<IGraphActionResult> GetView(
        [FromGraphQL("viewClientKey")] [Description("view client key")]
        Guid viewClientKey) {
        if (IsMaintenanceMode()) {
            return BadRequest("Maintenance Mode");
        }
        if (viewClientKey == Guid.Empty) {
            return BadRequest("ViewClientKey is required");
        }
        try {
            var loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await Mediator.Send(new GetViewRequest {
                UserId = loggedUserId!,
                ViewClientKey = viewClientKey
            }));
        }
        catch (Exception e) {
            Log.Logger.Error(e, e.Message);
            return Error(e.Message);
        }
    }

    [Query("getAvailableColumnsForView", typeof(ICollection<ViewColumn>))]
    [Description("get available columns for view by given entity type")]
    public async Task<IGraphActionResult> GetAvailableColumnsForView(
        [FromGraphQL("entityType")] [Description("view client key")]
        EntityTypes entityType) {
        if (IsMaintenanceMode()) {
            return BadRequest("Maintenance Mode");
        }
        try {
            var loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await Mediator.Send(new GetAvailableColumnsForViewRequest {
                UserId = loggedUserId!,
                EntityType = entityType
            }));
        }
        catch (Exception e) {
            Log.Logger.Error(e, e.Message);
            return Error(e.Message);
        }
    }

    [Mutation("addView", typeof(View))]
    [Description("Add a new view")]
    public async Task<IGraphActionResult> AddView(
        [FromGraphQL("view")] [Description("view")]
        ViewCreateInput view) {
        if (IsMaintenanceMode()) {
            return BadRequest("Maintenance Mode");
        }
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try {
            var loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var validator = await _viewCreateValidator.ValidateAsync(view);
            if (!validator.IsValid) {
                return BadRequest(string.Join(",", ResponseHelper.HandlerErrorResponse(validator)));
            }
            var verifyRecord = await Mediator.Send(view.ToProcessorEntityVerify(loggedUserId!));
            return verifyRecord ? BadRequest("View already exist") : Ok(await Mediator.Send(view.ToProcessorEntity(loggedUserId!)));
        }
        catch (Exception e) {
            Log.Logger.Error(e, e.Message);
            return Error(e.Message);
        }
    }

    [Mutation("updateView", typeof(View))]
    [Description("Update a view (will not update columns)")]
    public async Task<IGraphActionResult> UpdateView(
        [FromGraphQL("view")] [Description("view")]
        ViewUpdateInput view) {
        if (IsMaintenanceMode()) {
            return BadRequest("Maintenance Mode");
        }
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try {
            var loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var validator = await _viewUpdateValidator.ValidateAsync(view);
            if (!validator.IsValid) {
                return BadRequest(string.Join(",", ResponseHelper.HandlerErrorResponse(validator)));
            }
            var response = await Mediator.Send(view.ToProcessorEntity(loggedUserId!));
            return Ok(response);
        }
        catch (Exception e) {
            Log.Logger.Error(e, e.Message);
            return Error(e.Message);
        }
    }

    [Mutation("deleteView", typeof(bool))]
    [Description("Delete a view")]
    public async Task<IGraphActionResult> DeleteView(
        [FromGraphQL("viewClientKey")] [Description("view client key to delete")]
        Guid viewClientKey) {
        if (IsMaintenanceMode()) {
            return BadRequest("Maintenance Mode");
        }
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try {
            if (viewClientKey == Guid.Empty) {
                return BadRequest("ViewClientKey is required");
            }
            var loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await Mediator.Send(new DeleteViewRequest {
                UserId = loggedUserId!,
                ViewClientKey = viewClientKey
            }));
        }
        catch (Exception e) {
            Log.Logger.Error(e, e.Message);
            return Error(e.Message);
        }
    }

    [Mutation("updateViewColumns", typeof(View))]
    [Description("Update a view columns")]
    public async Task<IGraphActionResult> UpdateViewColumns(
        [FromGraphQL("view")] [Description("view")]
        ViewUpdateColumnsInput view) {
        if (IsMaintenanceMode()) {
            return BadRequest("Maintenance Mode");
        }
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try {
            var loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var validator = await _viewUpdateColumnsValidator.ValidateAsync(view);
            if (!validator.IsValid) {
                return BadRequest(string.Join(",", ResponseHelper.HandlerErrorResponse(validator)));
            }
            return Ok(await Mediator.Send(view.ToProcessorEntity(loggedUserId!)));
        }
        catch (Exception e) {
            Log.Logger.Error(e, e.Message);
            return Error(e.Message);
        }
    }
    
    [Mutation("updateViewFilters", typeof(View))]
    [Description("Update a view filters")]
    public async Task<IGraphActionResult> UpdateViewFilters(
        [FromGraphQL("view")] [Description("view")]
        ViewFilterUpdateInput view) {
        if (IsMaintenanceMode()) {
            return BadRequest("Maintenance Mode");
        }
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try {
            var loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var validator = await _viewFilterUpdateValidator.ValidateAsync(view);
            if (!validator.IsValid) {
                return BadRequest(string.Join(",", ResponseHelper.HandlerErrorResponse(validator)));
            }
            return Ok(await Mediator.Send(view.ToProcessorEntity(loggedUserId!)));
        }
        catch (Exception e) {
            Log.Logger.Error(e, e.Message);
            return Error(e.Message);
        }
    }

    // [Mutation("setDefaultView", typeof(bool))]
    // [Description("Set default view")]
    // public async Task<IGraphActionResult> SetDefaultView(
    //     [FromGraphQL("viewClientKey")] [Description("view client key to set as default")]
    //     Guid viewClientKey) {
    //     return null;
    // }
}