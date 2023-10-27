using Domain.Entities;
using Domain.Persistence.Context;
using Domain.Persistence.Mock.Configs;
using MockQueryable.Moq;
using Moq;
namespace Test.Shared.Common;

public static class MockTestHelper {
    private static Mock<IDomainContext> Context = null;
    public static List<Countries> Countries { get; set; } = new List<Countries>();

    public static Mock<IDomainContext> SetupContext() {
        var context = SetupEntities();
        Context.Setup(x => x.Instance.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        return context;
    }

    private static Mock<IDomainContext> SetupEntities() {
        Context = new Mock<IDomainContext>();
        var users = GetUsers();
        var countries = GetCountries();
        Countries = countries;
        Context.SetupProperty(c => c.Countries, countries.AsQueryable().BuildMockDbSet().Object);
        // Context.SetupProperty(c => c.Users, users.AsQueryable().BuildMockDbSet().Object);
        return Context;
    }

    public static List<Countries> GetCountries() {
        var faker = new CountriesMockConfig();
        return faker.Generate(20);;
    }
    public static List<ApplicationUser> GetUsers() {
        return new List<ApplicationUser> {
            new ApplicationUser { },
            new ApplicationUser { }
        };
    }
}