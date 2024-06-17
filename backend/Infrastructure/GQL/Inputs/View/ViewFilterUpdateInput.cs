using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using GraphQL.AspNet.Attributes;
using Infrastructure.Enums;
using Infrastructure.Requests.Processor.Services.Views;
using Nest;
namespace Infrastructure.GQL.Inputs.View;

[Description("View Filter Update Object input")]
public class ViewFilterUpdateInput {
    [Description("View client key that is used to based the new view on")]
    [GraphField(TypeExpression = "Guid!")]
    [Required]
    public Guid FromViewClientKey { get; set; }
    
    [Description("View filter collection")]
    public IEnumerable<ViewFilterItemInput> Filters { get; set; }

    public UpdateViewFilterRequest ToProcessorEntity(string userId) {
        return new UpdateViewFilterRequest() {
            UserId = userId,
            ViewClientKey = FromViewClientKey,
            Filters = Filters.Select(x => x.ToProcessorEntity())
        };
    }
}

[Description("View filter collection input")]
public class ViewFilterItemInput {
    [Description("Filter field name")]
    [Required]
    public string FilterFieldName { get; set; }
    [Description("Filter field data type")]
    [Required]
    public EntityColumnDataType FilterDateType { get; set; }
    [Description("Filter field operation type")]
    [Required]
    public EntityColumnOperationType FilterFieldType { get; set; }
    [Description("Filter field collection operation type")]
    [Required]
    public FilterCollectionOperation FilterCollectionOperation { get; set; }
    [Description("Filter field operation")]
    [Required]
    public FilterOperations FilterOperations { get; set; }
    
    [Description("Filter value/s")]
    [Required]
    public string Values { get; set; }

    public UpdateViewFilterItem ToProcessorEntity() {
        return new UpdateViewFilterItem {
            FilterFieldName = FilterFieldName,
            FilterCollectionOperation = FilterCollectionOperation,
            FilterDateType = FilterDateType,
            FilterOperations = FilterOperations,
            FilterFieldType = FilterFieldType,
            Values = Values
        };
    }
}