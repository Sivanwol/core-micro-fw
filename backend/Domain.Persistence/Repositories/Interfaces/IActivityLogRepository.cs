using Domain.Entities;
using Domain.Filters;
using Domain.Persistence.Repositories.Common.Interfaces;
using Infrastructure.Enums;
namespace Domain.Persistence.Repositories.Interfaces;

public enum PurgeActivityLogOlderThan {
    FromThreeMonths,
    FromSixMonths,
    FromOneYear,
    FromTwoYears,
    FromThreeYears,
    FromFiveYears
}

public interface IActivityLogRepository : IGenericRepository<ActivityLog, int> {
    Task AddActivity(Guid? userId, string entityType, string operationType, string activity, string details, ActivityStatus status);
    Task AddActivity(Guid? userId, string entityType, string operationType, string activity, string details, ActivityStatus status, string? ipAddress, string? userAgent);

    Task AddActivity(Guid? userId, string entityId, string entityType, string operationType, string activity, string details, ActivityStatus status);
    Task AddActivity(Guid? userId, Guid? ownerUserId, string entityId, string entityType, string operationType, string activity, string details, ActivityStatus status);
    Task AddActivity(Guid? userId, string entityId, string entityType, string operationType, string activity, string details, ActivityStatus status, string? ipAddress,
        string? userAgent);
    Task AddActivity(Guid? userId, Guid? ownerUserId, string entityId, string entityType, string operationType, string activity, string details, ActivityStatus status,
        string? ipAddress, string? userAgent);
    Task<ICollection<ActivityLog>> GetActivities(RecordFilterPagination<ActivityLogFilters> filter);
    Task<int> GetActivitiesTotalPages(RecordFilterPagination<ActivityLogFilters> filter);
    Task<int> GetActivitiesCount(RecordFilterPagination<ActivityLogFilters> filter);
    Task DeleteOldActivities(PurgeActivityLogOlderThan priorToDelete);
}