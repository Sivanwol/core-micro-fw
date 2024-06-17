using AutoBogus;
using Bogus;
using Domain.Entities;
using Infrastructure.Enums;
namespace Domain.Persistence.Mock.Configs;

public sealed class CountriesMockConfig : AutoFaker<Countries> {
    public CountriesMockConfig() {
        Randomizer.Seed = new Random(8675309);
        RuleFor(fake => fake.Id, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.CountryName, fake => fake.Address.Country());
        RuleFor(fake => fake.CountryCode, fake => fake.Address.CountryCode());
        RuleFor(fake => fake.CountryNumber, fake => "972");
        RuleFor(fake => fake.Provider, fake => fake.PickRandom<SMSProviders>());
        RuleFor(fake => fake.CreatedAt, fake => fake.Date.Past());
        RuleFor(fake => fake.SupportedAt, fake => fake.Date.Recent(2));
        RuleFor(fake => fake.UpdatedAt, fake => fake.Date.Recent(3));
    }
}