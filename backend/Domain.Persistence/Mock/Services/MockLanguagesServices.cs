using Domain.Entities;
using Domain.Persistence.Mock.Configs;
using Domain.Persistence.Mock.Services.Interfaces;
namespace Domain.Persistence.Mock.Services;

public class MockLanguagesServices : ILanguagesMockService {
    public MockLanguagesServices() {
        Faker = new LanguagesMockConfig();
    }
    private LanguagesMockConfig Faker { get; }
    public IEnumerable<Languages> GetAll() {
        return Faker.Generate(3);
    }
    public Languages GetOne() {
        return Faker.Generate(1).First();
    }
}