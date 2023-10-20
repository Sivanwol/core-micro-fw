using Domain.Interfaces.Repositories;
using Domain.Persistence.Context;
using Infrastructure.Requests.Processor.Services.Countries;
using Infrastructure.Responses.Processor.Services.Countries;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
namespace Processor.Services.Countries.Base;

public class LocateHandler : IRequestHandler<LocateCountryRequest, LocateCountryResponse> {
    private readonly IBus _bus;
    private readonly ILogger _logger;
    private readonly IMediator _mediator;
    private readonly ICountriesRepository _repository;
    public LocateHandler(
        IMediator mediator,
        IDomainContext context,
        ILoggerFactory loggerFactory,
        ICountriesRepository repository,
        IBus bus) {
        _mediator = mediator;
        _repository = repository;
        _bus = bus;
        _logger = loggerFactory.CreateLogger<LocateHandler>();
    }


    public async Task<LocateCountryResponse> Handle(LocateCountryRequest request,
        CancellationToken cancellationToken) {
        var result = await _repository.GetById(request.CountryId);
        return new LocateCountryResponse {
            IsFound = result != null,
            Record = result
        };
    }
}