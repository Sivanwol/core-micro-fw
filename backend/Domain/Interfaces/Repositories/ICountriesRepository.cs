using Domain.Entities;
namespace Domain.Interfaces.Repositories; 

public interface ICountriesRepository : IGenericRepository<Countries> {
    Task<IEnumerable<Countries>> GetAll();
}