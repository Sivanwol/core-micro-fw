using Infrastructure.Responses.Processor.Countries;
using MediatR;

namespace Infrastructure.Requests.Processor.Countries;

public class LocateCountryRequest : IRequest<LocateCountryResponse> {
    public int CountryId { get; set; }
}