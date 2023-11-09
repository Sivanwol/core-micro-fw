using AutoBogus;
using Bogus;
using Domain.Entities;
namespace Domain.Persistence.Mock.Configs;

public sealed class PartnerEthnicitiesMockConfig : AutoFaker<PartnerEthnicities> {
    public PartnerEthnicitiesMockConfig() {
        Randomizer.Seed = new Random(8675309);
        RuleFor(fake => fake.UserId, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.EthnicityId, fake => fake.Random.Number(1, 1000));
    }
}