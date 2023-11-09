using Test.Shared.Common;
namespace Processor.Test.Services.Users;

[TestFixture]
public class UsersProcessTest : BaseTest {
    [SetUp]
    public void Setup() {
        SetupTest("UsersProcessTest");
    }

    [Test]
    public async Task GetUserListTest() {
        Assert.Ignore();
        // ReloadData();
        // var requester = new ListUsersRequest();
        // var handler = new ListUsersHandler(Mediator.Object, Context.Object);
        // var response = await handler.Handle(requester, CancellationToken.None);
        // var exptendedEntities = MockTestHelper.GetUsers();
        // Assert.That(response.Count, Is.EqualTo(exptendedEntities.Count));
        // Assert.That(response[0].Id, Is.EqualTo(exptendedEntities[0].Id));
        // Assert.That(response[1].Id, Is.EqualTo(exptendedEntities[1].Id));
    }

    [Test]
    public async Task IndexNewUserTest() {
        Assert.Ignore();
        // var user = MockTestHelper.GetUsers()[0];
        // await using var provider = new ServiceCollection()
        //     .AddMassTransitTestHarness(cfg => {
        //         cfg.AddDelayedMessageScheduler();
        //         cfg.AddConsumers(typeof(IndexUserConsumerHandler));
        //         cfg.UsingInMemory((context, cfg) => {
        //             cfg.UseDelayedMessageScheduler();
        //
        //             cfg.ConfigureEndpoints(context);
        //         });
        //     })
        //     .BuildServiceProvider(true);
        // var harness = provider.GetRequiredService<ITestHarness>();
        // await harness.Start();
        // try {
        //     var client = harness.GetRequestClient<IndexUserRequest>();
        //     var response = await client.GetResponse<IndexUserConsumerHandler>(new IndexUserRequest {
        //         Id = Guid.NewGuid().ToString(),
        //         UpdateTime = SystemClock.Now().DateTime
        //     });
        //
        //     Assert.Equals(await harness.Sent.Any<IndexUserRequest>(), true);
        //     Assert.Equals(await harness.Consumed.Any<IndexUserEvent>(), true);
        // }
        // catch (Exception exec) {
        //     var error = exec.Message;
        // }
        // finally {
        //     await harness.Stop();
        //     await provider.DisposeAsync();
        // }
    }
}