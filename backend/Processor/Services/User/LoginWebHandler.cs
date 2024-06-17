using Application.Configs;
using Application.Configs.General;
using Application.Configs.Providers;
using Application.Exceptions;
using Application.Utils;
using Domain.Entities;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.User;
using Infrastructure.Responses.Controllers.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Serilog;
namespace Processor.Services.User;

public class LoginWebHandler : IRequestHandler<LoginWebUserRequest, RequestOTPResponse> {
    private readonly IActivityLogRepository _activityLogRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly BackendApplicationConfig _config;
    private readonly FrontEndPaths _frontEndPaths;
    private readonly InfoUSmsConfig _infoUSmsConfig;
    private readonly IMediator _mediator;
    private readonly IOTPRepository _otpRepository;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginWebHandler(IMediator mediator,
        BackendApplicationConfig config,
        IOptions<FrontEndPaths> frontEndPaths,
        IApplicationUserRepository applicationUserRepository,
        IOTPRepository otpRepository,
        UserManager<ApplicationUser> userManager,
        InfoUSmsConfig infoUSmsConfig,
        IActivityLogRepository activityLogRepository,
        SignInManager<ApplicationUser> signInManager) {
        _mediator = mediator;
        _signInManager = signInManager;
        _config = config;
        _infoUSmsConfig = infoUSmsConfig;
        _frontEndPaths = frontEndPaths.Value;
        _userManager = userManager;
        _activityLogRepository = activityLogRepository;
        _otpRepository = otpRepository;
        _applicationUserRepository = applicationUserRepository;
    }

    public async Task<RequestOTPResponse> Handle(LoginWebUserRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"LogoutUserHandler: [{request.Email}]");
        var user = await _userManager.FindByEmailAsync(request.Email);
        // user not found
        if (user?.DeletedAt != null) {
            Log.Logger.Warning("User account not found");
            throw new UserNotFoundException(request.Email);
        }
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, !_config.DeveloperMode);
        // user account got disabled
        if (result.IsNotAllowed) {
            Log.Logger.Warning("User account is disabled - {UserId}", user.Id);
            throw new UserBlockException();
        }

        // user locked out
        if (result.IsLockedOut) {
            Log.Logger.Warning("User account locked out - {UserId}", user.Id);
            throw new UserLockedoutException();
        }
        // bad password
        if (!result.Succeeded) {
            Log.Logger.Warning("Invalid login attempt - {UserId}", user.Id);
            throw new InvalidLoginException();
        }
        if (user.RegisterCompletedAt == null) {
            Log.Logger.Warning("User account not completed registration - {UserId}", user.Id);
            await _activityLogRepository.AddActivity(
                Guid.Parse(user.Id),
                user.Id,
                nameof(ApplicationUser),
                ActivityLogOperationType.UserLogin,
                "User Request Login",
                $"User Request Login - {user.Id}",
                ActivityStatus.Failed,
                request.IpAddress,
                request.UserAgent);
            throw new UserNotCompletedRegistrationException(user.Id);
        }
        await _activityLogRepository.AddActivity(
            Guid.Parse(user.Id),
            user.Id,
            nameof(ApplicationUser),
            ActivityLogOperationType.UserLogin,
            "User Request Login",
            $"User Request Login - {user.Id}",
            ActivityStatus.Success,
            request.IpAddress,
            request.UserAgent);
        Log.Logger.Information("User logged in -  {UserId}", user.Id);
        return await SendOTP(user, request);
    }

    private async Task<RequestOTPResponse> SendOTP(ApplicationUser user, LoginWebUserRequest request) {
        var totalRetries = _infoUSmsConfig.TotalRetriesWithinSession;
        var expiredCodeInMinutes = _infoUSmsConfig.ExpiredInMinutes;
        if (request.Provider == MFAProvider.Email) {
            expiredCodeInMinutes = _config.OTPCodeExpiredInMinutes;
            totalRetries = _config.OTPCodeTotalRetriesWithinSession;
        }
        if (_config is { DeveloperMode: true, DeveloperModeEnabledOTP: true }) {
            expiredCodeInMinutes = _config.OTPCodeExpiredInMinutes * 24;
            totalRetries = _config.OTPCodeTotalRetriesWithinSession * 24;
        }
        var currentTotalCodesSent = await _applicationUserRepository.GetTotalOtpCodesWithinExpirationDate(new Guid(user.Id), totalRetries);
        if (currentTotalCodesSent >= totalRetries) {
            Log.Logger.Warning($"User {user.Id} has reached max resend limit");
            user.LockoutEnd = SystemClock.Now().AddMinutes(10);
            await _userManager.UpdateAsync(user);
            await _activityLogRepository.AddActivity(Guid.Parse(user.Id), user.Id, nameof(ApplicationUserOtpCodes), ActivityLogOperationType.UserSendOTP,
                "User Request Otp Code and been locked out for 10 minutes",
                $"User Request Otp Code and been locked out for 10 minutes , [{user.Id}, {user.Email}]",
                ActivityStatus.Failed, request.IpAddress, request.UserAgent);
            throw new UserReachedMaxResendLimitException(user.Id);

        }
        var forceResent = request.ReSend &&
                          await _applicationUserRepository.IsAllowToResendOtpCode(user, _infoUSmsConfig.ExpiredInMinutes, _infoUSmsConfig.TotalRetriesWithinSession);
        var otpEntity = await _applicationUserRepository.GenerateOtpCode(user, expiredCodeInMinutes, forceResent, request.Provider);
        if (otpEntity == null) {
            Log.Logger.Information($"User {user.Id} has within the expiration date and won't able to sent or been reached max resend limit");
            return await FetchOtpResponse(user);
        }
        Log.Logger.Information($"OTP Entity for user {user.Id} has been generated otp entity id {otpEntity.Id}");
        var result = await _otpRepository.RequestOTP(_frontEndPaths.PlatformDashboardUrl, $"{_frontEndPaths.PlatformDashboardUrl}/assets", user.Country, user, otpEntity,
            request.Provider);
        Log.Logger.Information($"Sendt OTP for user {user.Id}");
        await _activityLogRepository.AddActivity(Guid.Parse(user.Id), user.Id, nameof(ApplicationUserOtpCodes), ActivityLogOperationType.UserSendOTP,
            $"User Request Otp Code and been sent via {request.Provider}",
            $"User Request and sent to him , [{user.Id}, {otpEntity.Token}]",
            ActivityStatus.Success, request.IpAddress, request.UserAgent);
        return new RequestOTPResponse {
            UserToken = result.UserToken,
            OTPExpired = result.OTPExpired,
            OTPToken = result.OTPToken,
            HasSent = true
        };
    }
    private async Task<RequestOTPResponse> FetchOtpResponse(ApplicationUser user) {
        var otpResult = await _applicationUserRepository.FetchOTP(user);
        if (otpResult != null) {
            return new RequestOTPResponse {
                UserToken = user.Token,
                OTPExpired = otpResult.ExpirationDate,
                OTPToken = otpResult.Token,
                HasSent = false
            };
        }
        return new RequestOTPResponse {
            UserToken = "",
            OTPExpired = SystemClock.Now(),
            OTPToken = "",
            HasSent = false
        };
    }
}