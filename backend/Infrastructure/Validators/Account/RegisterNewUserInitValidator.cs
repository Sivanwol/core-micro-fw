using FluentValidation;
using Infrastructure.Requests.Controllers.Auth;
namespace Infrastructure.Validators.Account;

public class RegisterNewUserInitValidator : AbstractValidator<MobileRegisterNewUserRequest> {
    public RegisterNewUserInitValidator() {
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

        RuleFor(x => x.LanguageId)
            .NotEmpty()
            .WithMessage("Language Id is required");

        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .WithMessage("Birth Date is required")
            .LessThan(DateTime.Now.AddYears(-18))
            .WithMessage("Birth Date must be at least 18 years old");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email is not valid");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First Name is required");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last Name is required");

        RuleFor(x => x.Latitude)
            .NotNull()
            .WithMessage("Latitude is required");

        RuleFor(x => x.Longitude)
            .NotNull()
            .WithMessage("Longitude is required");

    }
}