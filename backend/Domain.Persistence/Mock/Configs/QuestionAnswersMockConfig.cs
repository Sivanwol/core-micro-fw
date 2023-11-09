using AutoBogus;
using Bogus;
using Domain.Entities;
namespace Domain.Persistence.Mock.Configs;

public sealed class QuestionAnswersMockConfig : AutoFaker<QuestionAnswers> {
    public QuestionAnswersMockConfig() {
        Randomizer.Seed = new Random(8675309);
        RuleFor(fake => fake.Id, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.QuestionId, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.Text, fake => fake.Lorem.Sentence(10));
        RuleFor(fake => fake.CreatedAt, fake => fake.Date.Past(1));
        RuleFor(fake => fake.UpdatedAt, fake => fake.Date.Recent());
    }
}