using Domain.Entities;
namespace Domain.Persistence.Mock.Services.Interfaces;

public interface ICountiesMockService {
    public IEnumerable<Countries> GetAll();
    public Countries GetOne();
}