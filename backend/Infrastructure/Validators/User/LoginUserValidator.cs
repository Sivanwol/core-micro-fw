using FluentValidation;
using Infrastructure.Enums;
using Infrastructure.Requests.Controllers.Auth;
namespace Infrastructure.Validators.User;

public class LoginUserValidator : AbstractValidator<LoginRequest> {
    public LoginUserValidator() {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .MaximumLength(500)
            .WithMessage("Email must not exceed 500 characters")
            .EmailAddress()
            .WithMessage("Email is not valid");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters")
            .MaximumLength(18)
            .WithMessage("Password must not exceed 18 characters")
            .Matches(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\W)(?!.* ).{8,18}$")
            .WithMessage(
                "Password must contain at least 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character");

        RuleFor(x => x.Provider)
            .NotEmpty()
            .WithMessage("Provider is required")
            .IsEnumName(typeof(MFAProvider))
            .WithMessage("Provider is not valid");
    }
}