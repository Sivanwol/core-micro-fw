using Domain.Entities;
using Domain.Interfaces.Mock;
using Domain.Interfaces.Repositories;
using Domain.Persistence.Context;
using Microsoft.Extensions.Logging;
using Domain.Persistence.Repositories.Common;
namespace Domain.Persistence.Repositories; 

public class CountriesRepository: BaseRepository, ICountriesRepository {
    private readonly ILogger _logger;
    private readonly ICountiesMockService _mock;
    public CountriesRepository(
        IDomainContext context,
        ILoggerFactory loggerFactory,
        ICountiesMockService mock
        ) : base(context) {
        _logger = loggerFactory.CreateLogger<CountriesRepository>();
        _mock = mock;
    }

    public Task<IEnumerable<Countries>> GetAll() {
        // return await Context.Countries.ToListAsync();
        _logger.LogInformation("Fetching all countries");
        return Task.FromResult(_mock.GetAll());
    }
    
    public Task<Countries> GetById(int id) {
        // return Context.Countries.FirstOrDefaultAsync(c => c.ID == id);
         
        _logger.LogInformation($"Fetching country with id {id}"); 
        return Task.FromResult(_mock.GetOne());
    }
}