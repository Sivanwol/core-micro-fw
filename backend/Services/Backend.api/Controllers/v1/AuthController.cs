using System.Net;
using System.Security.Claims;
using Application.Configs;
using Application.Configs.General;
using Application.Exceptions;
using Application.Responses.Base;
using Application.Utils;
using Application.Utils.Service;
using Domain.Requests.Processor.Services.User;
using FluentValidation;
using Infrastructure.Requests.Controllers.Auth;
using Infrastructure.Requests.Controllers.Common;
using Infrastructure.Requests.Processor.Services.User;
using Infrastructure.Responses.Controllers.Auth;
using Infrastructure.Services.Auth;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;
using ConfirmEmailRequest = Infrastructure.Requests.Controllers.Auth.ConfirmEmailRequest;
using ResetPasswordRequest = Infrastructure.Requests.Controllers.Auth.ResetPasswordRequest;
namespace Backend.api.Controllers.v1;

[ApiVersion("1.0")]
[Produces("application/json")]
[Authorize]
public class AuthController : BaseApiController {
    private readonly IValidator<ConfirmEmailRequest> _confirmEmailValidator;
    private readonly IValidator<ForgetPasswordRequest> _forgetPasswordValidator;
    private readonly FrontEndPaths _frontConfig;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IJwtAuthManager _jwtAuthManager;
    private readonly IValidator<LoginRequest> _loginValidator;
    private readonly IValidator<RefreshTokenRequest> _refreshTokenValidator;
    private readonly IValidator<RegisterNewUserRequest> _registerNewUserValidator;
    private readonly IValidator<MobileRequestOTPUserRequest> _requestOTPValidator;
    private readonly IValidator<ResetPasswordRequest> _resetPasswordValidator;
    private readonly IValidator<VerifyOTPUserRequest> _verifyOtpUserValidator;

    public AuthController(
        IMediator mediator,
        BackendApplicationConfig configuration,
        IHttpContextAccessor httpContextAccessor,
        ILoggerFactory loggerFactory,
        IJwtAuthManager jwtAuthManager,
        IOptions<FrontEndPaths> frontEndPathsConfig,
        IValidator<VerifyOTPUserRequest> verifyOtpUserValidator,
        IValidator<ResetPasswordRequest> resetPasswordValidator,
        IValidator<ConfirmEmailRequest> confirmEmailValidator,
        IValidator<ForgetPasswordRequest> forgetPasswordValidator,
        IValidator<LoginRequest> loginValidator,
        IValidator<RefreshTokenRequest> refreshTokenValidator,
        IValidator<MobileRequestOTPUserRequest> requestOTPValidator,
        IValidator<RegisterNewUserRequest> registerNewUserValidator) : base(mediator, configuration) {
        _jwtAuthManager = jwtAuthManager;
        _frontConfig = frontEndPathsConfig.Value;
        _registerNewUserValidator = registerNewUserValidator;
        _forgetPasswordValidator = forgetPasswordValidator;
        _verifyOtpUserValidator = verifyOtpUserValidator;
        _resetPasswordValidator = resetPasswordValidator;
        _loginValidator = loginValidator;
        _requestOTPValidator = requestOTPValidator;
        _confirmEmailValidator = confirmEmailValidator;
        _refreshTokenValidator = refreshTokenValidator;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    ///     will be called when user request to login
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("login/web")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request", typeof(DataResponse<EmptyResponse>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized request", typeof(DataResponse<EmptyResponse>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(DataResponse<EmptyResponse>))]
    [SwaggerOperation(Tags = new[] {
        "Auth"
    })]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request) {
        if (IsMaintenanceMode()) {
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "System is under maintenance"
            }, HttpStatusCode.ServiceUnavailable);
        }
        var validator = await _loginValidator.ValidateAsync(request);
        if (!validator.IsValid) {
            return ResponseHelper.CreateEmptyResponse(ResponseHelper.HandlerErrorResponse(validator), HttpStatusCode.BadRequest);
        }

        try {
            var response = await Mediator.Send(request.ToProcessorEntity(_httpContextAccessor.HttpContext!.Connection.RemoteIpAddress!.ToString(),
                _httpContextAccessor.HttpContext!.Request.Headers["User-Agent"].ToString()));
            return ResponseHelper.CreateResponse(response);
        }
        catch (UserNotFoundException e) {
            return ResponseHelper.CreateEmptyResponse(["User not found or password is not correct"], HttpStatusCode.Unauthorized);
        }
        catch (UserBlockException e) {
            return ResponseHelper.CreateEmptyResponse(["User account disabled"], HttpStatusCode.Unauthorized);
        }
        catch (UserLockedoutException e) {
            return ResponseHelper.CreateEmptyResponse(["User account locked out"], HttpStatusCode.Unauthorized);
        }
        catch (UserNotCompletedRegistrationException e) {
            Log.Error(e, "User not completed registration");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "User not completed registration"
            }, HttpStatusCode.Unauthorized);
        }
        catch (InvalidLoginException e) {
            return ResponseHelper.CreateEmptyResponse(["User not found or password is not correct"], HttpStatusCode.Unauthorized);
        }
        catch (Exception e) {
            Log.Error(e, "Error while logging out user");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "Error while logging out user"
            }, HttpStatusCode.InternalServerError);
        }
    }

    /// <summary>
    ///     will be called when user confirm email and request to reset password
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("login/confirm")]
    [SwaggerResponse(StatusCodes.Status200OK, "Response of request Forgot Password Confirm", typeof(DataResponse<VerifyOTPResponse>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request", typeof(DataResponse<EmptyResponse>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(DataResponse<EmptyResponse>))]
    [SwaggerOperation(Tags = new[] {
        "Auth"
    })]
    [AllowAnonymous]
    public async Task<IActionResult> LoginConfirm([SwaggerRequestBody("OTP Verify payload", Required = true)] VerifyOTPUserRequest request) {
        if (IsMaintenanceMode()) {
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "System is under maintenance"
            }, HttpStatusCode.ServiceUnavailable);
        }
        var validator = await _verifyOtpUserValidator.ValidateAsync(request);
        if (!validator.IsValid) {
            return ResponseHelper.CreateEmptyResponse(ResponseHelper.HandlerErrorResponse(validator), HttpStatusCode.BadRequest);
        }
        try {
            return ResponseHelper.CreateResponse(await Mediator.Send(new VerifyOtpUserRequest {
                Code = request.Code,
                OTPToken = request.OTPToken,
                UserToken = request.UserToken,
                ActionOtpType = ActionOtpType.Login,
                IpAddress = _httpContextAccessor.HttpContext!.Connection.RemoteIpAddress!.ToString(),
                UserAgent = _httpContextAccessor.HttpContext!.Request.Headers["User-Agent"].ToString()
            }));
        }
        catch (EntityNotFoundException e) {
            Log.Error(e, "One of the entities was not found");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "One of the params was not found"
            }, HttpStatusCode.BadRequest);
        }
        catch (Exception e) {
            Log.Error(e, "Error in RequestOTPCode");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                e.Message
            }, HttpStatusCode.InternalServerError);
        }
    }

    /// <summary>
    ///     will be called when user request to logout
    /// </summary>
    /// <returns></returns>
    [HttpDelete("logout")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(DataResponse<EmptyResponse>))]
    [SwaggerOperation(Tags = new[] {
        "Auth"
    })]
    [Authorize]
    public async Task<IActionResult> Logout() {
        if (IsMaintenanceMode()) {
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "System is under maintenance"
            }, HttpStatusCode.ServiceUnavailable);
        }
        var userToken = User.Identity?.Name!;
        try {
            await Mediator.Send(new LogoutUserRequest {
                UserToken = userToken
            });
            return ResponseHelper.CreateEmptyResponse();
        }
        catch (Exception e) {
            Log.Error(e, "Error while logging out user");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "Error while logging out user"
            }, HttpStatusCode.InternalServerError);
        }
    }

    /// <summary>
    ///     will be called when user forgot password and request to reset password link
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("forgot-password/request")]
    [SwaggerOperation(Tags = new[] {
        "Auth"
    })]
    [SwaggerResponse(StatusCodes.Status200OK, "Response of request OTP", typeof(DataResponse<RequestOTPResponse>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request", typeof(DataResponse<EmptyResponse>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(DataResponse<EmptyResponse>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(DataResponse<EmptyResponse>))]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(ForgetPasswordRequest request) {
        if (IsMaintenanceMode()) {
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "System is under maintenance"
            }, HttpStatusCode.ServiceUnavailable);
        }
        var validator = await _forgetPasswordValidator.ValidateAsync(request);
        if (!validator.IsValid) {
            return ResponseHelper.CreateEmptyResponse(ResponseHelper.HandlerErrorResponse(validator), HttpStatusCode.BadRequest);
        }
        try {
            var result = await Mediator.Send(request.ToProcessorEntity());
            return ResponseHelper.CreateResponse(result);
        }
        catch (EntityNotFoundException e) {
            Log.Error(e, "One of the entities was not found");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "One of the params was not found"
            }, HttpStatusCode.BadRequest);
        }
        catch (MFAProviderNotImplementedException e) {
            Log.Error(e, "MFA Provider is not implemented");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "MFA Provider is not implemented"
            }, HttpStatusCode.BadRequest);
        }
        catch (OTPNotFoundException e) {
            Log.Error(e, "OTP not found");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "OTP not found"
            }, HttpStatusCode.Forbidden);
        }
        catch (UserNotCompletedRegistrationException e) {
            Log.Error(e, "User not completed registration");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "User not completed registration"
            }, HttpStatusCode.Unauthorized);
        }
        catch (Exception e) {
            Log.Error(e, "Error in RequestOTPCode");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                e.Message
            }, HttpStatusCode.InternalServerError);
        }
    }

    /// <summary>
    ///     will be called when user confirm email and request to reset password
    ///     the output is redirect to login page
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("forgot-password/confirm")]
    [SwaggerOperation(Tags = new[] {
        "Auth"
    })]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPasswordConfirm([SwaggerRequestBody("OTP Verify payload", Required = true)] VerifyOTPUserRequest request) {
        if (IsMaintenanceMode()) {
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "System is under maintenance"
            }, HttpStatusCode.ServiceUnavailable);
        }
        var validator = await _verifyOtpUserValidator.ValidateAsync(request);
        if (!validator.IsValid) {
            ResponseHelper.CreateEmptyResponse(ResponseHelper.HandlerErrorResponse(validator), HttpStatusCode.BadRequest);
        }
        try {
            var result = await Mediator.Send(new VerifyOtpUserRequest {
                Code = request.Code,
                OTPToken = request.OTPToken,
                UserToken = request.UserToken,
                ActionOtpType = ActionOtpType.ForgotPassword,
                IpAddress = HttpContext.Connection.RemoteIpAddress!.ToString(),
                UserAgent = HttpContext.Request.Headers["User-Agent"].ToString()
            });
            if (result.UserId != null)
                return ResponseHelper.CreateResponse(result);
            Log.Error($"unable to confirm OTP for user [{request.UserToken} , {request.OTPToken}]");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "unable to confirm OTP"
            }, HttpStatusCode.BadRequest);
        }
        catch (EntityNotFoundException e) {
            Log.Error(e, "One of the entities was not found");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "One of the params was not found"
            }, HttpStatusCode.BadRequest);
        }
        catch (Exception e) {
            Log.Error(e, "Error in RequestOTPCode");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                e.Message
            }, HttpStatusCode.InternalServerError);
        }
    }

    /// <summary>
    ///     will be called when user request to reset password
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("reset-password")]
    [SwaggerOperation(Tags = new[] {
        "Auth"
    })]
    [SwaggerResponse(StatusCodes.Status200OK, "Response of request Reset Password", typeof(DataResponse<EmptyResponse>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request", typeof(DataResponse<EmptyResponse>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(DataResponse<EmptyResponse>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(DataResponse<EmptyResponse>))]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request) {
        if (IsMaintenanceMode()) {
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "System is under maintenance"
            }, HttpStatusCode.ServiceUnavailable);
        }
        var validator = await _resetPasswordValidator.ValidateAsync(request);
        if (!validator.IsValid) {
            return ResponseHelper.CreateEmptyResponse(ResponseHelper.HandlerErrorResponse(validator), HttpStatusCode.BadRequest);
        }
        try {
            var loginUserId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return ResponseHelper.CreateResponse(await Mediator.Send(request.ToProcessorEntity(Guid.Parse(loginUserId))));
        }
        catch (EntityNotFoundException e) {
            Log.Error(e, "One of the entities was not found");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "One of the params was not found"
            }, HttpStatusCode.BadRequest);
        }
        catch (AuthenticationException e) {
            Log.Error(e, e.Message);
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                e.Message
            }, HttpStatusCode.Unauthorized);
        }
        catch (UserNotCompletedRegistrationException e) {
            Log.Error(e, "User not completed registration");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "User not completed registration"
            }, HttpStatusCode.Unauthorized);
        }
        catch (Exception e) {
            Log.Error(e, "Error in ResetPassword");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                e.Message
            }, HttpStatusCode.InternalServerError);
        }
    }


    [HttpPost("login/mobile")]
    [SwaggerOperation("Request Mobile user OTP", "Send request OTP for mobile user", Tags = new[] {
        "Mobile Auth"
    })]
    [SwaggerResponse(StatusCodes.Status200OK, "Response of request OTP", typeof(DataResponse<RequestOTPResponse>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request", typeof(DataResponse<EmptyResponse>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(DataResponse<EmptyResponse>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(DataResponse<EmptyResponse>))]
    public async Task<IActionResult> RequestOTPCode([SwaggerRequestBody("OTP Request payload", Required = true)] MobileRequestOTPUserRequest request) {
        if (IsMaintenanceMode()) {
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "System is under maintenance"
            }, HttpStatusCode.ServiceUnavailable);
        }
        var validationResult = await _requestOTPValidator.ValidateAsync(request);
        if (!validationResult.IsValid) {
            return ResponseHelper.CreateEmptyResponse(ResponseHelper.HandlerErrorResponse(validationResult), HttpStatusCode.BadRequest);
        }
        try {
            var result = await Mediator.Send(request.ToProcessorEntity(_httpContextAccessor.HttpContext!.Connection.RemoteIpAddress!.ToString(),
                _httpContextAccessor.HttpContext!.Request.Headers["User-Agent"].ToString()));
            return ResponseHelper.CreateResponse(result);
        }
        catch (EntityNotFoundException e) {
            Log.Error(e, "One of the entities was not found");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "One of the params was not found"
            }, HttpStatusCode.BadRequest);
        }
        catch (MFAProviderNotImplementedException e) {
            Log.Error(e, "MFA Provider is not implemented");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "MFA Provider is not implemented"
            }, HttpStatusCode.BadRequest);
        }
        catch (OTPNotFoundException e) {
            Log.Error(e, "OTP not found");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "OTP not found"
            }, HttpStatusCode.Forbidden);
        }
        catch (UserNotCompletedRegistrationException e) {
            Log.Error(e, "User not completed registration");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "User not completed registration"
            }, HttpStatusCode.Unauthorized);
        }
        catch (Exception e) {
            Log.Error(e, "Error in RequestOTPCode");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                e.Message
            }, HttpStatusCode.InternalServerError);
        }
    }

    [HttpPut("refresh_tokens")]
    [SwaggerOperation("Request Mobile refresh OTP", "Send request refresh OTP for mobile user", Tags = new[] {
        "Mobile Auth"
    })]
    [SwaggerResponse(StatusCodes.Status200OK, "Response of request refresh OTP", typeof(DataResponse<MobileAuthResponse>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request", typeof(DataResponse<EmptyResponse>))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(DataResponse<EmptyResponse>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error", typeof(DataResponse<EmptyResponse>))]
    [Authorize]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request) {
        if (IsMaintenanceMode()) {
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "System is under maintenance"
            }, HttpStatusCode.ServiceUnavailable);
        }
        var validator = await _refreshTokenValidator.ValidateAsync(request);
        if (!validator.IsValid) {
            return ResponseHelper.CreateEmptyResponse(ResponseHelper.HandlerErrorResponse(validator), HttpStatusCode.BadRequest);
        }

        try {
            var userToken = User.Identity?.Name!;
            Log.Information($"User [{userToken}] is trying to refresh JWT token.");
            if (string.IsNullOrWhiteSpace(request.RefreshToken)) {
                return ResponseHelper.CreateEmptyResponse(new List<string> {
                    "Invalid client request"
                }, HttpStatusCode.BadRequest);
            }

            var accessToken = (await HttpContext.GetTokenAsync("Bearer", "access_token"))!;
            var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);
            Log.Information($"User [{userToken}] has refreshed JWT token.");
            return ResponseHelper.CreateResponse(new MobileAuthResponse {
                Tokens = jwtResult
            });
        }
        catch (SecurityTokenException e) {
            Log.Error("Invalid JWT token: {EMessage}", e.Message);
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "Invalid JWT token"
            }, HttpStatusCode.BadRequest); // return 401 so that the client side can redirect the user to login page
        }
        catch (Exception e) {
            Log.Error(e, "Error in RefreshToken");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                e.Message
            }, HttpStatusCode.InternalServerError);

        }
    }

    /// <summary>
    ///     this will be called when user request to confirm email
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("confirm-email")]
    [SwaggerOperation(Tags = new[] {
        "Auth"
    })]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(ConfirmEmailRequest request) {
        if (IsMaintenanceMode()) {
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "System is under maintenance"
            }, HttpStatusCode.ServiceUnavailable);
        }
        var validator = await _confirmEmailValidator.ValidateAsync(request);
        if (!validator.IsValid) {
            return ResponseHelper.CreateEmptyResponse(ResponseHelper.HandlerErrorResponse(validator), HttpStatusCode.BadRequest);
        }
        try {
            var loginUserId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await Mediator.Send(request.ToProcessorEntity(Guid.Parse(loginUserId!),
                HttpContext.Connection.RemoteIpAddress!.ToString(),
                HttpContext.Request.Headers["User-Agent"].ToString()));
            return ResponseHelper.CreateResponse(result);
        }
        catch (EntityNotFoundException e) {
            Log.Error(e, "One of the entities was not found");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "One of the params was not found"
            }, HttpStatusCode.BadRequest);
        }
        catch (InvalidRequestException e) {
            Log.Error(e, "Invalid request");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                $"Invalid request - {e.Message}"
            }, HttpStatusCode.BadRequest);
        }
        catch (AuthenticationException e) {
            Log.Error(e, e.Message);
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                e.Message
            }, HttpStatusCode.Unauthorized);
        }
        catch (Exception e) {
            Log.Error(e, "Error in RequestOTPCode");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                e.Message
            }, HttpStatusCode.InternalServerError);
        }
    }

    /// <summary>
    ///     this will be called when user request to register new account
    /// </summary>
    /// <param name="request"></param>
    /// <param name="automaticLogin">only for admin uses</param>
    /// <returns></returns>
    [HttpPost("register")]
    [SwaggerOperation(Tags = new[] {
        "Auth"
    })]
    [SwaggerResponse(StatusCodes.Status200OK, "Register new User", typeof(DataResponse<AuthResponse>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "bad register data", typeof(DataResponse<EmptyResponse>))]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        [SwaggerRequestBody("register user payload", Required = true)]
        RegisterNewUserRequest request) {
        if (IsMaintenanceMode()) {
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "System is under maintenance"
            }, HttpStatusCode.ServiceUnavailable);
        }
        var validator = await _registerNewUserValidator.ValidateAsync(request);
        if (!validator.IsValid) {
            return ResponseHelper.CreateEmptyResponse(ResponseHelper.HandlerErrorResponse(validator), HttpStatusCode.BadRequest);
        }

        try {
            return ResponseHelper.CreateResponse(await Mediator.Send(request.ToProcessorEntity(HttpContext.Connection.RemoteIpAddress!.ToString(),
                HttpContext.Request.Headers["User-Agent"].ToString())));
        }
        catch (InvalidRequestException e) {
            Log.Error(e, "Invalid request");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                $"Invalid request one or more the params not match"
            }, HttpStatusCode.BadRequest);
        }
        catch (AuthenticationException e) {
            Log.Error(e, e.Message);
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                e.Message
            }, HttpStatusCode.Unauthorized);
        }
        catch (EntityNotFoundException e) {
            Log.Error(e, "One of the entities was not found");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "One of the params was not found"
            }, HttpStatusCode.BadRequest);
        }
        catch (Exception e) {
            Log.Error(e, "Error in Register");
            return ResponseHelper.CreateEmptyResponse(new List<string> {
                "error while creating user account"
            }, HttpStatusCode.InternalServerError);
        }
    }

    // [Authorize(Roles = Roles.Admin)]

    // TODO : implement this  the main idea took from https://github.com/dotnet-labs/JwtAuthDemo/blob/master/webapi/JwtAuthDemo/Controllers/AccountController.cs please rebuild it 
    // [HttpPost("impersonation")]
    // [Authorize(Roles = UserRoles.Admin)]
    // public ActionResult Impersonate([FromBody] ImpersonationRequest request)
    // {
    //     var userName = User.Identity?.Name;
    //     _logger.LogInformation($"User [{userName}] is trying to impersonate [{request.UserName}].");
    //
    //     var impersonatedRole = _userService.GetUserRole(request.UserName);
    //     if (string.IsNullOrWhiteSpace(impersonatedRole))
    //     {
    //         _logger.LogInformation($"User [{userName}] failed to impersonate [{request.UserName}] due to the target user not found.");
    //         return BadRequest($"The target user [{request.UserName}] is not found.");
    //     }
    //     if (impersonatedRole == UserRoles.Admin)
    //     {
    //         _logger.LogInformation($"User [{userName}] is not allowed to impersonate another Admin.");
    //         return BadRequest("This action is not supported.");
    //     }
    //
    //     var claims = new[]
    //     {
    //         new Claim(ClaimTypes.Name,request.UserName),
    //         new Claim(ClaimTypes.Role, impersonatedRole),
    //         new Claim("OriginalUserName", userName ?? string.Empty)
    //     };
    //
    //     var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);
    //     _logger.LogInformation($"User [{request.UserName}] is impersonating [{request.UserName}] in the system.");
    //     return Ok(new LoginResult
    //     {
    //         UserName = request.UserName,
    //         Role = impersonatedRole,
    //         OriginalUserName = userName,
    //         AccessToken = jwtResult.AccessToken,
    //         RefreshToken = jwtResult.RefreshToken.TokenString
    //     });
    // }

    // TODO : implement this  the main idea took from https://github.com/dotnet-labs/JwtAuthDemo/blob/master/webapi/JwtAuthDemo/Controllers/AccountController.cs please rebuild it 
    // [HttpPost("stop-impersonation")]
    // public ActionResult StopImpersonation()
    // {
    //     var userName = User.Identity?.Name;
    //     var originalUserName = User.FindFirst("OriginalUserName")?.Value;
    //     if (string.IsNullOrWhiteSpace(originalUserName))
    //     {
    //         return BadRequest("You are not impersonating anyone.");
    //     }
    //     _logger.LogInformation($"User [{originalUserName}] is trying to stop impersonate [{userName}].");
    //
    //     var role = _userService.GetUserRole(originalUserName);
    //     var claims = new[]
    //     {
    //         new Claim(ClaimTypes.Name,originalUserName),
    //         new Claim(ClaimTypes.Role, role)
    //     };
    //
    //     var jwtResult = _jwtAuthManager.GenerateTokens(originalUserName, claims, DateTime.Now);
    //     _logger.LogInformation($"User [{originalUserName}] has stopped impersonation.");
    //     return Ok(new LoginResult
    //     {
    //         UserName = originalUserName,
    //         Role = role,
    //         OriginalUserName = null,
    //         AccessToken = jwtResult.AccessToken,
    //         RefreshToken = jwtResult.RefreshToken.TokenString
    //     });
    // }
}