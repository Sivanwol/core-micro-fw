using FluentValidation;
using Infrastructure.GQL.Inputs.Client;
namespace Infrastructure.Validators.Client;

public class CreateClientValidator : AbstractValidator<ClientInput> {
    public CreateClientValidator() {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.Website).NotEmpty().WithMessage("Website is required");
        RuleFor(x => x.CountryId)
            .NotEmpty()
            .WithMessage("Country is required")
            .GreaterThan(0)
            .WithMessage("Country must be valid id");
        RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required");
        RuleFor(x => x.City).NotEmpty().WithMessage("City is required");

        When(x => x.ParentId != null, () => {
            RuleFor(x => x.ParentId)
                .NotEmpty()
                .WithMessage("OwnClientId is required")
                .GreaterThan(0)
                .WithMessage("OwnClientId must be valid id");
        });
    }
}