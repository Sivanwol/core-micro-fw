using Application.Configs;
using Application.Utils.Service;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Requests.Account;
using Infrastructure.Services.Auth;
using Infrastructure.Services.Auth.Sender;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace FrontApi.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class MobileAuthController : BaseApiController {
    private readonly IEmailSender _emailSender;
    private readonly IValidator<ForgetPasswordRequest> _forgetPasswordValidator;
    private readonly IJwtAuthManager _jwtAuthManager;
    private readonly ILogger _logger;
    private readonly IValidator<LoginRequest> _loginValidator;
    private readonly IValidator<RefreshTokenRequest> _refreshTokenValidator;
    private readonly IValidator<RegisterNewUserRequest> _registerNewUserValidator;
    private readonly IValidator<ResetPasswordRequest> _resetPasswordValidator;
    private readonly IValidator<SendCodeToProviderRequest> _sendCodeToProviderValidator;
    private readonly ISmsSender _smsSender;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IValidator<VerifyFromProviderRequest> _verifyFromProviderValidator;

    public MobileAuthController(
        IMediator mediator,
        IEmailSender emailSender,
        ISmsSender smsSender,
        ILoggerFactory loggerFactory,
        IJwtAuthManager jwtAuthManager,
        BackendApplicationConfig configuration,
        IValidator<ResetPasswordRequest> resetPasswordValidator,
        IValidator<ForgetPasswordRequest> forgetPasswordValidator,
        IValidator<LoginRequest> loginValidator,
        IValidator<RefreshTokenRequest> refreshTokenValidator,
        IValidator<VerifyFromProviderRequest> verifyFromProviderValidator,
        IValidator<SendCodeToProviderRequest> sendCodeToProviderValidator,
        IValidator<RegisterNewUserRequest> registerNewUserValidator) : base(mediator, configuration) {
        _emailSender = emailSender;
        _smsSender = smsSender;
        _logger = loggerFactory.CreateLogger<AuthController>();
        _jwtAuthManager = jwtAuthManager;
        _registerNewUserValidator = registerNewUserValidator;
        _resetPasswordValidator = resetPasswordValidator;
        _forgetPasswordValidator = forgetPasswordValidator;
        _loginValidator = loginValidator;
        _sendCodeToProviderValidator = sendCodeToProviderValidator;
        _refreshTokenValidator = refreshTokenValidator;
        _verifyFromProviderValidator = verifyFromProviderValidator;
    }
}