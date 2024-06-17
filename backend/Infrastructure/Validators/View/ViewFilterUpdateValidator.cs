using Application.Utils;
using FluentValidation;
using Infrastructure.GQL.Inputs.View;
using Microsoft.IdentityModel.Tokens;
namespace Infrastructure.Validators.View;

public class ViewFilterUpdateValidator : AbstractValidator<ViewFilterUpdateInput> {
    public ViewFilterUpdateValidator() {
        RuleFor(x => x.FromViewClientKey)
            .NotEmpty()
            .WithMessage("FromViewClientKey is required")
            .IsGuid()
            .WithMessage("FromViewClientKey is not valid");
        When(f => f.Filters.Any(), () => {
            RuleForEach(x => x.Filters).SetValidator(new ViewFilterItemUpdateValidator());
        });
    }
}
