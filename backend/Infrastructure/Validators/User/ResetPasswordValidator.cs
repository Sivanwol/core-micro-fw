using Application.Utils;
using FluentValidation;
using Infrastructure.Requests.Controllers.Auth;
namespace Infrastructure.Validators.User;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequest> {
    public ResetPasswordValidator() {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required")
            .IsGuid()
            .WithMessage("User Id is not valid");

        RuleFor(x => x.OldPassword.Trim())
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters")
            .MaximumLength(18)
            .WithMessage("Password must not exceed 18 characters");
        RuleFor(x => x.NewPassword.Trim())
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters")
            .MaximumLength(18)
            .WithMessage("Password must not exceed 18 characters")
            .NotEqual(x => x.OldPassword.Trim())
            .WithMessage("New Password must not be the same as Old Password")
            .Matches(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\W)(?!.* ).{8,18}$")
            .WithMessage(
                "Password must contain at least 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character");
    }
}