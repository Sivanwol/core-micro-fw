using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace Domain.Persistence.Repositories;

public class CountriesRepository : BaseRepository, ICountriesRepository {
    private readonly ILogger _logger;
    public CountriesRepository(
        IDomainContext context,
        ILoggerFactory loggerFactory) : base(context) {
        _logger = loggerFactory.CreateLogger<CountriesRepository>();
    }

    public async Task<IEnumerable<Countries>> GetAll() {
        return await Context.Countries.ToListAsync();
    }

    public Task<Countries?> GetById(int id) {
        return Context.Countries.FirstOrDefaultAsync(c => c.ID == id);
    }
}