using Domain.Entities;
namespace Domain.Interfaces.Mock; 

public interface ILanguagesMockService {
    IEnumerable<Languages> GetAll();
    Languages GetOne();
}