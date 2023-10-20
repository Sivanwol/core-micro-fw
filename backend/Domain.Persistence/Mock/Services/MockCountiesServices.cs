using Domain.Entities;
using Domain.Interfaces;
using Domain.Persistence.Mock.Configs;
namespace Domain.Persistence.Mock.Services;

public class MockCountiesServices : ICountiesMockService {
    public List<Countries> GetCountries() {
        var faker = new CountriesMockConfig();
        return faker.Generate(20);
    }
}