using Domain.Entities;
using Domain.Interfaces.Mock;
using Domain.Interfaces.Repositories;
using Domain.Persistence.Context;
using Microsoft.Extensions.Logging;
using Domain.Persistence.Repositories.Common;
namespace Domain.Persistence.Repositories; 

public class ReligionsRepository: BaseRepository, IReligionsRepository {
    private readonly ILogger _logger;
    private readonly IReligionsMockService _mock;
    public ReligionsRepository(
        IDomainContext context,
        ILoggerFactory loggerFactory,
        IReligionsMockService mock
        ) : base(context) {
        _logger = loggerFactory.CreateLogger<ReligionsRepository>();
        _mock = mock;
    }

    public Task<IEnumerable<Religions>> GetAll() {
        // return await Context.Countries.ToListAsync();
        _logger.LogInformation("Fetching all religions");
        return Task.FromResult(_mock.GetAll());
    }
    
    public Task<Religions> GetById(int id) {
        // return Context.Countries.FirstOrDefaultAsync(c => c.ID == id);
        _logger.LogInformation($"Fetching religion with id {id}");
        return Task.FromResult(_mock.GetOne());
    }
}