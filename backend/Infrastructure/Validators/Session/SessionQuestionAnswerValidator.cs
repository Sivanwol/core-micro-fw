using FluentValidation;
using Infrastructure.Requests.Controllers.Session;
namespace Infrastructure.Validators.Session;

public class SessionQuestionAnswerValidator : AbstractValidator<SessionQuestionAnswerRequest> {
    public SessionQuestionAnswerValidator() {

        RuleFor(x => x.QuestionId)
            .NotNull()
            .NotEmpty()
            .WithMessage("Question Id is required");

        When(x => string.IsNullOrEmpty(x.AnswerText), () => {
            RuleFor(x => x.AnswerId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Answer Id is required");
        });
        When(x => x.AnswerId == null, () => {
            RuleFor(x => x.AnswerText)
                .NotNull()
                .NotEmpty()
                .WithMessage("Answer Text is required");
        });
    }
}