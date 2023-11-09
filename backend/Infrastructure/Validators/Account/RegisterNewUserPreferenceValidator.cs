using FluentValidation;
using Infrastructure.Requests.Controllers.User;
namespace Infrastructure.Validators.Account;

public class RegisterNewUserPreferenceValidator : AbstractValidator<UpdateRegisterNewUserPreferenceRequest> {
    public RegisterNewUserPreferenceValidator() {

        RuleFor(x => x.Gender)
            .IsInEnum()
            .WithMessage("Gender is required");

        RuleFor(x => x.HeightFrom)
            .NotEmpty()
            .WithMessage("Height is required")
            .GreaterThan(60)
            .WithMessage("Height must be greater than 60")
            .LessThan(300)
            .WithMessage("Height must be less than 300")
            .When(x => x.HeightFrom > x.HeightTo)
            .WithMessage("Height From must be less than height To");

        RuleFor(x => x.HeightTo)
            .NotEmpty()
            .WithMessage("Height is required")
            .GreaterThan(60)
            .WithMessage("Height must be greater than 60")
            .LessThan(300)
            .WithMessage("Height must be less than 300")
            .When(x => x.HeightFrom < x.HeightTo)
            .WithMessage("Height To must be greater than height From");

        RuleFor(x => x.AgeFrom)
            .NotEmpty()
            .WithMessage("Age From is required")
            .GreaterThan(60)
            .WithMessage("Age From must be greater than 60")
            .LessThan(300)
            .WithMessage("Age From must be less than 300")
            .When(x => x.AgeFrom > x.AgeTo)
            .WithMessage("Age From to must be greater than Age To");

        RuleFor(x => x.AgeTo)
            .NotEmpty()
            .WithMessage("Age From is required")
            .GreaterThan(60)
            .WithMessage("Age From must be greater than 60")
            .LessThan(300)
            .WithMessage("Age From must be less than 300")
            .When(x => x.AgeFrom < x.AgeTo)
            .WithMessage("Age From to must be greater than Age To");


        RuleFor(x => x.ReligionIds)
            .NotEmpty()
            .WithMessage("Religion Id is required");
        RuleForEach(x => x.ReligionIds)
            .GreaterThan(0)
            .WithMessage("Religion Id must be greater than 0");

        RuleFor(x => x.EthnicityIds)
            .NotEmpty()
            .WithMessage("Ethnicity Id is required");
        RuleForEach(x => x.EthnicityIds)
            .GreaterThan(0)
            .WithMessage("Religion Id must be greater than 0");
    }
}