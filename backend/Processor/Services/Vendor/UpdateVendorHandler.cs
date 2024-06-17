using Application.Constraints;
using Application.Exceptions;
using Application.Utils;
using Domain.Entities;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.Enums;
using Infrastructure.GQL;
using Infrastructure.GQL.Inputs.Media;
using Infrastructure.Requests.Processor.Services.User;
using Infrastructure.Requests.Processor.Services.Vendor;
using Infrastructure.Services.S3;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Serilog;

namespace Processor.Services.Vendor;

public class UpdateVendorHandler : IRequestHandler<UpdateVendorRequest, Infrastructure.GQL.Vendor>
{
    private readonly IActivityLogRepository _activityRepository;
    private readonly IMediator _mediator;
    private readonly IVendorRepository _vendorRepository;
    private readonly IStorageControlService _storageControlService;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    public UpdateVendorHandler(IMediator mediator,
    IStorageControlService storageControlService,
    IActivityLogRepository activityRepository,
    IApplicationUserRepository applicationUserRepository,
    UserManager<ApplicationUser> userManager,
    IVendorRepository vendorRepository)
    {
        _storageControlService = storageControlService;
        _mediator = mediator;
        _activityRepository = activityRepository;
        _vendorRepository = vendorRepository;
        _userManager = userManager;
        _applicationUserRepository = applicationUserRepository;
    }

    public async Task<Infrastructure.GQL.Vendor> Handle(UpdateVendorRequest request, CancellationToken cancellationToken)
    {
        Log.Logger.Information($"UpdateVendorHandler: {request.VendorId} , {request.Name} , {request.CountryId}");
        if (!await _vendorRepository.HasExist(request.VendorId))
        {
            throw new EntityNotFoundException("vendors", request.VendorId);
        }
        if (await _vendorRepository.HasUpdateVendorExist(request.UserId, request.VendorId, request.Name, request.CountryId))
        {
            throw new EntityFoundException("vendors", $"[{request.Name} , {request.CountryId}]");
        }
        await ValidateRequest(request);
        IEnumerable<Infrastructure.GQL.Media> media = null;
        if (request.Logo != null)
        {
            var files = new List<ImageMedia>();
            files.Add(new ImageMedia() { Media = request.Logo });
            media = await _mediator.Send(new UploadMediaRequest()
            {
                Files = files,
                IpAddress = request.IpAddress,
                UserAgent = request.UserAgent,
                LoggedInUserId = request.LoggedInUserId,
                IsGlobalFiles = true,
                OperationType = Infrastructure.Enums.UploadOperationType.Vendor
            });
            if (media == null || !media.Any())
            {
                throw new EntityNotFoundException("media", request.Logo.FileName);
            }
        }
        await _activityRepository.AddActivity(request.LoggedInUserId, request.LoggedInUserId, "", nameof(Domain.Entities.Vendors), ActivityLogOperationType.VendorUpdate,
            "Request to update vendor", $"request update vendor bt this request {JsonConvert.SerializeObject(request)}", ActivityStatus.Success, request.IpAddress,
            request.UserAgent);
        await _vendorRepository.UpdateVendor(request, media?.First().Id);
        var vendor = await _vendorRepository.GetVendor(request.VendorId);
        return vendor.ToGql();
    }

    private async Task ValidateRequest(UpdateVendorRequest request)
    {
        if (Guid.Empty == request.LoggedInUserId)
        {
            Log.Logger.Error($"UpdateVendorHandler: Logged in User Id is Invalid");
            throw new EntityNotFoundException(nameof(ApplicationUser), request.LoggedInUserId.ToString());
        }
        var loggedUser = await _applicationUserRepository.GetById(request.LoggedInUserId);
        if (loggedUser == null)
        {
            Log.Logger.Error($"UpdateVendorHandler: Logged user not found");
            throw new EntityNotFoundException(nameof(ApplicationUser), request.LoggedInUserId.ToString());
        }
        var loggedRoles = await _userManager.GetRolesAsync(loggedUser);
        var matchedRoles = new List<string>() {
            Roles.Admin
        };
        if (!matchedRoles.Any(x => loggedRoles.Contains(x)))
        {
            Log.Logger.Error($"UpdateVendorHandler: User not authorized");
            throw new AuthorizationException();
        }
    }
}
