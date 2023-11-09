using FluentValidation;
using Infrastructure.Requests.Controllers.Session;
namespace Infrastructure.Validators.Session;

public class SessionReplyToQuestionValidator : AbstractValidator<SessionReplyToQuestionRequest> {
    public SessionReplyToQuestionValidator() {
        RuleFor(x => x.QuestionId)
            .NotNull()
            .NotEmpty()
            .WithMessage("Question Id is required");

        RuleFor(x => x.Message)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(500)
            .WithMessage("Message must be between 2 and 500 characters");
    }
}