using FluentValidation;
using Infrastructure.Requests.Controllers.Chat;
namespace Infrastructure.Validators.Chat;

public class ChatSendMessageValidator : AbstractValidator<ChatSendMessageRequest> {
    public ChatSendMessageValidator() {
        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Message is required")
            .MinimumLength(2)
            .MaximumLength(500)
            .WithMessage("Message must be between 2 and 500 characters");

        When(x => x.ReplyMessageId.HasValue, () => {
            RuleFor(x => x.ReplyMessageId)
                .NotEmpty()
                .WithMessage("Reply message id is required");
        });
    }
}