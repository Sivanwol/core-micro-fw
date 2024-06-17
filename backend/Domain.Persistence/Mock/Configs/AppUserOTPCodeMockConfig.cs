using Application.Utils;
using AutoBogus;
using Bogus;
using Domain.Entities;
using Infrastructure.Enums;
namespace Domain.Persistence.Mock.Configs;

public sealed class AppUserOTPCodeMockConfig : AutoFaker<ApplicationUserOtpCodes> {
    public AppUserOTPCodeMockConfig() {
        Randomizer.Seed = new Random(8675309);
        var dt = SystemClock.Now();
        RuleFor(fake => fake.Id, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.UserId, fake => fake.Random.Guid());
        RuleFor(fake => fake.ProviderType, fake => MFAProvider.Email);
        RuleFor(fake => fake.Token, fake => StringExtensions.RandomUniqueString(128));
        RuleFor(fake => fake.Code, fake => fake.Random.Number(111111, 999999).ToString());
        RuleFor(fake => fake.ComplateAt, fake => fake.Random.Number(1, 1000) % 2 == 0 ? null : dt);
        RuleFor(fake => fake.ExpirationDate, fake => dt.AddMinutes(3));
        RuleFor(fake => fake.CreatedAt, fake => dt);
        RuleFor(fake => fake.UpdatedAt, fake => dt);
    }
}