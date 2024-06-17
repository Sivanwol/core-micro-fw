using Domain.Entities;
using Domain.Persistence.Mock.Configs;
using Domain.Persistence.Mock.Services.Interfaces;
namespace Domain.Persistence.Mock.Services;

public class MockAppUserServices : IAppUserMockService {
    public MockAppUserServices() {
        Faker = new AppUserMockConfig();
        LanguagesFaker = new LanguagesMockConfig();
    }
    private AppUserMockConfig Faker { get; }
    private LanguagesMockConfig LanguagesFaker { get; set; }

    public ApplicationUser GetOne(Guid id) {
        var user = Faker.Generate(1).First();
        user.Id = id.ToString();
        return user;
    }
    public ApplicationUser GetOneByToken(string userToken) {
        var user = Faker.Generate(1).First();
        user.Token = userToken;
        return user;
    }
}