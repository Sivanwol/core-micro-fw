using Application.Configs;
using Application.Responses.Base;
using Application.Utils;
using Domain.Entities;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.User;
using MediatR;
namespace Processor.Services.User;

public class ResetUserPreferenceHandler : IRequestHandler<ResetUserPreferenceRequest,EmptyResponse> {
    private readonly IActivityLogRepository _activityLogRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly BackendApplicationConfig _config;
    public ResetUserPreferenceHandler(IApplicationUserRepository applicationUserRepository,
        IActivityLogRepository activityLogRepository,
        BackendApplicationConfig config) {
        _applicationUserRepository = applicationUserRepository;
        _activityLogRepository = activityLogRepository;
        _config = config;
    }
    public async Task<EmptyResponse> Handle(ResetUserPreferenceRequest request, CancellationToken cancellationToken) {
        await _applicationUserRepository.ResetUserPreference(request.UserId);
        await _activityLogRepository.AddActivity(request.UserId, request.UserId.ToString(), nameof(ApplicationUserPreferences), ActivityLogOperationType.UserResetPreference, "User Update Preference",
            "User Update Preference been reset successfully", ActivityStatus.Success, request.IpAddress, request.UserAgent);
        return new EmptyResponse();
    }
}