using Application.Utils;
using FluentValidation;
using Infrastructure.GQL.Inputs.Common;
namespace Infrastructure.Validators.Common;

public class EntityPageValidator : AbstractValidator<EntityPage> {
    public EntityPageValidator() {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0");
        RuleFor(x => x.PageSize)
            .IsInEnum()
            .WithMessage("PageSize is not valid");
        RuleFor(x => x.ViewClientId)
            .NotEmpty()
            .WithMessage("ViewClientId is required")
            .IsGuid()
            .WithMessage("ViewClientId is not valid")
            .NotEqual(Guid.Empty)
            .WithMessage("ViewClientId is required");
        RuleFor(x => x.SortByField)
            .NotEmpty()
            .WithMessage("SortByField is required")
            .MaximumLength(500)
            .WithMessage("SortByField must not exceed 500 characters");
        RuleFor(x => x.SortDirection)
            .IsInEnum()
            .WithMessage("SortDirection is not valid");
        RuleForEach(x => x.Filters)
            .SetValidator(new EntityFilterItemValidator());
    }
}
