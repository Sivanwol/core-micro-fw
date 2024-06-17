using FluentValidation;
namespace Application.Utils;

public static class ValidationExtensions {
    public static IRuleBuilderOptions<T, string> MatchPhoneNumber<T>(this IRuleBuilder<T, string> rule) {
        return rule.Matches(@"^(1-)?\d{3}-\d{3}-\d{4}$").WithMessage("Invalid phone number");
    }
    public static IRuleBuilderOptions<T, Guid?> IsGuid<T>(this IRuleBuilder<T, Guid?> rule) {
        return rule.Must(x => IsValidGuid(x.Value)).WithMessage("Invalid Guid");
    }
    public static IRuleBuilderOptions<T, Guid> IsGuid<T>(this IRuleBuilder<T, Guid> rule) {
        return rule.Must(x => IsValidGuid(x)).WithMessage("Invalid Guid");
    }
    public static IRuleBuilderOptions<T, string> IsGuid<T>(this IRuleBuilder<T, string> rule) {
        return rule.Must(x => Guid.TryParse(x, out var validatedGuid) && IsValidGuid(validatedGuid)).WithMessage("Invalid Guid");
    }


    private static bool IsValidGuid(Guid unValidatedGuid) {
        if (unValidatedGuid != Guid.Empty) {
            return Guid.TryParse(unValidatedGuid.ToString(), out var validatedGuid);
        }
        return false;
    }
}