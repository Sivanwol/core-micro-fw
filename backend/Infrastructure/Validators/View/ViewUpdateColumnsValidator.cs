using Application.Utils;
using FluentValidation;
using Infrastructure.GQL.Inputs.View;
namespace Infrastructure.Validators.View;

public class ViewUpdateColumnsValidator : AbstractValidator<ViewUpdateColumnsInput> {
    public ViewUpdateColumnsValidator() {
        RuleFor(x => x.ViewClientKey)
            .NotEmpty()
            .WithMessage("ViewClientKey is required")
            .IsGuid()
            .WithMessage("ViewClientKey is not valid");
        RuleFor(x => x.Columns)
            .NotEmpty()
            .WithMessage("Columns is required")
            .Must(x => x.Count() >= 5)
            .WithMessage("Columns must be at least 5");

    }
}