using Application.Configs;
using Application.Exceptions;
using Application.Utils;
using Domain.Entities;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.User;
using MediatR;
using Serilog;
namespace Processor.Services.User;

public class UpdateUserProfileHandler : IRequestHandler<UpdateUserProfileRequest, Infrastructure.GQL.User> {
    private readonly IActivityLogRepository _activityLogRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly ICountriesRepository _countriesRepository;
    private readonly BackendApplicationConfig _config;
    public UpdateUserProfileHandler(IApplicationUserRepository applicationUserRepository,
        IActivityLogRepository activityLogRepository,
        ICountriesRepository countriesRepository,
        BackendApplicationConfig config) {
        _countriesRepository = countriesRepository;
        _applicationUserRepository = applicationUserRepository;
        _activityLogRepository = activityLogRepository;
        _config = config;
    }
    public async Task<Infrastructure.GQL.User> Handle(UpdateUserProfileRequest request, CancellationToken cancellationToken) {
        try {
            var country = await _countriesRepository.GetById(request.CountryId);
            if (country == null) {
                throw new EntityNotFoundException("Country", request.CountryId);
            }
            
            await _applicationUserRepository.UpdateMyProfile(request.UserId, request.PhoneNumber, request.FirstName, request.LastName, country!, request.Address);
            await _activityLogRepository.AddActivity(request.UserId, request.UserId.ToString(), nameof(ApplicationUser), ActivityLogOperationType.UserUpdate, "User Update Profile",
                $"User Update Profile {request.UserId} been updated successfully", ActivityStatus.Success, request.IpAddress, request.UserAgent);
            
            var user = await _applicationUserRepository.GetById(request.UserId);
            return user!.ToGql();
        }
        catch (Exception e) {
            Log.Logger.Error(e, e.Message);
            await _activityLogRepository.AddActivity(request.UserId, request.UserId.ToString(), nameof(ApplicationUserPreferences), ActivityLogOperationType.UserUpdatePreference, "User Update Profile",
                $"User Update Profile {request.UserId} not been updated", ActivityStatus.Failed, request.IpAddress, request.UserAgent);
            return new Infrastructure.GQL.User();
        }
    }
}