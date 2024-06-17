using Application.Configs;
using Application.Configs.Providers;
using Application.Constraints;
using Application.Exceptions;
using Application.Utils;
using Domain.Entities;
using Domain.Persistence.Repositories.Interfaces;
using EasyCaching.Core;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.User;
using Infrastructure.Responses.Controllers.Auth;
using Infrastructure.Utils.SMS;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;
namespace Processor.Services.User;

public class RequestOtpHandler : IRequestHandler<SendOTPRequest, RequestOTPResponse> {
    private readonly IActivityLogRepository _activityLogRepository;
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly IRedisCachingProvider _cache;
    private readonly BackendApplicationConfig _config;
    private readonly ICountriesRepository _countriesRepository;
    private readonly InfoUSmsConfig _infoUSmsConfig;
    private readonly ILogger _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IMediator _mediator;
    private readonly IOTPRepository _otpRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public RequestOtpHandler(
        IMediator mediator,
        ILoggerFactory logger,
        IEasyCachingProviderFactory factory,
        IOTPRepository otpRepository,
        IApplicationUserRepository applicationUserRepository,
        IActivityLogRepository activityLogRepository,
        ICountriesRepository countriesRepository,
        UserManager<ApplicationUser> userManager,
        BackendApplicationConfig config,
        InfoUSmsConfig infoUSmsConfig) {
        _otpRepository = otpRepository;
        _countriesRepository = countriesRepository;
        _cache = factory.GetRedisProvider(Cache.ProviderName);
        _applicationUserRepository = applicationUserRepository;
        _logger = logger.CreateLogger<RequestOtpHandler>();
        _mediator = mediator;
        _userManager = userManager;
        _activityLogRepository = activityLogRepository;
        _loggerFactory = logger;
        _infoUSmsConfig = infoUSmsConfig;
        _config = config;
    }

    public async Task<RequestOTPResponse> Handle(SendOTPRequest request, CancellationToken cancellationToken) {
        _logger.LogInformation($"RequestOtpHandler: [{request.Provider},{request.CountryId}, {request.PhoneNumber}]");
        _logger.LogInformation("Start Prep for Sending SMS OTP");
        _logger.LogInformation($"Locating User By Phone: {request.CountryId}-{request.PhoneNumber}");
        Domain.Entities.Countries? country = null;
        if (IsIdentifyByPhoneNumber(request)) {
            var cacheKey = await _cache.KeyExistsAsync(Cache.GetKey($"COUNTRY_{request.CountryId}"));
            if (cacheKey) {
                var payload = await _cache.StringGetAsync(Cache.GetKey($"COUNTRY_{request.CountryId}"));
                country = JsonConvert.DeserializeObject<Domain.Entities.Countries>(payload);
            }
            if (country == null) {
                country = await _countriesRepository.GetById(request.CountryId!.Value);
                if (country == null) {
                    Log.Logger.Warning("Country not found");
                    throw new EntityNotFoundException("Country", request.CountryId!.Value.ToString());
                }
                await _cache.StringSetAsync(Cache.GetKey($"COUNTRY_{request.CountryId}"), country.ToJson(), TimeSpan.FromDays(1));
            }
            if (country.Provider == null)
                throw new MFAProviderNotImplementedException(country.Id);
        }
        var user = await LocateUser(request);
        if (user == null) {
            throw new EntityNotFoundException("User", $"{request.CountryId}-{request.PhoneNumber}");
        }
        if (user.RegisterCompletedAt == null) {
            throw new UserNotCompletedRegistrationException(user.Id);
        }
        if (request.Provider == MFAProvider.SMS && country is { Provider: not null }) {
            LoadProvider(country, country.Provider.Value);
        }
        if (request.ReSend) {
            return await HandleResend(user, request);
        }
        return await SendOTP(user, request);
    }

    private bool IsIdentifyByEmailAddress(SendOTPRequest request) {
        return !string.IsNullOrEmpty(request.Email) && request.CountryId == null && string.IsNullOrEmpty(request.PhoneNumber);
    }

    private bool IsIdentifyByPhoneNumber(SendOTPRequest request) {
        return !string.IsNullOrEmpty(request.PhoneNumber) && request.CountryId != null && string.IsNullOrEmpty(request.Email);
    }

    private void LoadProvider(Domain.Entities.Countries country, SMSProviders provider) {
        _logger.LogInformation($"Loading Sms Provider: {provider}");
        switch (provider) {
            case SMSProviders.INFOU:
                SMSSender.Instance.Load(_loggerFactory, _infoUSmsConfig);
                break;
            default:
                throw new MFAProviderNotImplementedException(country.Id, Enum.GetName(provider)!);
        }
    }

    private async Task<ApplicationUser> LocateUser(SendOTPRequest request) {
        ApplicationUser? user = null;
        if (IsIdentifyByEmailAddress(request)) {
            user = await _applicationUserRepository.LocateUserByEmail(request.Email!);
        } else if (IsIdentifyByPhoneNumber(request)) {
            user = await _applicationUserRepository.LocateUserByPhone(request.CountryId!.Value, request.PhoneNumber!);
        }
        if (user == null)
            throw new EntityNotFoundException("User", $"{request.CountryId}-{request.PhoneNumber}");

        return user;
    }


    private async Task<RequestOTPResponse> HandleResend(ApplicationUser user, SendOTPRequest request) {
        _logger.LogInformation($"Verify OTP Entity for user {user.Id} and check if user will able to resend");
        var totalCodesSent = await _applicationUserRepository.GetTotalOtpCodesWithinExpirationDate(new Guid(user.Id), _config.OTPCodeTotalRetriesWithinSession);

        if (totalCodesSent >= _config.OTPCodeTotalRetriesWithinSession) {
            _logger.LogInformation($"User {user.Id} has reached max resend limit");
            return await FetchOtpResponse(user);
        }

        return await SendOTP(user, request);
    }

    private async Task<RequestOTPResponse> SendOTP(ApplicationUser user, SendOTPRequest request) {
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
            _logger.LogInformation($"User {user.Id} has reached max resend limit");
            user.LockoutEnd = SystemClock.Now().AddMinutes(10);
            await _userManager.UpdateAsync(user);
            await _activityLogRepository.AddActivity(Guid.Parse(user.Id), user.Id, nameof(ApplicationUserOtpCodes), ActivityLogOperationType.UserSendOTP,
                "User Request Otp Code and been locked out for 10 minutes",
                $"User Request Otp Code and been locked out for 10 minutes , [{user.Id}, {user.Email}]",
                ActivityStatus.Failed, request.IpAddress, request.UserAgent);
            throw new UserReachedMaxResendLimitException(user.Id);

        }
        var forceResent = request.ReSend &&
                          await _applicationUserRepository.IsAllowToResendOtpCode(user, expiredCodeInMinutes, totalRetries);
        var otpEntity = await _applicationUserRepository.GenerateOtpCode(user, expiredCodeInMinutes, forceResent, request.Provider);
        if (otpEntity == null) {
            _logger.LogInformation($"User {user.Id} has within the expiration date and won't able to sent or been reached max resend limit");
            return await FetchOtpResponse(user);
        }
        _logger.LogInformation($"OTP Entity for user {user.Id} has been generated otp entity id {otpEntity.Id}");
        var result = await _otpRepository.RequestOTP(request.BaseUrl, request.BaseUrl + "/assets", user.Country, user, otpEntity, request.Provider);
        _logger.LogInformation($"Sendt OTP for user {user.Id}");
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