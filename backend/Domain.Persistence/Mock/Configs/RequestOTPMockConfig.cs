using Application.Utils;
using AutoBogus;
using Bogus;
using Domain.DTO.OTP;
namespace Domain.Persistence.Mock.Configs;

public sealed class RequestOTPMockConfig : AutoFaker<RequestOTPResponseData> {
    public RequestOTPMockConfig() {
        Randomizer.Seed = new Random(8675309);
        RuleFor(fake => fake.UserToken, fake => StringExtensions.RandomUniqueString(128));
        RuleFor(fake => fake.OTPToken, fake => StringExtensions.RandomUniqueString(128));
        RuleFor(fake => fake.OTPExpired, fake => fake.Date.Soon(3));
    }
}