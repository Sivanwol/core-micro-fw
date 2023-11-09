using AutoBogus;
using Bogus;
using Domain.Entities;
using Infrastructure.Enums;
namespace Domain.Persistence.Mock.Configs;

public sealed class UserMessagesMockConfig : AutoFaker<UserMessages> {
    public UserMessagesMockConfig() {
        Randomizer.Seed = new Random(8675309);

        RuleFor(fake => fake.Id, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.UserId, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.SessionId, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.QuestionId, fake => null);
        RuleFor(fake => fake.ReplyId, fake => fake.Random.Number(1, 1000) > 500 ? fake.Random.Number(1, 1000) : null);
        RuleFor(fake => fake.Message, fake => fake.Lorem.Sentence(10));
        RuleFor(fake => fake.Type, fake => fake.PickRandom<MessageType>());
        RuleFor(fake => fake.CreatedAt, fake => fake.Date.Past(1));
        RuleFor(fake => fake.UpdatedAt, fake => fake.Date.Recent());
    }
}