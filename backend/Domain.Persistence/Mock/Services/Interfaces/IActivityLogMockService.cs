using Domain.Entities;
namespace Domain.Persistence.Mock.Services.Interfaces;

public interface IActivityLogMockService {
    ActivityLog GetOne(Guid userId);
    ICollection<ActivityLog> GetMany(int totalEntries = 10);
    ICollection<ActivityLog> GetMany(Guid userId, int totalEntries = 10);
}