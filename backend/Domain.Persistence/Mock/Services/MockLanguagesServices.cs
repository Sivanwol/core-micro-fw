using Domain.Entities;
using Domain.Interfaces.Mock;
using Domain.Persistence.Mock.Configs;
namespace Domain.Persistence.Mock.Services; 

public class MockLanguagesServices: ILanguagesMockService {
    private LanguagesMockConfig Faker { get; set; }
    public MockLanguagesServices() {
        Faker = new LanguagesMockConfig();
    }
    public IEnumerable<Languages> GetAll() {
        return Faker.Generate(3);
    }
    public Languages GetOne() {
        return Faker.Generate(1).First();
    }
}