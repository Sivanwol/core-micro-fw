using Application.Configs;
using Application.Utils.Service;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Requests.Account.Auth;
using Infrastructure.Requests.Account.Backoffice;
using Infrastructure.Services.Auth;
using Infrastructure.Services.Auth.Sender;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace BackendApi.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class MobileAuthController : BaseApiController {
    private readonly IEmailSender _emailSender;
    private readonly IValidator<ForgetPasswordRequest> _forgetPasswordValidator;
    private readonly IJwtAuthManager _jwtAuthManager;
    private readonly ILogger _logger;
    private readonly IValidator<MobileLoginUserRequest> _loginValidator;
    private readonly IValidator<MobileRequestOTPUserRequest> _requestOTPValidator;
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
        IValidator<MobileRequestOTPUserRequest> requestOtpValidator) : base(mediator, configuration) {
        _emailSender = emailSender;
        _smsSender = smsSender;
        _logger = loggerFactory.CreateLogger<MobileAuthController>();
        _jwtAuthManager = jwtAuthManager;
        _requestOTPValidator = requestOtpValidator;
    }
    // [HttpGet("request_otp_code")]
    // [SwaggerOperation("Request Mobile user OTP", "Send OTP Code for mobile user", Tags = new[] {
    //     "Users",
    //     "Auth"
    // })]
    // public async Task<IActionResult> RequestOTPCode() {
    //     return;
    // }
}