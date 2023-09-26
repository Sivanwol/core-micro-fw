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
}