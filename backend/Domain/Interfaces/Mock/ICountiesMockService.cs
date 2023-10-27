using Domain.Entities;
namespace Domain.Interfaces.Mock; 

public interface ICountiesMockService {
    public IEnumerable<Countries> GetAll();
    public Countries GetOne();
}