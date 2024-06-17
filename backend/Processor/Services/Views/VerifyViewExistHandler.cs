using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.Requests.Processor.Services.Views;
using Infrastructure.Services.Cache;
using MediatR;
using Serilog;
namespace Processor.Services.Views;

public class VerifyViewExistHandler : IRequestHandler<VerifyViewExistRequest, bool> {
    private readonly IConfigurableUserViewRepository _configurableUserViewRepository;

    public VerifyViewExistHandler(IConfigurableUserViewRepository configurableUserViewRepository, ICacheService cacheService) {
        _configurableUserViewRepository = configurableUserViewRepository;
    }
    public async Task<bool> Handle(VerifyViewExistRequest request, CancellationToken cancellationToken) {
        Log.Logger.Information("verify view exist");
        return await _configurableUserViewRepository.VerifyViewExist(request);
    }
}