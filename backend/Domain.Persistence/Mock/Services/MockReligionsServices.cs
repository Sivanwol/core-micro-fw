using Domain.Entities;
using Domain.Interfaces.Mock;
using Domain.Persistence.Mock.Configs;
namespace Domain.Persistence.Mock.Services; 

public class MockReligionsServices: IReligionsMockService {
    private ReligionsMockConfig Faker { get; set; }
    public MockReligionsServices() {
        Faker = new ReligionsMockConfig();
    }
    public IEnumerable<Religions> GetAll() {
        return Faker.Generate(5);
    }
    public Religions GetOne() {
        return Faker.Generate(1).First();
    }
}