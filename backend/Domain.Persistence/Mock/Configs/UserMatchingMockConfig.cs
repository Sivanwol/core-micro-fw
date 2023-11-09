using AutoBogus;
using Bogus;
using Domain.Entities;
using Infrastructure.Enums;
namespace Domain.Persistence.Mock.Configs;

public sealed class UserMatchingMockConfig : AutoFaker<UserMatches> {
    public UserMatchingMockConfig() {
        Randomizer.Seed = new Random(8675309);
        RuleFor(fake => fake.Id, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.UserId, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.SessionId, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.Status, fake => fake.PickRandom<UserMatchingStatus>());
        RuleFor(fake => fake.Score, fake => fake.Random.Number(1, 10));
    }
}