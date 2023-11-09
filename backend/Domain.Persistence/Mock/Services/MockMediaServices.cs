using Domain.Entities;
using Domain.Persistence.Interfaces.Mock;
using Domain.Persistence.Mock.Configs;
namespace Domain.Persistence.Mock.Services;

public class MockMediaServices : IMediaMockService {
    public MockMediaServices() {
        Faker = new MediaMockConfig();
    }
    private MediaMockConfig Faker { get; set; }
    public IEnumerable<Media> GetUserMedia(int userId) {
        Faker.UserId = userId;
        return Faker.Generate(Random.Shared.Next(1, 20)).ToList();
    }
    public Media GetOne(int userId) {
        Faker.UserId = userId;
        return Faker.Generate(1).First();
    }
}