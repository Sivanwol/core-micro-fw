using AutoBogus;
using Bogus;
using Domain.Entities;
namespace Domain.Persistence.Mock.Configs; 

public sealed class CountriesMockConfig : AutoFaker<Countries>{
    public CountriesMockConfig() {
        Randomizer.Seed = new Random(8675309);
        RuleFor(fake => fake.ID, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.CountryName, fake => fake.Address.Country());
        RuleFor(fake => fake.CountryCode, fake => fake.Address.CountryCode());
        RuleFor(fake => fake.CountryCode3, fake => fake.Address.CountryCode());
        RuleFor(fake => fake.CountryNumber, fake => "972");
        RuleFor(fake => fake.CreatedAt, fake => fake.Date.Past(1));
        RuleFor(fake => fake.UpdatedAt, fake => fake.Date.Recent());
    }
}