using FluentValidation;
using Infrastructure.Requests.Processor.Services.User;
namespace Infrastructure.Validators.User;

public class UpdateUserPreferenceValidator : AbstractValidator<UpdateUserPreferenceRequest> {
    public UpdateUserPreferenceValidator() {
        RuleFor(x => x.LoggedInUserId)
            .NotEmpty()
            .WithMessage("UserId is required");
        RuleFor(x => x.Preferences)
            .NotEmpty()
            .WithMessage("Preferences is required")
            .Must(x => x.Count > 0)
            .WithMessage("Preferences must have at least one item");
        When(x => x.Preferences.Any(), () => {
            RuleForEach(x => x.Preferences)
                .NotEmpty()
                .WithMessage("Preference is required")
                .Must(x => x is { Key: not null, Value: not null })
                .WithMessage("Preference must have a key and a value")
                .Must(x => x.Key.Length >= 3)
                .WithMessage("Preference key must be at least 3 characters")
                .Must(x => x.Key.Length <= 250)
                .WithMessage("Preference key must not exceed 250 characters")
                .Must(x => x.Value.Length <= 250)
                .WithMessage("Preference value must not exceed 250 characters");

        });
    }
}