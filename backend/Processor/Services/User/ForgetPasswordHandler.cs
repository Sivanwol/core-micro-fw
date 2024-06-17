using Application.Configs;
using Application.Configs.Providers;
using Application.Exceptions;
using Application.Utils;
using Domain.Entities;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.User;
using Infrastructure.Responses.Controllers.Auth;
using Infrastructure.Utils.SMS;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;
namespace Processor.Services.User;

public class ForgetPasswordHandler : IRequestHandler<ForgotPasswordRequest, RequestOTPResponse> {
    private readonly IApplicationUserRepository _applicationUserRepository;
    private readonly BackendApplicationConfig _config;
    private readonly ICountriesRepository _countriesRepository;
    private readonly InfoUSmsConfig _infoUSmsConfig;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IMediator _mediator;
    private readonly IOTPRepository _otpRepository;

    public ForgetPasswordHandler(IMediator mediator,
        IApplicationUserRepository applicationUserRepository,
        ILoggerFactory logger,
        ICountriesRepository countriesRepository,
        IOTPRepository otpRepository,
        InfoUSmsConfig infoUSmsConfig,
        BackendApplicationConfig config) {
        _mediator = mediator;
        _applicationUserRepository = applicationUserRepository;
        _countriesRepository = countriesRepository;
        _config = config;
        _otpRepository = otpRepository;
        _loggerFactory = logger;
        _infoUSmsConfig = infoUSmsConfig;
    }

    public async Task<RequestOTPResponse> Handle(ForgotPasswordRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information($"RequestOtpHandler: [{request.Provider},{request.CountryId}, {request.PhoneNumber}]");
        Log.Logger.Information("Start Prep for Sending SMS OTP");
        Log.Logger.Information($"Locating User By Phone: {request.CountryId}-{request.PhoneNumber}");
        Domain.Entities.Countries? country = null;
        if (IsIdentifyByPhoneNumber(request)) {
            country = await _countriesRepository.GetById(request.CountryId!.Value);
            if (country == null)
                throw new EntityNotFoundException("Country", request.CountryId!.Value.ToString());
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

    private bool IsIdentifyByEmailAddress(ForgotPasswordRequest request) {
        return !string.IsNullOrEmpty(request.Email) && request.CountryId == null && string.IsNullOrEmpty(request.PhoneNumber);
    }

    private bool IsIdentifyByPhoneNumber(ForgotPasswordRequest request) {
        return !string.IsNullOrEmpty(request.PhoneNumber) && request.CountryId != null && string.IsNullOrEmpty(request.Email);
    }

    private void LoadProvider(Domain.Entities.Countries country, SMSProviders provider) {
        Log.Logger.Information($"Loading Sms Provider: {provider}");
        switch (provider) {
            case SMSProviders.INFOU:
                SMSSender.Instance.Load(_loggerFactory, _infoUSmsConfig);
                break;
            default:
                throw new Application.Exceptions.MFAProviderNotImplementedException(country.Id, provider.ToString());
        }
    }

    private async Task<RequestOTPResponse> HandleResend(ApplicationUser user, ForgotPasswordRequest request) {
        Log.Logger.Information($"Verify OTP Entity for user {user.Id} and check if user will able to resend");
        var totalCodesSent = await _applicationUserRepository.GetTotalOtpCodesWithinExpirationDate(new Guid(user.Id), _config.OTPCodeTotalRetriesWithinSession);

        if (totalCodesSent >= _config.OTPCodeTotalRetriesWithinSession) {
            Log.Logger.Information($"User {user.Id} has reached max resend limit");
            return await FetchOtpResponse(user);
        }

        return await SendOTP(user, request);
    }

    private async Task<ApplicationUser> LocateUser(ForgotPasswordRequest request) {
        ApplicationUser? user = null;
        if (IsIdentifyByEmailAddress(request)) {
            user = await _applicationUserRepository.LocateUserByEmail(request.Email);
        } else if (IsIdentifyByPhoneNumber(request)) {
            user = await _applicationUserRepository.LocateUserByPhone(request.CountryId!.Value, request.PhoneNumber!);
        }
        if (user == null)
            throw new EntityNotFoundException("User", $"{request.CountryId}-{request.PhoneNumber}");

        return user;
    }

    private async Task<RequestOTPResponse> SendOTP(ApplicationUser user, ForgotPasswordRequest request) {
        var forceResent = request.ReSend &&
                          await _applicationUserRepository.IsAllowToResendOtpCode(user, _config.OTPCodeExpiredInMinutes, _config.OTPCodeTotalRetriesWithinSession);
        var otpEntity = await _applicationUserRepository.GenerateOtpCode(user, _config.OTPCodeExpiredInMinutes, forceResent, request.Provider);
        if (otpEntity == null) {
            Log.Logger.Information($"User {user.Id} has within the expiration date and won't able to sent or been reached max resend limit");
            return await FetchOtpResponse(user);
        }
        Log.Logger.Information($"OTP Entity for user {user.Id} has been generated otp entity id {otpEntity.Id}");
        var result = await _otpRepository.RequestOTPForgetPassword(request.BaseUrl, request.BaseUrl + "/assets", user.Country, user, otpEntity, request.Provider);
        Log.Logger.Information($"Sendt OTP for user {user.Id}");
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