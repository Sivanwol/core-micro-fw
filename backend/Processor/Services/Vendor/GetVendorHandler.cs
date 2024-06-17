using Application.Exceptions;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.GQL;
using Infrastructure.GQL.Inputs.Media;
using Infrastructure.Requests.Processor.Services.User;
using Infrastructure.Requests.Processor.Services.Vendor;
using Infrastructure.Services.S3;
using MediatR;
using Serilog;

namespace Processor.Services.Vendor;

public class GetVendorHandler : IRequestHandler<GetVendorRequest, Infrastructure.GQL.Vendor?>
{
    private readonly IMediator _mediator;
    private readonly IVendorRepository _vendorRepository;
    private readonly IStorageControlService _storageControlService;
    public GetVendorHandler(IMediator mediator,
    IStorageControlService storageControlService,
    IVendorRepository vendorRepository)
    {
        _storageControlService = storageControlService;
        _mediator = mediator;
        _vendorRepository = vendorRepository;
    }

    public async Task<Infrastructure.GQL.Vendor?> Handle(GetVendorRequest request, CancellationToken cancellationToken)
    {
        Log.Logger.Information($"GetVendorHandler: {request.VendorId}");
        var vendor = await _vendorRepository.GetVendor(request.VendorId);
        return vendor.ToGql();
    }
}
