using AutoBogus;
using Bogus;
using Domain.Entities;
using Infrastructure.Enums;
namespace Domain.Persistence.Mock.Configs;

public sealed class ActivityLogMockConfig : AutoFaker<ActivityLog> {
    public ActivityLogMockConfig() {
        Randomizer.Seed = new Random(8675309);
        RuleFor(fake => fake.Id, fake => fake.Random.Number(1, 1000));
        var opType = new Random().Next(1, 10);
        if (opType > 5) {
            RuleFor(fake => fake.UserId, fake => null);
            RuleFor(fake => fake.OperationType, fake => {
                var values = Enum.GetValues(typeof(ActivityOperationType))
                    .Cast<ActivityOperationType>()
                    .Where(val => val != ActivityOperationType.USER)
                    .Select(val => val.ToString());
                return fake.PickRandom(values);
            });
        } else {
            RuleFor(fake => fake.UserId, fake => fake.Random.Guid().ToString());
            RuleFor(fake => fake.OperationType, fake => nameof(ActivityOperationType.USER));
        }
        RuleFor(fake => fake.EntityId, fake => fake.Random.Guid().ToString());
        RuleFor(fake => fake.Activity, fake => fake.Random.String2(10));
        RuleFor(fake => fake.Details, fake => fake.Random.String2(10));
        RuleFor(fake => fake.Status, fake => {
            var randomStatus = fake.PickRandom<ActivityStatus>();
            return randomStatus.ToString();
        });
        RuleFor(fake => fake.IpAddress, fake => fake.Internet.Ip());
        RuleFor(fake => fake.UserAgent, fake => fake.Internet.UserAgent());
        RuleFor(fake => fake.EntityType, fake => fake.Random.String2(10));
        RuleFor(fake => fake.CreatedAt, fake => fake.Date.Past());
    }
}