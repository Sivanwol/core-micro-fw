using Application.Utils;
using FluentValidation;
using Infrastructure.Requests.Controllers.Auth;
namespace Infrastructure.Validators.User;

public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailRequest> {
    public ConfirmEmailValidator() {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required")
            .IsGuid()
            .WithMessage("User Id is not valid");

        RuleFor(x => x.UserToken)
            .NotEmpty()
            .WithMessage("User Token is required")
            .MaximumLength(128)
            .WithMessage("User Token must not exceed 128 characters")
            .MinimumLength(128)
            .WithMessage("User Token must be at least 128 characters");

        When(x => !string.IsNullOrEmpty(x.Code), () => {
            RuleFor(x => x.Code)
                .MaximumLength(6)
                .WithMessage("Code must not exceed 6 characters")
                .MinimumLength(20)
                .WithMessage("Code must be at least 20 characters");
        });
    }
}