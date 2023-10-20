using FluentValidation;
using Infrastructure.Requests.Account;
namespace Infrastructure.Validators;

public class SendCodeToProviderValidator : AbstractValidator<SendCodeToProviderRequest> {
    public SendCodeToProviderValidator() {
        RuleFor(x => x.Provider)
            .NotEmpty()
            .WithMessage("Provider is required")
            .IsInEnum()
            .WithMessage("Provider is not valid");
    }
}