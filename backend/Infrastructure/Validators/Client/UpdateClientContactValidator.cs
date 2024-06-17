using FluentValidation;
using Infrastructure.GQL.Inputs.Client;
namespace Infrastructure.Validators.Client;

public class UpdateClientContactValidator : AbstractValidator<ClientContactUpdateInput> {
    public UpdateClientContactValidator() {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithMessage("Client Id is required")
            .GreaterThan(0)
            .WithMessage("Client Id is not valid");
        When(x => !string.IsNullOrEmpty(x.FirstName), () => {
            RuleFor(x => x.FirstName)
                .MaximumLength(75)
                .WithMessage("First Name is too long")
                .MinimumLength(2)
                .WithMessage("First Name is too short");
        });

        When(x => !string.IsNullOrEmpty(x.LastName), () => {
            RuleFor(x => x.LastName)
                .MaximumLength(75)
                .WithMessage("Last Name is too long")
                .MinimumLength(2)
                .WithMessage("Last Name is too short");
        });

        When(x => !string.IsNullOrEmpty(x.Email), () => {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Email is not valid");
        });

        When(x => !string.IsNullOrEmpty(x.Phone1), () => {
            RuleFor(x => x.Phone1)
                .MaximumLength(20)
                .WithMessage("Phone 1 is too long")
                .MinimumLength(3)
                .WithMessage("Phone 1 is too short");
        });

        When(x => !string.IsNullOrEmpty(x.Phone2), () => {
            RuleFor(x => x.Phone2)
                .MaximumLength(20)
                .WithMessage("Phone 2 is too long");
        });

        When(x => !string.IsNullOrEmpty(x.Fax), () => {
            RuleFor(x => x.Fax)
                .MaximumLength(20)
                .WithMessage("Fax is too long");
        });

        When(x => !string.IsNullOrEmpty(x.Whatsapp), () => {
            RuleFor(x => x.Whatsapp)
                .MaximumLength(20)
                .WithMessage("Whatsapp is too long");
        });

        When(x => x.CountryId != null, () => {
            RuleFor(x => x.CountryId)
                .GreaterThan(0)
                .WithMessage("Country Id is not valid");
        });

        When(x => !string.IsNullOrEmpty(x.PostalCode), () => {
            RuleFor(x => x.PostalCode)
                .MaximumLength(10)
                .WithMessage("Postal Code is too long");
        });

        When(x => !string.IsNullOrEmpty(x.Address), () => {
            RuleFor(x => x.Address)
                .MaximumLength(100)
                .WithMessage("Address is too long");
        });

        When(x => !string.IsNullOrEmpty(x.City), () => {
            RuleFor(x => x.City)
                .MaximumLength(100)
                .WithMessage("City is too long");
        });

        When(x => !string.IsNullOrEmpty(x.State), () => {
            RuleFor(x => x.State)
                .MaximumLength(100)
                .WithMessage("State ia too long");
        });

        When(x => !string.IsNullOrEmpty(x.JobTitle), () => {
            RuleFor(x => x.JobTitle)
                .MaximumLength(100)
                .WithMessage("Job Title is too long")
                .MinimumLength(2)
                .WithMessage("Job Title is too short");
        });

        When(x => !string.IsNullOrEmpty(x.Department), () => {
            RuleFor(x => x.Department)
                .MaximumLength(100)
                .WithMessage("Department is too long");
        });

        When(x => !string.IsNullOrEmpty(x.Company), () => {
            RuleFor(x => x.Company)
                .MaximumLength(100)
                .WithMessage("Company is too long")
                .MinimumLength(2)
                .WithMessage("Company is too short");
        });

        When(x => !string.IsNullOrEmpty(x.Notes), () => {
            RuleFor(x => x.Notes)
                .MaximumLength(1000)
                .WithMessage("Notes is too long");
        });
    }
}