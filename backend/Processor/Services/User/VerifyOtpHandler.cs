using System.Security.Claims;
using Application.Configs;
using Application.Exceptions;
using Application.Utils;
using Domain.DTO.OTP;
using Domain.Entities;
using Domain.Persistence.Repositories.Interfaces;
using Domain.Requests.Processor.Services.User;
using Infrastructure.Enums;
using Infrastructure.Responses.Controllers.Auth;
using Infrastructure.Services.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serilog;
namespace Processor.Services.User;

public class VerifyOtpHandler : IRequestHandler<VerifyOtpUserRequest, VerifyOTPResponse> {
    private readonly IActivityLogRepository _activityLogRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly BackendApplicationConfig _config;
    private readonly ICountriesRepository _countriesRepository;
    private readonly IJwtAuthManager _jwtAuthManager;
    private readonly IMediator _mediator;
    private readonly IOTPRepository _otpRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public VerifyOtpHandler(IMediator mediator,
        IJwtAuthManager jwtAuthManager,
        IApplicationUserRepository applicationUserRepository,
        IActivityLogRepository activityLogRepository,
        ICountriesRepository countriesRepository,
        IOTPRepository otpRepository,
        UserManager<ApplicationUser> userManager,
        BackendApplicationConfig config) {
        _otpRepository = otpRepository;
        _applicationUserRepository = applicationUserRepository;
        _countriesRepository = countriesRepository;
        _jwtAuthManager = jwtAuthManager;
        _userManager = userManager;
        _activityLogRepository = activityLogRepository;
        _mediator = mediator;
        _config = config;
    }

    public async Task<VerifyOTPResponse> Handle(VerifyOtpUserRequest request, CancellationToken cancellationToken) {
        var user = await GetValidUserByToken(request.UserToken);
        var expiredCodeInMinutes = _config.OTPCodeExpiredInMinutes;
        var otpEntity = await GetOtpEntity(request, expiredCodeInMinutes, user);
        var result = await _otpRepository.VerifyOTP(user, otpEntity, request.Code);

        Guid? userId = result.IsValid ? new Guid(user.Id) : null;
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim> {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new("TenantId", ""),
        };
        roles.ToList().ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r)));
        if (_config is { DeveloperMode: true, DeveloperModeEnabledOTP: false }) {
            await RecordActivity(request, user, result, userId!.Value);
            return new VerifyOTPResponse {
                UserId = userId,
                Tokens = _jwtAuthManager.GenerateTokens(request.UserToken, claims.ToArray(), SystemClock.Now())
            };
        }
        if (result.IsValid) {
            Log.Logger.Information($"Clearing OTP Codes for User: [{userId}]");
            await _applicationUserRepository.ClearOtpCodes(Guid.Parse(user.Id));
            if (otpEntity.ProviderType == MFAProvider.Email) {
                if (!user.EmailConfirmed) {
                    Log.Logger.Information($"Confirming Email for User: [{userId}]");
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                }
            } else {
                if (!user.PhoneNumberConfirmed) {
                    Log.Logger.Information($"Confirming Phone Number for User: [{userId}]");
                    user.PhoneNumberConfirmed = true;
                    await _userManager.UpdateAsync(user);
                }
            }
        }
        await RecordActivity(request, user, result, userId!.Value);
        // await _signInManager.SignInAsync(user, true);
        return new VerifyOTPResponse {
            UserId = userId,
            Tokens = result.IsValid
                ? _jwtAuthManager.GenerateTokens(request.UserToken, claims.ToArray(), SystemClock.Now())
                : null
        };
    }

    private async Task RecordActivity(VerifyOtpUserRequest request, ApplicationUser user, VerifyOTPResponseData result, Guid userId) {

        if (request.ActionOtpType == ActionOtpType.Login) {
            await _activityLogRepository.AddActivity(userId, user.Id, nameof(ApplicationUserOtpCodes), ActivityLogOperationType.UserVerifyOTP, "User Received access tokens",
                "User Request origin Logged In",
                result.IsValid ? ActivityStatus.Success : ActivityStatus.Failed, request.IpAddress, request.UserAgent);
            return;
        }
        await _activityLogRepository.AddActivity(userId, user.Id, nameof(ApplicationUserOtpCodes), ActivityLogOperationType.UserVerifyOTP, "User Received access tokens",
            "User Request origin Forgot Password",
            result.IsValid ? ActivityStatus.Success : ActivityStatus.Failed, request.IpAddress, request.UserAgent);
    }

    private async Task<ApplicationUser> GetValidUserByToken(string userToken) {
        Log.Logger.Information($"VerifyOtpHandler: [{userToken}]");
        var user = await _applicationUserRepository.GetUserByToken(userToken);
        if (user == null) {
            throw new EntityNotFoundException("User", userToken);
        }
        return user;
    }

    private async Task<ApplicationUserOtpCodes> GetOtpEntity(VerifyOtpUserRequest request, int expiredCodeInMinutes, ApplicationUser user) {
        Log.Logger.Information($"Checking if OTP Entity Exist for User: [{user.Id}, {request.OTPToken}]");
        var otpEntity = await _applicationUserRepository.IsExistingOtpEntity(request.UserToken, request.OTPToken, expiredCodeInMinutes);
        if (otpEntity == null) {
            throw new EntityNotFoundException("UserOtpCodes", request.UserToken);
        }
        return otpEntity;
    }
}