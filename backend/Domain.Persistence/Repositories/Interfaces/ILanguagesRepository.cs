using Domain.Entities;
using Domain.Persistence.Repositories.Common.Interfaces;
namespace Domain.Persistence.Repositories.Interfaces;

public interface ILanguagesRepository : IGenericRepository<Languages, int> {
    Task<IEnumerable<Languages>> GetAll();
    Task<Languages?> GetByCode(string code);
}