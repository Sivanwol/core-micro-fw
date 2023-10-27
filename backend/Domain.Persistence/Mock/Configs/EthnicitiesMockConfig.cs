using AutoBogus;
using Bogus;
using Domain.Entities;
namespace Domain.Persistence.Mock.Configs; 

public sealed class EthnicitiesMockConfig : AutoFaker<Ethnicities>{
    public EthnicitiesMockConfig() {
        Randomizer.Seed = new Random(8675309);
        RuleFor(fake => fake.ID, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.Name, fake => $"Ethnicitie {fake.Random.Number(1, 1000)}");
        RuleFor(fake => fake.CreatedAt, fake => fake.Date.Past(1));
        RuleFor(fake => fake.UpdatedAt, fake => fake.Date.Recent());
    }
}