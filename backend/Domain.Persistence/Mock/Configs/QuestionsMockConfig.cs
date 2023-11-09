using AutoBogus;
using Bogus;
using Domain.Entities;
using Infrastructure.Enums;
namespace Domain.Persistence.Mock.Configs;

public sealed class QuestionsMockConfig : AutoFaker<Questions> {
    public QuestionsMockConfig() {
        Randomizer.Seed = new Random(8675309);
        RuleFor(fake => fake.Id, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.CategoryId, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.LanguageId, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.Text, fake => fake.Lorem.Sentence(10));
        RuleFor(fake => fake.Type, fake => fake.PickRandom<QuestionType>());
        RuleFor(fake => fake.Active, fake => fake.Random.Bool());
        RuleFor(fake => fake.Score, fake => fake.Random.Number(1, 10));
        RuleFor(fake => fake.CreatedAt, fake => fake.Date.Past(1));
        RuleFor(fake => fake.UpdatedAt, fake => fake.Date.Recent());
    }
}