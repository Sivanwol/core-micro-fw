using FluentValidation;
using Infrastructure.Requests.Controllers.User;
namespace Infrastructure.Validators.Account;

public class UpdateUserProfileValidator : AbstractValidator<UpdateUserProfileRequest> {
    public UpdateUserProfileValidator() {
        RuleFor(x => x.PresonalInfo)
            .NotNull()
            .WithMessage("Personal Info is required");

        RuleFor(x => x.Preference)
            .NotNull()
            .WithMessage("Preference is required");

        RuleFor(x => x.PresonalInfo)
            .SetValidator(new RegisterNewUserInfoValidator());

        RuleFor(x => x.Preference)
            .SetValidator(new RegisterNewUserPreferenceValidator());
    }
}