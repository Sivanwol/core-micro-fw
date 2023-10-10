using FluentValidation;
using Infrastructure.Models.Account;

namespace Infrastructure.Validators;

public class SendCodeFromProviderValidator : AbstractValidator<VerifyFromProviderRequest> {
    public SendCodeFromProviderValidator() {
        RuleFor(x => x.Provider)
            .NotEmpty()
            .WithMessage("Provider is required")
            .IsInEnum()
            .WithMessage("Provider is not valid");
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is required")
            .MinimumLength(6)
            .WithMessage("Code must be at least 6 characters")
            .MaximumLength(12)
            .WithMessage("Code must not exceed 12 characters");
    }
}