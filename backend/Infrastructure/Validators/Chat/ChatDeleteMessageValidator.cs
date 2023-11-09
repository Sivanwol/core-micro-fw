using FluentValidation;
using Infrastructure.Requests.Controllers.Chat;
namespace Infrastructure.Validators.Chat;

public class ChatDeleteMessageValidator : AbstractValidator<ChatDeleteMessageRequest> {
    public ChatDeleteMessageValidator() {

        RuleFor(x => x.MessageId)
            .NotEmpty()
            .WithMessage("Message id is required");
    }
}