using Domain.Entities;
namespace Domain.Persistence.Interfaces.Repositories;

public interface ILanguagesRepository : IGenericRepository<Languages> {
    Task<IEnumerable<Languages>> GetAll();
}