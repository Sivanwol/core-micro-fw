using AutoBogus;
using Bogus;
using Domain.Entities;
namespace Domain.Persistence.Mock.Configs;

public sealed class MediaMockConfig : AutoFaker<Media> {

    public MediaMockConfig() {
        Randomizer.Seed = new Random(8675309);
        RuleFor(fake => fake.Id, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.UserId, fake => UserId);
        RuleFor(fake => fake.Path, fake => fake.System.DirectoryPath());
        RuleFor(fake => fake.FileName, fake => fake.System.FileName());
        RuleFor(fake => fake.FileUrl, fake => fake.Image.PicsumUrl());
        RuleFor(fake => fake.Height, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.Width, fake => fake.Random.Number(1, 1000));
        RuleFor(fake => fake.CreatedAt, fake => fake.Date.Past(1));
        RuleFor(fake => fake.UpdatedAt, fake => fake.Date.Recent());
    }
    public int UserId { get; set; }
}