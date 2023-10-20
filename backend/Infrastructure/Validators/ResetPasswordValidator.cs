using FluentValidation;
using Infrastructure.Requests.Account;
namespace Infrastructure.Validators;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequest> {
    public ResetPasswordValidator() {
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
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters")
            .MaximumLength(18)
            .WithMessage("Password must not exceed 18 characters")
            .Matches(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\W)(?!.* ).{8,18}$")
            .WithMessage(
                "Password must contain at least 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character")
            .Equal(x => x.Password)
            .WithMessage("Password and Confirm Password must match");
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is required");
    }
}