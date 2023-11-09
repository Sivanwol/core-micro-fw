using Domain.Persistence.Mock.Services;
using Domain.Persistence.Repositories;
using Infrastructure.Requests.Processor.Services.Countries;
using Processor.Services.Countries.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Test.Shared.Common;
namespace Processor.Test.Services.Counties; 

[TestFixture]
public class LocateHandlerTest  : BaseTest {
    protected ILoggerFactory factory;
    protected ILogger logger;
    
    [SetUp]
    public void Setup() {
        SetupTest("LocateHandlerTest");
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();
        factory = serviceProvider.GetService<ILoggerFactory>()!;
        logger = factory.CreateLogger<LocateHandlerTest>();
    }
    
    [Test]
    public async Task LocateCountryTest() {
        Assert.Ignore();
        // var requester = new LocateCountryRequest {
        //     CountryId = MockTestHelper.Countries.First().ID
        // };
        var countriesRepository = new CountriesRepository(Context.Object, factory, new MockCountiesServices());
        // var handler = new LocateHandler(Mediator.Object, Context.Object, factory, countriesRepository);
        // var response = await handler.Handle(requester, CancellationToken.None);
        // Assert.That(response.IsFound, Is.EqualTo(true));
        // Assert.That(response.Record, Is.EqualTo(MockTestHelper.Countries.First()));
    }
}