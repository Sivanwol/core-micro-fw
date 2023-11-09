using FluentValidation;
using Infrastructure.Requests.Controllers.Session;
namespace Infrastructure.Validators.Session;

public class StartSessionValidator : AbstractValidator<StartSessionRequest> {
    public StartSessionValidator() {

        RuleFor(x => x.MatchingUserId)
            .NotNull()
            .NotEmpty()
            .WithMessage("Matching User Ids are required");
        RuleForEach(x => x.MatchingUserId)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Matching User Ids are required");

    }
}