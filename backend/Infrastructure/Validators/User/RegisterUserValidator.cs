using FluentValidation;
using Infrastructure.Requests.Controllers.Auth;
namespace Infrastructure.Validators.User;

public class RegisterUserValidator : AbstractValidator<RegisterNewUserRequest> {
    public RegisterUserValidator() {
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

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MinimumLength(2)
            .WithMessage("First name must be at least 3 characters")
            .MaximumLength(50)
            .WithMessage("First name must not exceed 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MinimumLength(2)
            .WithMessage("Last name must be at least 2 characters")
            .MaximumLength(50)
            .WithMessage("Last name must not exceed 50 characters");

        When(x => !string.IsNullOrEmpty(x.PhoneNumber), () => {
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
        });


    }
}