using FluentValidation;
using Infrastructure.Requests.Account.Auth;
namespace Infrastructure.Validators.Account;

public class LoginUserValidator : AbstractValidator<MobileLoginUserRequest> {
    public LoginUserValidator() {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .MinimumLength(5)
            .WithMessage("Phone number must be at least 5 characters")
            .MaximumLength(15)
            .WithMessage("Phone number must not exceed 15 characters")
            .Matches(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")
            .WithMessage("Phone number is not valid");

        RuleFor(x => x.CountryId)
            .NotEmpty()
            .WithMessage("Country Id is required");
        
        RuleFor(x => x.Provider)
            .NotEmpty()
            .WithMessage("Provider is required")
            .IsInEnum()
            .WithMessage("Provider is not valid");
    }
}