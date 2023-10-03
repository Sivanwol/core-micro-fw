using Domain.Entities;
using FluentValidation;

namespace Domain.Validators;

public class CreateUserValidator : AbstractValidator<ApplicationUser> {
    public CreateUserValidator() {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MinimumLength(2)
            .WithMessage("First name must be at least 3 characters")
            .MaximumLength(50)
            .WithMessage("First name must not exceed 50 characters");
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MinimumLength(2)
            .WithMessage("Last name must be at least 2 characters")
            .MaximumLength(50)
            .WithMessage("Last name must not exceed 50 characters");
    }
}