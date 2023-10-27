using Domain.Entities;
using Domain.Interfaces.Mock;
using Domain.Interfaces.Repositories;
using Domain.Persistence.Context;
using Microsoft.Extensions.Logging;
using Domain.Persistence.Repositories.Common;
namespace Domain.Persistence.Repositories; 

public class EthnicitiesRepository: BaseRepository, IEthnicitiesRepository {
    private readonly ILogger _logger;
    private readonly IEthnicitiesMockService _mock;
    public EthnicitiesRepository(
        IDomainContext context,
        ILoggerFactory loggerFactory,
        IEthnicitiesMockService mock
        ) : base(context) {
        _logger = loggerFactory.CreateLogger<EthnicitiesRepository>();
        _mock = mock;
    }

    public Task<IEnumerable<Ethnicities>> GetAll() {
        // return await Context.Countries.ToListAsync();
        _logger.LogInformation("Fetching all ethnicities");
        return Task.FromResult(_mock.GetAll());
    }
    
    public Task<Ethnicities> GetById(int id) {
        // return Context.Countries.FirstOrDefaultAsync(c => c.ID == id);
        _logger.LogInformation($"Fetching ethnicity with id {id}");
        return Task.FromResult(_mock.GetOne());
    }
}