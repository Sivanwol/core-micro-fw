using Domain.Entities;
using Domain.Persistence.Interfaces.Mock;
using Domain.Persistence.Mock.Configs;
namespace Domain.Persistence.Mock.Services;

public class MockEthnicitiesServices : IEthnicitiesMockService {
    public MockEthnicitiesServices() {
        Faker = new EthnicitiesMockConfig();
    }
    private EthnicitiesMockConfig Faker { get; set; }
    public IEnumerable<Ethnicities> GetAll() {
        return Faker.Generate(5);
    }
    public Ethnicities GetOne() {
        return Faker.Generate(1).First();
    }
}