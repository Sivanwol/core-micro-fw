using Domain.Context;
using Domain.Entities;
using MockQueryable.Moq;
using Moq;

namespace Processor.test.Common;

public static class MockTestHelper {
    private static Mock<IDomainContext> Context = null;

    public static Mock<IDomainContext> SetupContext() {
        var context = SetupEntities();
        Context.Setup(x => x.Instance.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        return context;
    }

    private static Mock<IDomainContext> SetupEntities() {
        Context = new Mock<IDomainContext>();
        var users = GetUsers();

        Context.SetupProperty(c => c.Users, users.AsQueryable().BuildMockDbSet().Object);
        return Context;
    }

    public static List<User> GetUsers() {
        return new List<User> {
            new User { },
            new User { }
        };
    }

    // private static List<Game> GetGames(List<Client> clients) {
    //     return new List<Game> {
    //         new Game {
    //             Id = 1,
    //             ClientId = clients.First().Id,
    //             Client = clients.First(),
    //             Name = "Game 1",
    //             Theme = "Theme 1",
    //             PlatformType = PlatformType.PC,
    //             GameType = GameType.Action,
    //             ActiveAt = SystemClock.Now(),
    //             DeletedAt = null,
    //             CreatedAt = SystemClock.Now(),
    //             UpdatedAt = SystemClock.Now()
    //         },
    //         new Game {
    //             Id = 2,
    //             ClientId = clients.First().Id,
    //             Client = clients.First(),
    //             Name = "Game 2",
    //             Theme = "Theme 2",
    //             PlatformType = PlatformType.PS4,
    //             GameType = GameType.Adventure,
    //             ActiveAt = SystemClock.Now(),
    //             DeletedAt = null,
    //             CreatedAt = SystemClock.Now(),
    //             UpdatedAt = SystemClock.Now()
    //         },
    //         new Game {
    //             Id = 3,
    //             ClientId = clients.Last().Id,
    //             Client = clients.Last(),
    //             Name = "Game 3",
    //             Theme = "Theme 3",
    //             PlatformType = PlatformType.PS4,
    //             GameType = GameType.Adventure,
    //             ActiveAt = SystemClock.Now(),
    //             DeletedAt = null,
    //             CreatedAt = SystemClock.Now(),
    //             UpdatedAt = SystemClock.Now()
    //         },
    //         new Game {
    //             Id = 4,
    //             ClientId = clients.Last().Id,
    //             Client = clients.Last(),
    //             Name = "Game 4",
    //             Theme = "Theme 4",
    //             PlatformType = PlatformType.PS5,
    //             GameType = GameType.BoardGames,
    //             ActiveAt = SystemClock.Now(),
    //             DeletedAt = null,
    //             CreatedAt = SystemClock.Now(),
    //             UpdatedAt = SystemClock.Now()
    //         },
    //         new Game {
    //             Id = 5,
    //             ClientId = clients.Last().Id,
    //             Client = clients.Last(),
    //             Name = "Game 5",
    //             Theme = "Theme 4",
    //             PlatformType = PlatformType.PS4,
    //             GameType = GameType.Educational,
    //             ActiveAt = SystemClock.Now(),
    //             DeletedAt = null,
    //             CreatedAt = SystemClock.Now(),
    //             UpdatedAt = SystemClock.Now()
    //         },
    //         new Game {
    //             Id = 6,
    //             ClientId = clients.Last().Id,
    //             Client = clients.Last(),
    //             Name = "Game 5",
    //             Theme = "Theme 3",
    //             PlatformType = PlatformType.PS4,
    //             GameType = GameType.Adventure,
    //             ActiveAt = SystemClock.Now(),
    //             DeletedAt = null,
    //             CreatedAt = SystemClock.Now(),
    //             UpdatedAt = SystemClock.Now()
    //         },
    //         new Game {
    //             Id = 7,
    //             ClientId = clients.Last().Id,
    //             Client = clients.Last(),
    //             Name = "Game 5.5",
    //             Theme = "Theme 3",
    //             PlatformType = PlatformType.XBOX,
    //             GameType = GameType.Adventure,
    //             ActiveAt = SystemClock.Now(),
    //             DeletedAt = null,
    //             CreatedAt = SystemClock.Now(),
    //             UpdatedAt = SystemClock.Now()
    //         }
    //     };
    // }
    //
    // private static List<GameMetaData> GetGamesMetaData(List<Game> games) {
    //     var entities = new List<GameMetaData>();
    //     games.ForEachWithIndex<Game>((item, index) => {
    //         entities.Add(new GameMetaData {
    //             Id = index + 1,
    //             GameId = item.Id,
    //             Game = item,
    //             Key = $"Key_{index + 1}",
    //             Value = $"Value_{index + 1}"
    //         });
    //     });
    //     return entities;
    // }
}