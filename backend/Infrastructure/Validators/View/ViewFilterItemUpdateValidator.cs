using FluentValidation;
using Infrastructure.Enums;
using Infrastructure.GQL.Inputs.View;
namespace Infrastructure.Validators.View;

public class ViewFilterItemUpdateValidator:  AbstractValidator<ViewFilterItemInput> {
    public ViewFilterItemUpdateValidator() {
        RuleFor(x => x.FilterFieldName)
            .NotEmpty()
            .WithMessage("Field Name is required")
            .MinimumLength(2)
            .WithMessage("Field Name must be at least 3 characters")
            .MaximumLength(100).WithMessage("Field Name must be less than 100 characters");
        RuleFor(x => x.FilterOperations)
            .IsInEnum()
            .WithMessage("Filter Operations Value not match");
        RuleFor(x => x.FilterCollectionOperation)
            .IsInEnum()
            .WithMessage("Filter Collection Operations Value not match");
        RuleFor(x => x.FilterDateType)
            .IsInEnum()
            .WithMessage("Filter Data type Value not match");
        RuleFor(x => x.FilterFieldType)
            .IsInEnum()
            .WithMessage("Filter Field type Value not match");
        
    }
}