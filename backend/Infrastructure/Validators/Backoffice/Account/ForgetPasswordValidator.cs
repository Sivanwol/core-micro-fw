using FluentValidation;
using Infrastructure.Requests.Controllers.Backoffice;
namespace Infrastructure.Validators.Backoffice.Account;

public class ForgetPasswordValidator : AbstractValidator<ForgetPasswordRequest> {
    public ForgetPasswordValidator() {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .MaximumLength(500)
            .WithMessage("Email must not exceed 500 characters")
            .EmailAddress()
            .WithMessage("Email is not valid");
    }
}