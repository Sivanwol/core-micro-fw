using Domain.Entities;
namespace Domain.Persistence.Interfaces.Mock;

public interface ILanguagesMockService {
    IEnumerable<Languages> GetAll();
    Languages GetOne();
}