using Infrastructure.Enums;
using Infrastructure.GQL;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Views;

public class GetAvailableColumnsForViewRequest : IRequest<ICollection<ViewColumn>> {
    public string UserId { get; set; }
    public EntityTypes EntityType { get; set; }
}