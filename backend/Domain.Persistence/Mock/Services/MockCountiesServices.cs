using Domain.Entities;
using Domain.Interfaces.Mock;
using Domain.Persistence.Mock.Configs;
namespace Domain.Persistence.Mock.Services; 

public class MockCountiesServices: ICountiesMockService {
    private CountriesMockConfig Faker { get; set; }
    public MockCountiesServices() {
        Faker = new CountriesMockConfig();
    }
    public IEnumerable<Countries> GetAll() {
        return Faker.Generate(20);
    }
    public Countries GetOne() {
        return Faker.Generate(1).First();
    }
}