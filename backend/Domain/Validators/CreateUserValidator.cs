using Domain.Entities;
using FluentValidation;

namespace Domain.Validators;

public class CreateUserValidator : AbstractValidator<User> {
    public CreateUserValidator() {
        RuleFor(x => x.Occupation).NotEmpty().WithMessage("Occupation is required");
    }
}