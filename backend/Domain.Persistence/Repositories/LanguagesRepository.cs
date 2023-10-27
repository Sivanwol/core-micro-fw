using Domain.Entities;
using Domain.Interfaces.Mock;
using Domain.Interfaces.Repositories;
using Domain.Persistence.Context;
using Microsoft.Extensions.Logging;
using Domain.Persistence.Repositories.Common;
namespace Domain.Persistence.Repositories; 

public class LanguagesRepository: BaseRepository, ILanguagesRepository {
    private readonly ILogger _logger;
    private readonly ILanguagesMockService _mock;
    public LanguagesRepository(
        IDomainContext context,
        ILoggerFactory loggerFactory,
        ILanguagesMockService mock
        ) : base(context) {
        _logger = loggerFactory.CreateLogger<LanguagesRepository>();
        _mock = mock;
    }

    public Task<IEnumerable<Languages>> GetAll() {
        // return await Context.Countries.ToListAsync();
        _logger.LogInformation("Fetching all languages");
        return Task.FromResult(_mock.GetAll());
    }
    
    public Task<Languages> GetById(int id) {
        // return Context.Countries.FirstOrDefaultAsync(c => c.ID == id);
        _logger.LogInformation($"Fetching language with id {id}");
        return Task.FromResult(_mock.GetOne());
    }
}