using AutoBogus;
using Bogus;
using Domain.Entities;
namespace Domain.Persistence.Mock.Configs; 

public sealed class LanguagesMockConfig : AutoFaker<Languages>{
    public LanguagesMockConfig() {
        Randomizer.Seed = new Random(8675309);
        RuleFor(fake => fake.ID, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.Name, fake => $"Language {fake.Random.Number(1, 1000)}");
        RuleFor(fake => fake.Code, fake => $"{fake.Locale}-{fake.Random.Number(1, 1000)}");
        RuleFor(fake => fake.CreatedAt, fake => fake.Date.Past(1));
        RuleFor(fake => fake.UpdatedAt, fake => fake.Date.Recent());
    }
}