using GraphQL.AspNet.Attributes;
using Infrastructure.Enums;
namespace Infrastructure.GQL;

public class ViewFilter {
    public int Id { get; set; }
    public EntityColumnDataType FilterDateType { get; set; }
    public EntityColumnOperationType FilterFieldType { get; set; }
    public FilterCollectionOperation FilterCollectionOperation { get; set; }
    public FilterOperations FilterOperations { get; set; }
    public string FilterFieldName { get; set; }
    public string Values { get; set; }
}