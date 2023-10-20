using Domain.Entities;
namespace Domain.Interfaces;

public interface ICountiesMockService {
    public List<Countries> GetCountries();
}