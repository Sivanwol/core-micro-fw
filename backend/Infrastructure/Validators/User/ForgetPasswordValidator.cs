using Application.Utils;
using FluentValidation;
using Infrastructure.Enums;
using Infrastructure.Requests.Controllers.Auth;
namespace Infrastructure.Validators.User;

public class ForgetPasswordValidator : AbstractValidator<ForgetPasswordRequest> {
    public ForgetPasswordValidator() {
        When(x => !string.IsNullOrEmpty(x.Email) && string.IsNullOrEmpty(x.PhoneNumber) && !x.CountryId.HasValue, () => {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .MaximumLength(500)
                .WithMessage("Email must not exceed 500 characters")
                .EmailAddress()
                .WithMessage("Email is not valid");
        });
        When(x => string.IsNullOrEmpty(x.Email) && !string.IsNullOrEmpty(x.PhoneNumber) && x.CountryId.HasValue, () => {
            RuleFor(x => x.CountryId)
                .NotEmpty()
                .WithMessage("CountyId is required")
                .GreaterThan(0)
                .WithMessage("CountyId is not valid");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone is required")
                .MinimumLength(7)
                .WithMessage("Phone must not be less than 7 characters")
                .MaximumLength(15)
                .WithMessage("Phone must not exceed 15 characters")
                .MatchPhoneNumber();
        });
        RuleFor(x => x.Provider)
            .IsEnumName(typeof(MFAProvider), false)
            .WithMessage("Provider is not valid");
    }
}