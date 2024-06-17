using Domain.Entities;
using Domain.Persistence.Mock.Configs;
using Domain.Persistence.Mock.Services.Interfaces;
namespace Domain.Persistence.Mock.Services;

public class MockCountiesServices : ICountiesMockService {
    public MockCountiesServices() {
        Faker = new CountriesMockConfig();
    }
    private CountriesMockConfig Faker { get; }
    public IEnumerable<Countries> GetAll() {
        return Faker.Generate(20);
    }
    public Countries GetOne() {
        return Faker.Generate(1).First();
    }
}