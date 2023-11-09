using FluentValidation;
using Infrastructure.Enums;
using Infrastructure.Requests.Controllers.Chat;
namespace Infrastructure.Validators.Chat;

public class ChatFlagUserValidator : AbstractValidator<ChatFlagUserRequest> {
    public ChatFlagUserValidator() {
        RuleFor(x => x.ReportUserId)
            .NotEmpty()
            .WithMessage("Report User Id is required");
        RuleFor(x => x.ReportType)
            .IsInEnum()
            .WithMessage("Report Type is required");
        RuleFor(x => x.ReportType)
            .IsEnumName(typeof(ReportFlag), caseSensitive: false)
            .WithMessage("Report Type is not valid");
        When(x => Enum.Parse<ReportFlag>(x.ReportType) == ReportFlag.Other, () => {
            RuleFor(x => x.Reason)
                .NotEmpty()
                .WithMessage("Reason is required")
                .MinimumLength(2)
                .MaximumLength(500)
                .WithMessage("Reason must be between 2 and 1000 characters");
        });
        When(x => !string.IsNullOrEmpty(x.Reason) && Enum.Parse<ReportFlag>(x.ReportType) != ReportFlag.Other, () => {
            RuleFor(x => x.Reason)
                .MinimumLength(2)
                .MaximumLength(500)
                .WithMessage("Reason must be between 2 and 1000 characters");
        });
    }
}