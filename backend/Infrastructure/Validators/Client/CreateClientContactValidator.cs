using FluentValidation;
using Infrastructure.GQL.Inputs.Client;
namespace Infrastructure.Validators.Client;

public class CreateClientContactValidator : AbstractValidator<ClientContactInput> {
    public CreateClientContactValidator() {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithMessage("Client Id is required")
            .GreaterThan(0)
            .WithMessage("Client Id is not valid");
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First Name is required")
            .MaximumLength(75)
            .WithMessage("First Name is too long")
            .MinimumLength(2)
            .WithMessage("First Name is too short");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last Name is required")
            .MaximumLength(75)
            .WithMessage("Last Name is too long")
            .MinimumLength(2)
            .WithMessage("Last Name is too short");
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email is not valid");
        RuleFor(x => x.Phone1)
            .NotEmpty()
            .WithMessage("Phone 1 is required")
            .MaximumLength(20)
            .WithMessage("Phone 1 is too long")
            .MinimumLength(3)
            .WithMessage("Phone 1 is too short");
        When(x => x.Phone2 != null, () => {
            RuleFor(x => x.Phone2)
                .MaximumLength(20)
                .WithMessage("Phone 2 is too long");
        });
        When(x => x.Fax != null, () => {
            RuleFor(x => x.Fax)
                .MaximumLength(20)
                .WithMessage("Fax is too long");
        });
        When(x => x.Whatsapp != null, () => {
            RuleFor(x => x.Whatsapp)
                .MaximumLength(20)
                .WithMessage("Whatsapp is too long");
        });
        RuleFor(x => x.CountryId)
            .NotEmpty()
            .WithMessage("Country Id is required")
            .GreaterThan(0)
            .WithMessage("Country Id is not valid");

        RuleFor(x => x.JobTitle).NotEmpty()
            .WithMessage("Job Title is required")
            .MaximumLength(100)
            .WithMessage("Job Title is too long")
            .MinimumLength(3)
            .WithMessage("Job Title is too short");
        When(x => x.Department != null, () => {
            RuleFor(x => x.Department)
                .MaximumLength(100)
                .WithMessage("Department is too long");
        });
        RuleFor(x => x.Company).NotEmpty()
            .WithMessage("Company is required")
            .MaximumLength(100)
            .WithMessage("Company is too long")
            .MinimumLength(3)
            .WithMessage("Company is too short");

        When(x => x.Notes != null, () => {
            RuleFor(x => x.Notes)
                .MaximumLength(5000)
                .WithMessage("Notes is too long");
        });
    }
}