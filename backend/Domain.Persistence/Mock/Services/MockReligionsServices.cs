using Domain.Entities;
using Domain.Persistence.Interfaces.Mock;
using Domain.Persistence.Mock.Configs;
namespace Domain.Persistence.Mock.Services;

public class MockReligionsServices : IReligionsMockService {
    public MockReligionsServices() {
        Faker = new ReligionsMockConfig();
    }
    private ReligionsMockConfig Faker { get; set; }
    public IEnumerable<Religions> GetAll() {
        return Faker.Generate(5);
    }
    public Religions GetOne() {
        return Faker.Generate(1).First();
    }
}