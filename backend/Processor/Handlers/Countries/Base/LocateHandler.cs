using Domain.Context;
using Infrastructure.Requests.Processor.Countries;
using Infrastructure.Responses.Processor.Countries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Processor.Handlers.Countries.Base;

public class LocateHandler : IRequestHandler<LocateCountryRequest, LocateCountryResponse> {
    private readonly IDomainContext _context;
    private readonly IMediator _mediator;
    private readonly ILogger _logger;

    public LocateHandler(
        IMediator mediator,
        IDomainContext context,
        ILoggerFactory loggerFactory) {
        _context = context;
        _mediator = mediator;
        _logger = loggerFactory.CreateLogger<LocateHandler>();
    }


    public async Task<LocateCountryResponse> Handle(LocateCountryRequest request,
        CancellationToken cancellationToken) {
        var result = _context.Countries.FirstOrDefault(c => c.ID == request.CountryId);
        return new LocateCountryResponse {
            IsFound = result != null,
            Record = result
        };
    }
}