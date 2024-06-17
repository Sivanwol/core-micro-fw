using Application.Utils;
using Domain.Entities;
using Domain.Filters;
using Domain.Persistence.Context;
using Domain.Persistence.Repositories.Common;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.Enums;
using Microsoft.EntityFrameworkCore;
using Serilog;
namespace Domain.Persistence.Repositories;

public class ActivityLogRepository : BaseRepository, IActivityLogRepository {

    public ActivityLogRepository(IDomainContext context) : base(context) { }

    public Task<ActivityLog?> GetById(int id) {
        Log.Logger.Information($"Fetching activity log with id {id}");
        return Context.ActivityLogs.FirstOrDefaultAsync(w => w.Id == id);
    }

    public Task AddActivity(Guid? userId, string entityType, string operationType, string activity, string details, ActivityStatus status) {
        return AddActivity(userId, null, entityType, operationType, activity, details, status);
    }
    public Task AddActivity(Guid? userId, string entityType, string operationType, string activity, string details, ActivityStatus status, string? ipAddress, string? userAgent) {
        return AddActivity(userId, null, entityType, operationType, activity, details, status, ipAddress, userAgent);
    }
    public Task AddActivity(Guid? userId, string entityId, string entityType, string operationType, string activity, string details, ActivityStatus status) {
        return AddActivity(userId, null, entityId, entityType, operationType, activity, details, status);
    }
    public Task AddActivity(Guid? userId, Guid? ownerUserId, string entityId, string entityType, string operationType, string activity, string details, ActivityStatus status) {
        Log.Logger.Information($"Adding activity log with id {userId}-{entityId}-{entityType}-{operationType}");
        var activityLog = new ActivityLog {
            OwnerUserId = ownerUserId.HasValue ? ownerUserId.Value.ToString() : null,
            UserId = userId.HasValue ? userId.Value.ToString() : null,
            EntityId = entityId,
            CreatedAt = SystemClock.Now(),
            EntityType = entityType,
            OperationType = operationType,
            Activity = activity,
            Details = details,
            Status = status.ToString(),
        };
        Context.ActivityLogs.Add(activityLog);
        return Context.Instance.SaveChangesAsync();
    }

    public Task AddActivity(Guid? userId, string entityId, string entityType, string operationType, string activity, string details, ActivityStatus status, string? ipAddress,
        string? userAgent) {
        return AddActivity(userId, null, entityId, entityType, operationType, activity, details, status, ipAddress, userAgent);
    }
    public Task AddActivity(Guid? userId, Guid? ownerUserId, string entityId, string entityType, string operationType, string activity, string details, ActivityStatus status,
        string? ipAddress, string? userAgent) {
        Log.Logger.Information($"Adding activity log with id {userId}-{entityId}-{entityType}-{operationType}");
        var activityLog = new ActivityLog {
            OwnerUserId = ownerUserId.HasValue ? ownerUserId.Value.ToString() : null,
            UserId = userId.HasValue ? userId.Value.ToString() : null,
            EntityId = entityId,
            EntityType = entityType,
            OperationType = operationType,
            CreatedAt = SystemClock.Now(),
            Activity = activity,
            Details = details,
            Status = status.ToString(),
            IpAddress = ipAddress,
            UserAgent = userAgent
        };
        Context.ActivityLogs.Add(activityLog);
        return Context.Instance.SaveChangesAsync();
    }

    public async Task<ICollection<ActivityLog>> GetActivities(RecordFilterPagination<ActivityLogFilters> filter) {
        Log.Logger.Information($"Fetching activity log with filter {filter}");


        var query = Context.ActivityLogs.AsQueryable();

        return filter.ApplyQuery(query).ToList();
    }
    public async Task<int> GetActivitiesTotalPages(RecordFilterPagination<ActivityLogFilters> filter) {
        Log.Logger.Information($"Fetching activity log total pages with filter {filter}");
        var query = Context.ActivityLogs.AsQueryable();
        var entitiesCount = filter.ApplyQueryWithoutPagination(query).Count();
        return (int)Math.Ceiling((double)entitiesCount / filter.PageSize);
    }

    public async Task<int> GetActivitiesCount(RecordFilterPagination<ActivityLogFilters> filter) {
        Log.Logger.Information($"Fetching activity log count with filter {filter}");
        var query = Context.ActivityLogs.AsQueryable();
        return filter.ApplyQueryWithoutPagination(query).Count();
    }

    public Task DeleteOldActivities(PurgeActivityLogOlderThan priorToDelete) {
        Log.Logger.Information($"Deleting old activity log with filter {priorToDelete}");
        var query = Context.ActivityLogs.AsQueryable();
        switch (priorToDelete) {
            case PurgeActivityLogOlderThan.FromThreeMonths:
                query = query.Where(w => w.CreatedAt < DateTime.Now.AddMonths(-3));
                break;
            case PurgeActivityLogOlderThan.FromSixMonths:
                query = query.Where(w => w.CreatedAt < DateTime.Now.AddMonths(-6));
                break;
            case PurgeActivityLogOlderThan.FromOneYear:
                query = query.Where(w => w.CreatedAt < DateTime.Now.AddYears(-1));
                break;
            case PurgeActivityLogOlderThan.FromTwoYears:
                query = query.Where(w => w.CreatedAt < DateTime.Now.AddYears(-2));
                break;
            case PurgeActivityLogOlderThan.FromThreeYears:
                query = query.Where(w => w.CreatedAt < DateTime.Now.AddYears(-3));
                break;
            case PurgeActivityLogOlderThan.FromFiveYears:
                query = query.Where(w => w.CreatedAt < DateTime.Now.AddYears(-5));
                break;
        }
        return query.ForEachAsync(w => Context.ActivityLogs.Remove(w));
    }
}