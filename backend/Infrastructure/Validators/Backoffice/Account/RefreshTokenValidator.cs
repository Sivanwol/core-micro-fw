using FluentValidation;
using Infrastructure.Requests.Backoffice.Account;
namespace Infrastructure.Validators.Backoffice.Account;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequest> {
    public RefreshTokenValidator() {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Token is required")
            .MinimumLength(20)
            .WithMessage("Token must be at least 8 characters");
    }
}