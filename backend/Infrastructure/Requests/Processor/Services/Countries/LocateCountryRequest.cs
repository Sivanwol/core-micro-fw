using Infrastructure.Responses.Controllers.Countries;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Countries;

public class LocateCountryRequest : IRequest<LocateCountryResponse> {
    public int CountryId { get; set; }
}