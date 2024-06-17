using Application.Constraints;
using Application.Exceptions;
using Application.Utils;
using Domain.Entities;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.Enums;
using Infrastructure.GQL.Inputs.Media;
using Infrastructure.Requests.Processor.Services.User;
using Infrastructure.Requests.Processor.Services.Vendor;
using Infrastructure.Services.S3;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Processor.Contracts.Vendors;
using Serilog;

namespace Processor.Services.Vendor;

public class CreateVendorHandler : IRequestHandler<CreateVendorRequest, Infrastructure.GQL.Vendor>
{
    private readonly IActivityLogRepository _activityRepository;
    private readonly IMediator _mediator;
    private readonly IVendorRepository _vendorRepository;
    private readonly IStorageControlService _storageControlService;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IBus _bus;
    public CreateVendorHandler(IMediator mediator,
    IStorageControlService storageControlService,
    IActivityLogRepository activityRepository,
    IApplicationUserRepository applicationUserRepository,
    UserManager<ApplicationUser> userManager,
    IBus bus,
    IVendorRepository vendorRepository)
    {
        _storageControlService = storageControlService;
        _mediator = mediator;
        _vendorRepository = vendorRepository;
        _activityRepository = activityRepository;
        _userManager = userManager;
        _applicationUserRepository = applicationUserRepository;
        _bus = bus;
    }

    public async Task<Infrastructure.GQL.Vendor> Handle(CreateVendorRequest request, CancellationToken cancellationToken)
    {
        Log.Logger.Information($"CreateVendorHandler: {request.Name} , {request.CountryId}");
        if (await _vendorRepository.HasVendorExist(request.UserId, request.Name, request.CountryId))
        {
            throw new EntityFoundException("vendors", $"[{request.Name} , {request.CountryId}]");
        }
        await ValidateRequest(request);
        await _activityRepository.AddActivity(request.LoggedInUserId, request.LoggedInUserId, "", nameof(Domain.Entities.Vendors), ActivityLogOperationType.VendorCreate,
            "Request to create vendor", $"request create vendor bt this request {JsonConvert.SerializeObject(request)}", ActivityStatus.Success, request.IpAddress,
            request.UserAgent);
        var files = new List<ImageMedia>
        {
            new ImageMedia() { Media = request.Logo }
        };
        var media = await _mediator.Send(new UploadMediaRequest()
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
        var vendor = await _vendorRepository.CreateVendor(request, media.First().Id);
        await _bus.Publish<VendorCreatedInitial>(new { VendorId = vendor.Id, UserId = request.UserId }, x => x.ResponseAddress = _bus.Address);
        return vendor.ToGql();
    }

    private async Task ValidateRequest(CreateVendorRequest request)
    {
        if (Guid.Empty == request.LoggedInUserId)
        {
            Log.Logger.Error($"CreateVendorHandler: Logged in User Id is Invalid");
            throw new EntityNotFoundException(nameof(ApplicationUser), request.LoggedInUserId.ToString());
        }
        var loggedUser = await _applicationUserRepository.GetById(request.LoggedInUserId);
        if (loggedUser == null)
        {
            Log.Logger.Error($"CreateVendorHandler: Logged user not found");
            throw new EntityNotFoundException(nameof(ApplicationUser), request.LoggedInUserId.ToString());
        }
        var loggedRoles = await _userManager.GetRolesAsync(loggedUser);
        var matchedRoles = new List<string>() {
            Roles.Admin
        };
        if (!matchedRoles.Any(x => loggedRoles.Contains(x)))
        {
            Log.Logger.Error($"CreateVendorHandler: User not authorized");
            throw new AuthorizationException();
        }
    }
}
