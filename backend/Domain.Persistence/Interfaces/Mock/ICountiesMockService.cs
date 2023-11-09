using Domain.Entities;
namespace Domain.Persistence.Interfaces.Mock;

public interface ICountiesMockService {
    public IEnumerable<Countries> GetAll();
    public Countries GetOne();
}