using Domain.Entities;
using FluentValidation;

namespace Domain.Validators; 

public class CreateUserValidator: AbstractValidator<User> {
    public CreateUserValidator() {
        RuleFor(x => x.Auth0Id).NotEmpty().MinimumLength(10);
    }
}