using Domain.Entities;
using Domain.Persistence.Context;
using Domain.Persistence.Interfaces.Mock;
using Domain.Persistence.Interfaces.Repositories;
using Domain.Persistence.Repositories.Common;
using Microsoft.Extensions.Logging;
namespace Domain.Persistence.Repositories;

public class LanguagesRepository : BaseRepository, ILanguagesRepository {
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
        _logger.LogInformation("Fetching all languages");
        // return await Context.Countries.ToListAsync();
        return Task.FromResult(_mock.GetAll());
    }

    public Task<Languages> GetById(int id) {
        _logger.LogInformation($"Fetching language with id {id}");
        // return Context.Countries.FirstOrDefaultAsync(c => c.ID == id);
        return Task.FromResult(_mock.GetOne());
    }
}