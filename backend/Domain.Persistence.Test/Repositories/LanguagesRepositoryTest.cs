using Domain.Persistence.Mock.Services;
using Domain.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Test.Shared.Common;
namespace Domain.Persistence.Test.Repositories;

[TestFixture] [Description("Test the LanguagesRepository")]
[Category("Languages")]
public class LanguagesRepositoryTest : BaseTest {
    [SetUp]
    public void Setup() {
        SetupTest("LanguagesRepositoryTest");
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

        factory = serviceProvider.GetService<ILoggerFactory>()!;
        logger = factory.CreateLogger<LanguagesRepository>();
    }
    protected ILoggerFactory factory;
    protected ILogger logger;

    [Test] [Description("Test the GetAll method")]
    public async Task GetAll() {
        var mock = new MockLanguagesServices();
        var mockData = mock.GetAll();
        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.Languages).Returns(mockObj.Object);
        var repository = new LanguagesRepository(Context.Object, factory);
        var result = await repository.GetAll();
        Assert.That(result.Count(), Is.EqualTo(mockEnumerable.Count()));
        Assert.That(result.First().Id, Is.EqualTo(mockEnumerable.First().Id));
        Assert.That(result.First().Name, Is.EqualTo(mockEnumerable.First().Name));
    }

    [Test] [Description("Test the GetById method")]
    public async Task GetById() {
        var mock = new MockLanguagesServices();
        var mockData = mock.GetAll();
        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.Languages).Returns(mockObj.Object);
        var repository = new LanguagesRepository(Context.Object, factory);
        var result = await repository.GetById(mockEnumerable.First().Id);
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(mockEnumerable.First().Id));
        Assert.That(result!.Name, Is.EqualTo(mockEnumerable.First().Name));
    }

    [Test] [Description("Test the GetByCode method")]
    public async Task GetByCode() {
        var mock = new MockLanguagesServices();
        var mockData = mock.GetAll();
        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.Languages).Returns(mockObj.Object);
        var repository = new LanguagesRepository(Context.Object, factory);
        var result = await repository.GetByCode(mockEnumerable.First().Code);
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(mockEnumerable.First().Id));
        Assert.That(result!.Name, Is.EqualTo(mockEnumerable.First().Name));
    }
}