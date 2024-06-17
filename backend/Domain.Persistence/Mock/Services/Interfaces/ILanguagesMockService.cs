using Domain.Entities;
namespace Domain.Persistence.Mock.Services.Interfaces;

public interface ILanguagesMockService {
    IEnumerable<Languages> GetAll();
    Languages GetOne();
}