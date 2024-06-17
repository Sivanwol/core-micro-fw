using Domain.Entities;
using Domain.Persistence.Context;
using Domain.Persistence.Repositories.Common;
using Domain.Persistence.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
namespace Domain.Persistence.Repositories;

public class LanguagesRepository : BaseRepository, ILanguagesRepository {
    private readonly ILogger _logger;
    public LanguagesRepository(
        IDomainContext context,
        ILoggerFactory loggerFactory
    ) : base(context) {
        _logger = loggerFactory.CreateLogger<LanguagesRepository>();
    }

    public async Task<IEnumerable<Languages>> GetAll() {
        _logger.LogInformation("Fetching all languages");
        var result = Context.Languages.Select(row => row).ToList();
        return result;
    }
    public async Task<Languages?> GetByCode(string code) {
        _logger.LogInformation($"Fetching language with code {code}");
        var result = Context.Languages.FirstOrDefault(row => row.Code == code);
        return result;
    }

    public async Task<Languages?> GetById(int id) {
        _logger.LogInformation($"Fetching language with id {id}");
        var result = Context.Languages.FirstOrDefault(row => row.Id == id);
        return result;
    }
}