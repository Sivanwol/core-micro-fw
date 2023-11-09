using FluentValidation;
using Infrastructure.Requests.Controllers.Session;
namespace Infrastructure.Validators.Session;

public class SessionQuestionRateValidator : AbstractValidator<SessionQuestionRateRequest> {
    public SessionQuestionRateValidator() {

        RuleFor(x => x.QuestionId)
            .NotNull()
            .NotEmpty()
            .WithMessage("Question Id is required");

        RuleFor(x => x.Rate)
            .NotNull()
            .LessThanOrEqualTo(5)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Rate must be between 1 and 5");
    }
}