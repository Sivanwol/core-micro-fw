using FluentValidation;
using Infrastructure.Enums;
using Infrastructure.GQL.Inputs.Common;
namespace Infrastructure.Validators.Common;

public class EntityFilterItemValidator : AbstractValidator<EntityFilterItem> {

    public EntityFilterItemValidator() {
        RuleFor(x => x.FieldName)
            .NotEmpty()
            .WithMessage("FieldName is required")
            .MaximumLength(100)
            .WithMessage("FieldName must not exceed 100 characters");
        RuleFor(x => x.OperationType)
            .IsInEnum()
            .WithMessage("OperationType is not valid");
        RuleFor(x => x.FilterOperation)
            .IsInEnum()
            .WithMessage("FilterOperation is not valid");
        When(x => x.FilterOperation == FilterOperations.Between, () => {
            RuleFor(x => x.Values)
                .NotEmpty()
                .WithMessage("Values is required")
                .Must(x => x?.Count == 2)
                .WithMessage("Values must contain 2 items");
        });
        When(x => !IsSingleValueFilter(x.FilterOperation), () => {
            RuleFor(x => x.Values)
                .NotEmpty()
                .WithMessage("Values is required")
                .Must(x => x?.Count >= 1)
                .WithMessage("Values must contain at least 1 item");
        });

        // the single value filter not need be check as null can be happened in some case
    }

    private bool IsSingleValueFilter(FilterOperations filterOperation) {
        return filterOperation == FilterOperations.Equal ||
               filterOperation == FilterOperations.NotEqual ||
               filterOperation == FilterOperations.GreaterThan ||
               filterOperation == FilterOperations.GreaterThanOrEqual ||
               filterOperation == FilterOperations.LessThan ||
               filterOperation == FilterOperations.LessThanOrEqual ||
               filterOperation == FilterOperations.Contains ||
               filterOperation == FilterOperations.NotContains ||
               filterOperation == FilterOperations.StartsWith ||
               filterOperation == FilterOperations.EndsWith;
    }
}