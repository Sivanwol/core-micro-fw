using Domain.Entities;
namespace Domain.Interfaces.Repositories; 

public interface ILanguagesRepository : IGenericRepository<Languages> {
    Task<IEnumerable<Languages>> GetAll();
}