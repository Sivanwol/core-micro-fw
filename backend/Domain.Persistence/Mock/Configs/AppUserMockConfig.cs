using AutoBogus;
using Bogus;
using Domain.Entities;
using Domain.Enums;
namespace Domain.Persistence.Mock.Configs; 

public sealed class AppUserMockConfig : AutoFaker<Users> {
    public AppUserMockConfig() {
        Randomizer.Seed = new Random(8675309);
        RuleFor(fake => fake.ID, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.Token, fake => fake.Random.String2(128));
        RuleFor(fake => fake.FirstName, fake => fake.Name.FirstName());
        RuleFor(fake => fake.LastName, fake => fake.Name.LastName());
        RuleFor(fake => fake.Email, fake => fake.Person.Email);
        RuleFor(fake => fake.Gender, fake => fake.PickRandom<Gender>());
        RuleFor(fake => fake.Latitude,fake => fake.Address.Latitude());
        RuleFor(fake => fake.Height , fake => fake.Random.Decimal(110, 210));
        RuleFor(fake => fake.MeasureUnits, fake => fake.PickRandom<MeasureUnit>());
        RuleFor(fake => fake.PartnerAgeFrom, fake => fake.Random.Number(18, 40));
        RuleFor(fake => fake.PartnerAgeTo, fake => fake.Random.Number(41, 99));
        RuleFor(fake => fake.PartnerHeightFrom, fake => fake.Random.Number(110, 150));
        RuleFor(fake => fake.PartnerHeightTo, fake => fake.Random.Number(151, 210));
        RuleFor(fake => fake.PhoneNumber, fake => fake.Person.Phone);
        RuleFor(fake => fake.CountryId, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.ReligionId, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.EthnicityId, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.LanguageId, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.BirthDate, fake => fake.Date.Past(Randomizer.Seed.Next(18, 40)));
        RuleFor(fake => fake.EmailVerified, fake => fake.Random.Bool());
        RuleFor(fake => fake.CreatedAt, fake => fake.Date.Past(1));
        RuleFor(fake => fake.UpdatedAt, fake => fake.Date.Recent());
    }
}