using Application.Utils;
using FluentValidation;
using Infrastructure.GQL.Inputs.View;
using Microsoft.IdentityModel.Tokens;
namespace Infrastructure.Validators.View;

public class ViewUpdateValidator : AbstractValidator<ViewUpdateInput> {
    public ViewUpdateValidator() {
        RuleFor(x => x.ViewClientKey)
            .NotEmpty()
            .WithMessage("ViewClientKey is required")
            .IsGuid()
            .WithMessage("ViewClientKey is not valid");
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MinimumLength(3)
            .WithMessage("Name must be at least 3 characters")
            .MaximumLength(100).WithMessage("Name must be less than 50 characters");
        When(x => !x.Description.IsNullOrEmpty(), () => {
            RuleFor(x => x.Description).MaximumLength(200).WithMessage("Description must be less than 200 characters");
        });
        When(x => !x.Color.IsNullOrEmpty(), () => {
            RuleFor(x => x.Color).MaximumLength(7).WithMessage("Color must be less than 7 characters");
            RuleFor(x => x.Color).Matches("^#(?:[0-9a-fA-F]{3}){1,2}$").WithMessage("Color must be a valid hex color");
        });
        RuleFor(x => x.IsShareAble).NotNull().WithMessage("IsShareAble is required");
    }
}