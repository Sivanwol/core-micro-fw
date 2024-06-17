using FluentValidation;
using Infrastructure.GQL.Inputs.User;
using Infrastructure.Requests.Processor.Services.User;
namespace Infrastructure.Validators.User;

public class UpdateUserProfileValidator : AbstractValidator<UpdateUserProfileInput> {
    public UpdateUserProfileValidator() {
        
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("FirstName is required")
            .MinimumLength(2)
            .WithMessage("FirstName must be at least 2 characters")
            .MaximumLength(100)
            .WithMessage("FirstName must be at least 100 characters");
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("LastName is required")
            .MinimumLength(2)
            .WithMessage("LastName must be at least 2 characters")
            .MaximumLength(100)
            .WithMessage("LastName must be at least 100 characters");
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("PhoneNumber is required")
            .MinimumLength(6)
            .WithMessage("PhoneNumber must be at least 2 characters")
            .MaximumLength(12)
            .WithMessage("PhoneNumber must be at least 12 characters");
        
        When(x => x.Address.Length > 0 && x.CountryId > 0, () => {
            RuleFor(x => x.Address)
                .MinimumLength(2)
                .WithMessage("LastName must be at least 2 characters")
                .MaximumLength(100)
                .WithMessage("LastName must be at least 100 characters");
        });
    }
}