using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using IdentityModel.Client;
using Infrastructure.Enums;
using Infrastructure.Models.Account;
using Infrastructure.Responses.Auth;
using Infrastructure.Services.Auth;
using Infrastructure.Services.Auth.Sender;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using LoginRequest = Infrastructure.Models.Account.LoginRequest;
using RefreshTokenRequest = Infrastructure.Models.Account.RefreshTokenRequest;

namespace FrontApi.Controllers;

[Route("api/[controller]")]
[Authorize]
public class AuthController : Controller {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ISmsSender _smsSender;
    private readonly ILogger _logger;
    private readonly IJwtAuthManager _jwtAuthManager;
    private readonly IConfiguration _configuration;
    private readonly IValidator<RegisterNewUserRequest> _registerNewUserValidator;
    private readonly IValidator<ResetPasswordRequest> _resetPasswordValidator;
    private readonly IValidator<RefreshTokenRequest> _refreshTokenValidator;
    private readonly IValidator<VerifyFromProviderRequest> _verifyFromProviderValidator;
    private readonly IValidator<ForgetPasswordRequest> _forgetPasswordValidator;
    private readonly IValidator<SendCodeToProviderRequest> _sendCodeToProviderValidator;
    private readonly IValidator<LoginRequest> _loginValidator;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IEmailSender emailSender,
        ISmsSender smsSender,
        ILoggerFactory loggerFactory,
        IJwtAuthManager jwtAuthManager,
        IConfiguration configuration,
        IValidator<ResetPasswordRequest> resetPasswordValidator,
        IValidator<ForgetPasswordRequest> forgetPasswordValidator,
        IValidator<LoginRequest> loginValidator,
        IValidator<RefreshTokenRequest> refreshTokenValidator,
        IValidator<VerifyFromProviderRequest> verifyFromProviderValidator,
        IValidator<SendCodeToProviderRequest> sendCodeToProviderValidator,
        IValidator<RegisterNewUserRequest> registerNewUserValidator) {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _smsSender = smsSender;
        _logger = loggerFactory.CreateLogger<AuthController>();
        _configuration = configuration;
        _jwtAuthManager = jwtAuthManager;
        _registerNewUserValidator = registerNewUserValidator;
        _resetPasswordValidator = resetPasswordValidator;
        _forgetPasswordValidator = forgetPasswordValidator;
        _loginValidator = loginValidator;
        _sendCodeToProviderValidator = sendCodeToProviderValidator;
        _refreshTokenValidator = refreshTokenValidator;
        _verifyFromProviderValidator = verifyFromProviderValidator;
    }

    /// <summary>
    /// will be called when user request to login
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request) {
        var response = new AuthResponse();
        var validator = await _loginValidator.ValidateAsync(request);
        if (!validator.IsValid) {
            response.Errors = HandlerErrorResponse(validator);
            return BadRequest(response);
        }

        var result =
            await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe,
                lockoutOnFailure: false);
        var user = await _userManager.FindByEmailAsync(request.Email);
        // user not found
        if (user == null) {
            _logger.LogWarning(2, "User account not found");
            response.Errors.Add("User not found or password is not correct");
            return Unauthorized(response);
        }

        response.UserId = user.Id;
        // user account got disabled
        if (result.IsNotAllowed) {
            _logger.LogWarning(2, "User account is disabled - {UserId}", user.Id);
            response.Errors.Add("User account disabled");
            return Unauthorized(response);
        }

        // user need two factor
        if (result.RequiresTwoFactor) {
            _logger.LogWarning(2, "User account required two way factor - {UserId}", user.Id);
            response.Errors.Add("User account required two way factor");
            response.RequeiredMFA = true;
            return Unauthorized(response);
        }

        // user locked out
        if (result.IsLockedOut) {
            _logger.LogWarning(2, "User account locked out - {UserId}", user.Id);
            response.Errors.Add("User account locked out");
            response.LockedOut = true;
            return Unauthorized(response);
        }

        // bad password
        if (!result.Succeeded) {
            _logger.LogWarning(2, "Invalid login attempt - {UserId}", user.Id);
            response.Errors.Add("User not found or password is not correct");
            return Unauthorized(response);
        }

        _logger.LogInformation(1, "User logged in -  {UserId}", user.Id);
        response.Status = true;
        var claims = await _userManager.GetClaimsAsync(user);
        response.Tokens = _jwtAuthManager.GenerateTokens(user.Email, claims, DateTime.Now);
        response.UserId = user.Id;
        return Ok(response);
    }

    /// <summary>
    /// will be called when user request to logout
    /// </summary>
    /// <returns></returns>
    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult> Logout() {
        var userName = User.Identity?.Name;
        await _signInManager.SignOutAsync();
        _jwtAuthManager.RemoveRefreshTokenByUserName(userName);
        _logger.LogInformation("User [{UserName}] logged out the system.", userName);
        var response = new AuthResponse {
            Status = true
        };
        return Ok(response);
    }

    /// <summary>
    /// will be called when user forgot password and request to reset password link
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(ForgetPasswordRequest request) {
        var response = new AuthResponse();
        if (!ModelState.IsValid) return BadRequest(response);
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user))) {
            // Don't reveal that the user does not exist or is not confirmed
            return Ok(response);
        }

        response.Status = true;
        // TODO: handle of forget password logic
        // For more information on how to enable account confirmation and password reset please
        // visit https://go.microsoft.com/fwlink/?LinkID=532713
        // var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        // var callbackUrl = Url.Action("ResetPassword", "Account", new {userId = user.Id, code},
        //     protocol: HttpContext.Request.Scheme);
        // await _emailSender.SendEmailAsync(request.Email, "Reset Password",
        //     $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
        return Ok(response);
    }

    /// <summary>
    /// will be called when user request to reset password
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request) {
        var response = new AuthResponse();
        var validator = await _resetPasswordValidator.ValidateAsync(request);
        if (!validator.IsValid) {
            response.Errors = HandlerErrorResponse(validator);
            return BadRequest(response);
        }

        ;
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) {
            return BadRequest(response);
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Code, request.Password);
        if (result.Succeeded) {
            response.Status = true;
            return Ok(response);
        }

        _logger.LogError("User [{UserId}] failed to reset password. Errors: {ResultErrors}", user.Id,
            result.Errors.Select(s => s.Description).ToList());
        response.Errors.Add("Error while reseting password");
        return BadRequest(response);
    }

    /// <summary>
    /// will be called when active access token is expired and need to be refreshed
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("refresh-token")]
    [Authorize]
    public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request) {
        var response = new AuthResponse();
        var validator = await _refreshTokenValidator.ValidateAsync(request);
        if (!validator.IsValid) {
            response.Errors = HandlerErrorResponse(validator);
            return BadRequest(response);
        }

        try {
            var userName = User.Identity?.Name;
            _logger.LogInformation($"User [{userName}] is trying to refresh JWT token.");
            if (string.IsNullOrWhiteSpace(request.RefreshToken)) {
                response.Errors.Add("Invalid client request");
                return Unauthorized(response);
            }

            var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
            var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);
            _logger.LogInformation($"User [{userName}] has refreshed JWT token.");

            var user = await _userManager.FindByEmailAsync(userName);
            response.Tokens = jwtResult;
            response.UserId = user.Id;
            response.Status = true;
            return Ok(response);
        }
        catch (SecurityTokenException e) {
            _logger.LogError("Invalid JWT token: {EMessage}", e.Message);
            response.Errors.Add("Invalid JWT token");
            return
                Unauthorized(response); // return 401 so that the client side can redirect the user to login page
        }
    }

    /// <summary>
    /// will be called when user login request mfa access this will trigger the mfa code to be sent to user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("request-code")]
    [AllowAnonymous]
    public async Task<IActionResult> RequestCode(SendCodeToProviderRequest request) {
        var response = new AuthResponse();
        var validator = await _sendCodeToProviderValidator.ValidateAsync(request);
        if (!validator.IsValid) {
            response.Errors = HandlerErrorResponse(validator);
            return BadRequest(response);
        }

        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null) {
            response.Errors.Add("Unable to load two-factor authentication user.");
            return BadRequest(response);
        }

        var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
        var requestProvider = request.Provider == AuthProvidersMFA.SMS ? "Phone" : "Email";
        if (!providers.Contains(requestProvider)) {
            return BadRequest(response);
        }

        var code = await _userManager.GenerateTwoFactorTokenAsync(user, requestProvider);
        if (string.IsNullOrWhiteSpace(code)) {
            return BadRequest(response);
        }

        var message = "Your security code is: " + code;
        switch (requestProvider) {
            case "Email":
                await _emailSender.SendEmailAsync(await _userManager.GetEmailAsync(user), "Security Code", message);
                break;
            case "Phone":
                await _smsSender.SendSmsAsync(await _userManager.GetPhoneNumberAsync(user), message);
                break;
        }

        response.Status = true;
        return Ok(response);
    }

    /// <summary>
    /// this will be called when user request to verify mfa code
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("verify-code")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyCode(VerifyFromProviderRequest request) {
        var response = new AuthResponse();
        var validator = await _verifyFromProviderValidator.ValidateAsync(request);
        if (!validator.IsValid) {
            response.Errors = HandlerErrorResponse(validator);
            return BadRequest(response);
        }

        var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(request.Code, true, request.RememberMe);
        if (result.Succeeded) {
            response.Status = true;
            return Ok(response);
        }

        if (result.IsLockedOut) {
            _logger.LogWarning(2, "User account locked out.");
            response.Errors.Add("User account locked out.");
            return BadRequest(response);
        }

        response.Errors.Add("Invalid code.");
        return BadRequest(response);
    }

    /// <summary>
    /// this will be called when user request to confirm email
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    [HttpGet("confirm-email")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string userId, string code) {
        var response = new AuthResponse();
        if (userId == null || code == null) {
            response.Errors.Add("userId or code is null");
            return BadRequest(response);
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) {
            response.Errors.Add($"Unable to load user with ID '{userId}'.");
            return BadRequest(response);
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (result.Succeeded) {
            response.Status = true;
            return Ok(response);
        }

        response.Errors.Add($"wrong code for confirming email for user with ID '{userId}'");
        return BadRequest(response);
    }

    /// <summary>
    /// this will be called when user request to register new account
    /// </summary>
    /// <param name="request"></param>
    /// <param name="automaticLogin">only for admin uses</param>
    /// <returns></returns>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterNewUserRequest request, bool automaticLogin = false) {
        var response = new AuthResponse();
        var validator = await _registerNewUserValidator.ValidateAsync(request);
        if (!validator.IsValid) {
            response.Errors = HandlerErrorResponse(validator);
            return BadRequest(response);
        }

        var user = new ApplicationUser {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            TwoFactorEnabled = true
        };
        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded) {
            _logger.LogInformation(3, "User created a new account with password.");
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            // Todo : handle new user emaqil confirmation / phone number confirmation
            // var callbackUrl = Url.Action("ConfirmEmail", "Account", new {userId = user.Id, code},
            //     protocol: HttpContext.Request.Scheme);
            // await _emailSender.SendEmailAsync(request.Email, "Confirm your account",
            //     $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
            if (automaticLogin) {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            _logger.LogInformation(4, "User created a new account with password.");
            response.Status = true;
            response.UserId = user.Id;
            return Ok(response);
        }

        response.Errors = new List<string> {
            "Error while creating user account"
        };
        return BadRequest(response);
    }


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
    private IList<string> HandlerErrorResponse(ValidationResult result) {
        return result.Errors.Select(error => error.ErrorMessage).ToList();
    }

    private Task<ApplicationUser> GetCurrentUserAsync() {
        return _userManager.GetUserAsync(HttpContext.User);
    }
}