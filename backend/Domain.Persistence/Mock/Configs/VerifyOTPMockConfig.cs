using AutoBogus;
using Bogus;
using Domain.DTO.OTP;
namespace Domain.Persistence.Mock.Configs;

public sealed class VerifyOTPMockConfig : AutoFaker<VerifyOTPResponseData> {
    public VerifyOTPMockConfig() {
        Randomizer.Seed = new Random(8675309);
        var userFaker = new AppUserMockConfig();
        var user = userFaker.Generate(1).First();
        RuleFor(fake => fake.User, fake => user);
        RuleFor(fake => fake.IsValid, fake => true);
    }
}