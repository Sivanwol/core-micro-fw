using FluentValidation;
using Infrastructure.Requests.Controllers.Chat;
namespace Infrastructure.Validators.Chat;

public class ChatEditMessageValidator : AbstractValidator<ChatEditMessageRequest> {
    public ChatEditMessageValidator() {
        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Message is required")
            .MinimumLength(2)
            .MaximumLength(500)
            .WithMessage("Message must be between 2 and 500 characters");

        RuleFor(x => x.MessageId)
            .NotEmpty()
            .WithMessage("Message id is required");
    }
}