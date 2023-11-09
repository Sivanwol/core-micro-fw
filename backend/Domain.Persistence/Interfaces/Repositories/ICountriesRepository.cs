using Domain.Entities;
namespace Domain.Persistence.Interfaces.Repositories;

public interface ICountriesRepository : IGenericRepository<Countries> {
    Task<IEnumerable<Countries>> GetAll();
    Task<Countries> GetByCode(string code);
}