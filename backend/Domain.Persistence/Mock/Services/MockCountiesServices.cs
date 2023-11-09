using Domain.Entities;
using Domain.Persistence.Interfaces.Mock;
using Domain.Persistence.Mock.Configs;
namespace Domain.Persistence.Mock.Services;

public class MockCountiesServices : ICountiesMockService {
    public MockCountiesServices() {
        Faker = new CountriesMockConfig();
    }
    private CountriesMockConfig Faker { get; set; }
    public IEnumerable<Countries> GetAll() {
        return Faker.Generate(20);
    }
    public Countries GetOne() {
        return Faker.Generate(1).First();
    }
}