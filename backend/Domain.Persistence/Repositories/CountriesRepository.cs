using Domain.Entities;
using Domain.Persistence.Context;
using Domain.Persistence.Repositories.Common;
using Domain.Persistence.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
namespace Domain.Persistence.Repositories;

public class CountriesRepository : BaseRepository, ICountriesRepository {
    private readonly ILogger _logger;
    public CountriesRepository(
        IDomainContext context,
        ILoggerFactory loggerFactory
    ) : base(context) {
        _logger = loggerFactory.CreateLogger<CountriesRepository>();
    }

    public async Task<IEnumerable<Countries>> GetAll() {
        _logger.LogInformation("Fetching all countries");
        var result = Context.Countries
            .Select(row => row)
            .Where(w => w.SupportedAt != null)
            .ToList();
        return result;
    }
    public async Task<Countries?> GetByCode(string code) {
        _logger.LogInformation($"Fetching country with code {code}");
        var result = Context.Countries.FirstOrDefault(row => row.SupportedAt != null && row.CountryCode == code);
        return result;
    }

    public async Task<Countries?> GetById(int id) {
        _logger.LogInformation($"Fetching country with id {id}");
        var result = Context.Countries.FirstOrDefault(row => row.SupportedAt != null && row.Id == id);
        return result;
    }
}