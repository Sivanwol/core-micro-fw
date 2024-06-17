using Application.Constraints;
using Application.Exceptions;
using Application.Responses.Base;
using Domain.Entities;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.Requests.Processor.Services.Vendor;
using Infrastructure.Services.S3;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace Processor.Services.Vendor;

public class DeleteVendorHandler : IRequestHandler<DeleteVendorRequest, EmptyResponse>
{
    private readonly IActivityLogRepository _activityRepository;
    private readonly IMediator _mediator;
    private readonly IVendorRepository _vendorRepository;
    private readonly IStorageControlService _storageControlService;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    public DeleteVendorHandler(IMediator mediator,
    IStorageControlService storageControlService,
    IActivityLogRepository activityRepository,
    IApplicationUserRepository applicationUserRepository,
    UserManager<ApplicationUser> userManager,
    IVendorRepository vendorRepository)
    {
        _storageControlService = storageControlService;
        _mediator = mediator;
        _vendorRepository = vendorRepository;
        _activityRepository = activityRepository;
        _userManager = userManager;
        _applicationUserRepository = applicationUserRepository;
    }

    public async Task<EmptyResponse> Handle(DeleteVendorRequest request, CancellationToken cancellationToken)
    {
        Log.Logger.Information($"DeleteVendorHandler: {request.VendorId}");
        if (!await _vendorRepository.HasExist(request.VendorId))
        {
            throw new EntityNotFoundException("vendors", request.VendorId);
        }
        await ValidateRequest(request);
        await _vendorRepository.DeleteVendors(new List<int> { request.VendorId });
        return new EmptyResponse();
    }

    private async Task ValidateRequest(DeleteVendorRequest request)
    {
        if (Guid.Empty == request.LoggedInUserId)
        {
            Log.Logger.Error($"DeleteVendorHandler: Logged in User Id is Invalid");
            throw new EntityNotFoundException(nameof(ApplicationUser), request.LoggedInUserId.ToString());
        }
        var loggedUser = await _applicationUserRepository.GetById(request.LoggedInUserId);
        if (loggedUser == null)
        {
            Log.Logger.Error($"DeleteVendorHandler: Logged user not found");
            throw new EntityNotFoundException(nameof(ApplicationUser), request.LoggedInUserId.ToString());
        }
        var loggedRoles = await _userManager.GetRolesAsync(loggedUser);
        var matchedRoles = new List<string>() {
            Roles.Admin
        };
        if (!matchedRoles.Any(x => loggedRoles.Contains(x)))
        {
            Log.Logger.Error($"DeleteVendorHandler: User not authorized");
            throw new AuthorizationException();
        }
    }
}
