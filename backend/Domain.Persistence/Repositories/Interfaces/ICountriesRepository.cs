using Domain.Entities;
using Domain.Persistence.Repositories.Common.Interfaces;
namespace Domain.Persistence.Repositories.Interfaces;

public interface ICountriesRepository : IGenericRepository<Countries, int> {
    Task<IEnumerable<Countries>> GetAll();
    Task<Countries?> GetByCode(string code);
}