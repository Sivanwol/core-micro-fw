using FluentValidation;
using Infrastructure.Requests.Controllers.Auth;
namespace Infrastructure.Validators.User;

public class VerifyOTPValidator : AbstractValidator<VerifyOTPUserRequest> {
    private const int MAX_TOKEN_LENGTH = 128;
    public VerifyOTPValidator() {

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is required")
            .MinimumLength(6)
            .WithMessage("Code must be at least 6 characters")
            .MaximumLength(50)
            .WithMessage("Code must not exceed 50 characters");


        RuleFor(x => x.OTPToken)
            .NotEmpty()
            .WithMessage("OTPToken is required")
            .MinimumLength(MAX_TOKEN_LENGTH - 1)
            .WithMessage($"OTPToken must be at least {MAX_TOKEN_LENGTH - 1} characters")
            .MaximumLength(MAX_TOKEN_LENGTH)
            .WithMessage($"OTPToken must not exceed {MAX_TOKEN_LENGTH} characters");


        RuleFor(x => x.UserToken)
            .NotEmpty()
            .WithMessage("UserToken is required")
            .MinimumLength(MAX_TOKEN_LENGTH - 1)
            .WithMessage($"UserToken must be at least {MAX_TOKEN_LENGTH - 1} characters")
            .MaximumLength(MAX_TOKEN_LENGTH)
            .WithMessage($"UserToken must not exceed {MAX_TOKEN_LENGTH} characters");
    }
}