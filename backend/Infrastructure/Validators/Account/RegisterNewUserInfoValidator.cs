using FluentValidation;
using Infrastructure.Enums;
using Infrastructure.Requests.Controllers.User;
namespace Infrastructure.Validators.Account;

public class RegisterNewUserInfoValidator : AbstractValidator<UpdateRegisterNewUserInfoRequest> {
    public RegisterNewUserInfoValidator() {
        RuleFor(x => x.Gender)
            .IsEnumName(typeof(Gender), caseSensitive: false)
            .WithMessage("Gender is required");

        RuleFor(x => x.Height)
            .NotEmpty()
            .WithMessage("Height is required")
            .GreaterThan(60)
            .WithMessage("Height must be greater than 60")
            .LessThan(300)
            .WithMessage("Height must be less than 300");

        RuleFor(x => x.HeightMeasureUnit)
            .IsEnumName(typeof(MeasureUnit), caseSensitive: false)
            .WithMessage("Height measure unit is required");

        RuleFor(x => x.ReligionId)
            .NotEmpty()
            .WithMessage("Religion Id is required");

        RuleFor(x => x.EthnicityId)
            .NotEmpty()
            .WithMessage("Ethnicity Id is required");
    }
}