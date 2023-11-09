using AutoBogus;
using Bogus;
using Domain.Entities;
namespace Domain.Persistence.Mock.Configs;

public sealed class UserSessionMockConfig : AutoFaker<UserSession> {
    public UserSessionMockConfig() {
        Randomizer.Seed = new Random(8675309);
        RuleFor(fake => fake.Id, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.UserId, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.SessionId, fake => fake.Random.Number(1, 10));
    }
}