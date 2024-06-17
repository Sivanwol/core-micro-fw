using Domain.Persistence.Mock.Services;
using Domain.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Test.Shared.Common;
namespace Domain.Persistence.Test.Repositories;

[TestFixture] [Description("Test the CountriesRepository")]
[Category("Countries")]
public class CountriesRepositoryTest : BaseTest {
    [SetUp]
    public void Setup() {
        SetupTest("CountriesRepositoryTest");
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

        factory = serviceProvider.GetService<ILoggerFactory>()!;
        logger = factory.CreateLogger<CountriesRepository>();
    }
    protected ILoggerFactory factory;
    protected ILogger logger;

    [Test] [Description("Test the GetAll method")]
    public async Task GetAll() {
        var mock = new MockCountiesServices();
        var mockData = mock.GetAll();
        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.Countries).Returns(mockObj.Object);
        var repository = new CountriesRepository(Context.Object, factory);
        var result = await repository.GetAll();
        Assert.That(result.Count(), Is.EqualTo(mockEnumerable.Count()));
        Assert.That(result.First().Id, Is.EqualTo(mockEnumerable.First().Id));
        Assert.That(result.First().CountryName, Is.EqualTo(mockEnumerable.First().CountryName));
    }

    [Test] [Description("Test the GetById method")]
    public async Task GetById() {
        var mock = new MockCountiesServices();
        var mockData = mock.GetAll();
        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.Countries).Returns(mockObj.Object);
        var repository = new CountriesRepository(Context.Object, factory);
        var result = await repository.GetById(mockEnumerable.First().Id);
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(mockEnumerable.First().Id));
        Assert.That(result!.CountryName, Is.EqualTo(mockEnumerable.First().CountryName));
    }

    [Test] [Description("Test the GetByCode method")]
    public async Task GetByCode() {
        var mock = new MockCountiesServices();
        var mockData = mock.GetAll();
        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.Countries).Returns(mockObj.Object);
        var repository = new CountriesRepository(Context.Object, factory);
        var result = await repository.GetByCode(mockEnumerable.First().CountryCode);
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(mockEnumerable.First().Id));
        Assert.That(result!.CountryName, Is.EqualTo(mockEnumerable.First().CountryName));
        Assert.That(result!.CountryCode, Is.EqualTo(mockEnumerable.First().CountryCode));
    }
}