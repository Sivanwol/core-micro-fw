using Infrastructure.GQL;
using MediatR;
namespace Infrastructure.Requests.Processor.Services.Countries;

public class GetCountriesRequest : IRequest<IEnumerable<Country>> { }