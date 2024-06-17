using System.ComponentModel;
namespace Infrastructure.GQL.Common;

[Description("Entity Result will show the result of the entity")]
public class EntityPage<T> where T : class {
    [Description("How many pages are there in total")]
    public int TotalPages { get; set; }

    [Description("How many records are there in total")]
    public int TotalRecords { get; set; }

    [Description("Do we have a next page")]
    public bool HasNextPage { get; set; }

    [Description("Do we have a previous page")]
    public bool HasPreviousPage { get; set; }

    [Description("Records of the entity")]
    public IEnumerable<T> Records { get; set; } = new List<T>();
}