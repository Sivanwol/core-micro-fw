using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GraphQL.AspNet.Attributes;
using Infrastructure.Requests.Processor.Services.Views;
namespace Infrastructure.GQL.Inputs.View;

[Description("View Update Columns Object input")]
public class ViewUpdateColumnsInput {
    [Description("View client key that is will update columns")]
    [GraphField(TypeExpression = "Guid!")]
    [Required]
    public Guid ViewClientKey { get; set; }

    [Description("View Columns (will replace all columns)")]
    [GraphField(TypeExpression = "[Int!]!")]
    [Required]
    public IEnumerable<int> Columns { get; set; }

    public UpdateViewColumnsRequest ToProcessorEntity(string userId) {
        return new UpdateViewColumnsRequest() {
            UserId = userId,
            ViewClientKey = ViewClientKey,
            Columns = Columns
        };
    }
}