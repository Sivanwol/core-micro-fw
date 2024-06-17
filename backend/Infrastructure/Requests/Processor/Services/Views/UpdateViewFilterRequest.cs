using Infrastructure.Enums;
using Infrastructure.GQL;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Views;

public class UpdateViewFilterRequest : IRequest<View> {
    public string UserId { get; set; }
    public Guid ViewClientKey { get; set; }
    public IEnumerable<UpdateViewFilterItem> Filters { get; set; }
}

public class UpdateViewFilterItem {
    public string FilterFieldName { get; set; }
    public EntityColumnDataType FilterDateType { get; set; }
    public EntityColumnOperationType FilterFieldType { get; set; }
    public FilterCollectionOperation FilterCollectionOperation { get; set; }
    public FilterOperations FilterOperations { get; set; }
    public string Values;
}