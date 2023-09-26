using Application.Utils;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Processor.Consumers.IndexUser;
using Processor.Handlers.User.List;
using Processor.test.Common;

namespace Processor.test.Users;

[TestFixture]
public class UsersProcessTest : BaseTest {
    [SetUp]
    public void Setup() {
        SetupTest("Users");
    }

    [Test]
    public async Task GetUserListTest() {
        ReloadData();
        var requester = new ListUsersRequest();
        var handler = new ListUsersHandler(Mediator.Object, Context.Object);
        var response = await handler.Handle(requester, CancellationToken.None);
        var exptendedEntities = MockTestHelper.GetUsers();
        Assert.That(response.Count, Is.EqualTo(exptendedEntities.Count));
        Assert.That(response[0].Id, Is.EqualTo(exptendedEntities[0].Id));
        Assert.That(response[0].Auth0Id, Is.EqualTo(exptendedEntities[0].Auth0Id));
        Assert.That(response[1].Id, Is.EqualTo(exptendedEntities[1].Id));
        Assert.That(response[1].Auth0Id, Is.EqualTo(exptendedEntities[1].Auth0Id));
    }

    [Test]
    public async Task IndexNewUser() {
        var user = MockTestHelper.GetUsers()[0];
        var provider = new ServiceCollection()
            .AddMassTransitInMemoryTestHarness(cfg => {
                cfg.AddDelayedMessageScheduler();
                cfg.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));
                cfg.AddConsumers(typeof(IndexUserConsumerHandler));
                cfg.UsingRabbitMq((context, cfg) => { cfg.ConfigureEndpoints(context); });
            })
            .BuildServiceProvider(true);
        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        var client = harness.GetRequestClient<IndexUserEvent>();
        var response = await client.GetResponse<IndexUserEvent>(new IndexUserRequest {
            Auth0Id = user.Auth0Id,
            Id = Guid.NewGuid().ToString(),
            UpdateTime = SystemClock.Now().DateTime
        });

        Assert.IsTrue(await harness.Sent.Any<IndexUserEvent>());

        Assert.IsTrue(await harness.Consumed.Any<IndexUserEvent>());
        Assert.NotNull(response);
        Assert.NotNull(response.Message);
        Assert.Equals(response.Message.User.Auth0Id, user.Auth0Id);
    }
}