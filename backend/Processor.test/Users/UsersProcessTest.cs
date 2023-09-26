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
    public async Task IndexNewUserTest() {
        var user = MockTestHelper.GetUsers()[0];
        await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness(cfg => {
                cfg.AddDelayedMessageScheduler();
                cfg.AddConsumers(typeof(IndexUserConsumerHandler));
                cfg.UsingInMemory((context, cfg) => {
                    cfg.UseDelayedMessageScheduler();

                    cfg.ConfigureEndpoints(context);
                });
            })
            .BuildServiceProvider(true);
        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();
        try {
            var client = harness.GetRequestClient<IndexUserRequest>();
            var response = await client.GetResponse<IndexUserConsumerHandler>(new IndexUserRequest {
                Id = Guid.NewGuid().ToString(),
                Auth0Id = user.Auth0Id,
                UpdateTime = SystemClock.Now().DateTime
            });

            Assert.True(await harness.Sent.Any<IndexUserRequest>());
            Assert.True(await harness.Consumed.Any<IndexUserEvent>());
        }
        catch (Exception exec) {
            var error = exec.Message;
        }
        finally {
            await harness.Stop();
            await provider.DisposeAsync();
        }
    }
}