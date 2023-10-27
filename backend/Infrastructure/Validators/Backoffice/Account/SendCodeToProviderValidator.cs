using FluentValidation;
using Infrastructure.Requests.Account.Backoffice;
namespace Infrastructure.Validators.Backoffice.Account;

public class SendCodeToProviderValidator : AbstractValidator<SendCodeToProviderRequest> {
    public SendCodeToProviderValidator() {
        RuleFor(x => x.Provider)
            .NotEmpty()
            .WithMessage("Provider is required")
            .IsInEnum()
            .WithMessage("Provider is not valid");
    }
}