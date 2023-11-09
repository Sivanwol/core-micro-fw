using FluentValidation;
using Infrastructure.Enums;
using Infrastructure.Requests.Controllers.Session;
namespace Infrastructure.Validators.Session;

public class SessionMarkUnmatchValidator : AbstractValidator<SessionMarkUnmatchRequest> {
    public SessionMarkUnmatchValidator() {

        RuleFor(x => x.Status)
            .IsEnumName(typeof(UnmatchReasonStatus), caseSensitive: false)
            .WithMessage("Status must be 1 - unmatch or 2 - user blocked");

        RuleFor(x => x.Reason)
            .NotNull()
            .MinimumLength(3)
            .MaximumLength(100)
            .WithMessage("Reason must be between 3 and 100 characters");
    }
}