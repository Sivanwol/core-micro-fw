using Domain.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Test.Shared.Common;
namespace Domain.Persistence.Test.Repositories;

public class CountriesRepositoryTest : BaseTest {
    protected ILoggerFactory factory;
    protected ILogger logger;
    [SetUp]
    public void Setup() {
        SetupTest("CountriesRepositoryTest");
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

        factory = serviceProvider.GetService<ILoggerFactory>()!;
        logger = factory.CreateLogger<CountriesRepository>();
    }

    [Test]
    public async Task GetCountries() {
        var countriesRepository = new CountriesRepository(Context.Object, factory);
        var countries = await countriesRepository.GetAll();
        Assert.That(countries.Count(), Is.EqualTo(20));
    }
}