using Domain.Entities;
using Domain.Interfaces.Mock;
using Domain.Persistence.Mock.Configs;
namespace Domain.Persistence.Mock.Services; 

public class MockAppUserServices: IAppUserMockService {
    private AppUserMockConfig Faker { get; set; }
    public MockAppUserServices() {
        Faker = new AppUserMockConfig();
    }

    public IEnumerable<Users> GetPicks() {
        return Faker.Generate(3);
    }
    public IEnumerable<Users> GetSessionHistory() {
        return Faker.Generate(10);
    }
    public Users GetOne() {
        return Faker.Generate(1).First();
    }
}