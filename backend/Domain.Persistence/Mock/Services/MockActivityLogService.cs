using Domain.Entities;
using Domain.Persistence.Mock.Configs;
using Domain.Persistence.Mock.Services.Interfaces;
namespace Domain.Persistence.Mock.Services;

public class MockActivityLogService : IActivityLogMockService {

    public MockActivityLogService() {
        Faker = new ActivityLogMockConfig();
    }
    private ActivityLogMockConfig Faker { get; }
    public ActivityLog GetOne(Guid userId) {
        var log = Faker.Generate(1).First();
        log.UserId = userId.ToString();
        log.OperationType = nameof(ActivityOperationType.USER);
        return log;
    }
    public ICollection<ActivityLog> GetMany(int totalEntries = 10) {
        var logs = Faker.Generate(totalEntries);
        foreach (var log in logs) {
            if (log.OperationType == nameof(ActivityOperationType.SYSTEM)) {
                log.UserId = null;
                log.IpAddress = null;
                log.UserAgent = null;
            } else {
                log.UserId = Guid.NewGuid().ToString();
                log.OperationType = nameof(ActivityOperationType.USER);
            }
        }
        return logs;
    }

    public ICollection<ActivityLog> GetMany(Guid userId, int totalEntries = 10) {
        var logs = Faker.Generate(totalEntries);
        foreach (var log in logs) {
            log.UserId = userId.ToString();
            log.OperationType = nameof(ActivityOperationType.USER);
        }
        return logs;
    }
}