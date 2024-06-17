using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GraphQL.AspNet.Attributes;
using Infrastructure.Requests.Processor.Services.Views;
namespace Infrastructure.GQL.Inputs.View;

[Description("View Update Object input (will not update columns)")]
public class ViewUpdateInput {

    [Description("View client key that will be updated")]
    [GraphField(TypeExpression = "Guid!")]
    [Required]
    public Guid ViewClientKey { get; set; }

    [Description("View name")]
    [GraphField(TypeExpression = "String!")]
    [Required]
    public string Name { get; set; }

    [Description("View Description")]
    [GraphField(TypeExpression = "String")]
    public string? Description { get; set; }

    [Description("Is view shareable")]
    [GraphField(TypeExpression = "Boolean!")]
    public bool IsShareAble { get; set; }

    [Description("View Color (hex) (e.g. #000000) this will show as small dot in the view list")]
    [GraphField(TypeExpression = "String")]
    public string? Color { get; set; }

    public UpdateViewRequest ToProcessorEntity(string userId) {
        return new UpdateViewRequest() {
            UserId = userId,
            ViewClientKey = ViewClientKey,
            Name = Name,
            Description = Description,
            IsShareAble = IsShareAble,
            Color = Color
        };
    }
}