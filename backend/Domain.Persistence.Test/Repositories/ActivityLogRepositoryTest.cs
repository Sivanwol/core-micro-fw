using Domain.Entities;
using Domain.Filters;
using Domain.Filters.Fields;
using Domain.Persistence.Mock.Services;
using Domain.Persistence.Repositories;
using Infrastructure.Enums;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable.Moq;
using NUnit.Framework.Legacy;
using Test.Shared.Common;
namespace Domain.Persistence.Test.Repositories;

[TestFixture] [Description("Test the ApplicationUserRepository")]
[Category("Activities")]
public class ActivityLogRepositoryTest : BaseTest {

    [SetUp]
    public void Setup() {
        SetupTest("CountriesRepositoryTest");
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

    }

    [Test, Description("Add a new activity log")]
    public async Task AddActivity() {
        var mock = new MockActivityLogService();
        var mockData = mock.GetMany(Guid.NewGuid());
        var repository = new ActivityLogRepository(Context.Object);
        var activityLog = mockData.First();

        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.ActivityLogs).Returns(mockObj.Object);
        Context.Setup(x => x.Instance.SaveChanges()).Returns(1);
        await repository.AddActivity(Guid.Parse(activityLog.UserId), activityLog.EntityId, activityLog.EntityType, activityLog.OperationType, activityLog.Activity,
            activityLog.Details, Enum.Parse<ActivityStatus>(activityLog.Status));
        var result = await repository.GetById(activityLog.Id);
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() => {
            Assert.That(result!.Id, Is.EqualTo(activityLog.Id));
            Assert.That(result!.UserId, Is.EqualTo(activityLog.UserId));
            Assert.That(result!.EntityId, Is.EqualTo(activityLog.EntityId));
            Assert.That(result!.EntityType, Is.EqualTo(activityLog.EntityType));
            Assert.That(result!.OperationType, Is.EqualTo(activityLog.OperationType));
            Assert.That(result!.Activity, Is.EqualTo(activityLog.Activity));
            Assert.That(result!.Details, Is.EqualTo(activityLog.Details));
            Assert.That(result!.Status, Is.EqualTo(activityLog.Status));
        });
    }

    [Test] [Description("Add a new activity log With IP and UserAgent")]
    public async Task AddActivityWithIpAndUserAgent() {
        var mock = new MockActivityLogService();
        var mockData = mock.GetMany(Guid.NewGuid());
        var repository = new ActivityLogRepository(Context.Object);
        var activityLog = mockData.First();

        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.ActivityLogs).Returns(mockObj.Object);
        Context.Setup(x => x.Instance.SaveChanges()).Returns(1);
        await repository.AddActivity(Guid.Parse(activityLog.UserId), activityLog.EntityId, activityLog.EntityType, activityLog.OperationType, activityLog.Activity,
            activityLog.Details, Enum.Parse<ActivityStatus>(activityLog.Status), activityLog.IpAddress, activityLog.UserAgent);
        var result = await repository.GetById(activityLog.Id);
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() => {
            Assert.That(result!.Id, Is.EqualTo(activityLog.Id));
            Assert.That(result!.UserId, Is.EqualTo(activityLog.UserId));
            Assert.That(result!.EntityId, Is.EqualTo(activityLog.EntityId));
            Assert.That(result!.EntityType, Is.EqualTo(activityLog.EntityType));
            Assert.That(result!.OperationType, Is.EqualTo(activityLog.OperationType));
            Assert.That(result!.Activity, Is.EqualTo(activityLog.Activity));
            Assert.That(result!.Details, Is.EqualTo(activityLog.Details));
            Assert.That(result!.Status, Is.EqualTo(activityLog.Status));
            Assert.That(result!.IpAddress, Is.EqualTo(activityLog.IpAddress));
            Assert.That(result!.UserAgent, Is.EqualTo(activityLog.UserAgent));
        });
    }


    [Test, Description("Get Total amount of pages")]
    [TestCase(10, 1)]
    [TestCase(20, 2)]
    [TestCase(30, 3)]
    public async Task GetActivitiesTotalPages(int totalEntries, int totalPages) {
        var mock = new MockActivityLogService();
        var mockData = mock.GetMany(totalEntries);
        var repository = new ActivityLogRepository(Context.Object);
        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.ActivityLogs).Returns(mockObj.Object);
        var result = await repository.GetActivitiesTotalPages(new RecordFilterPagination<ActivityLogFilters>());
        Assert.That(result, Is.EqualTo(totalPages));
    }

    [Test] [Description("Get Total amount of entries")]
    [TestCase(10)]
    [TestCase(20)]
    [TestCase(30)]
    public async Task GetActivitiesTotalEntities(int totalEntries) {
        var mock = new MockActivityLogService();
        var mockData = mock.GetMany(totalEntries);
        var repository = new ActivityLogRepository(Context.Object);
        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.ActivityLogs).Returns(mockObj.Object);
        var result = await repository.GetActivitiesCount(new RecordFilterPagination<ActivityLogFilters>());
        Assert.That(result, Is.EqualTo(totalEntries));
    }

    [Test, Description("Get Activities without filters and pagination")]
    public async Task GetActivitiesWithoutFilltersAndPagination() {
        var mock = new MockActivityLogService();
        var mockData = mock.GetMany(); // Use a fixed number of entries for testing
        var repository = new ActivityLogRepository(Context.Object);

        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.ActivityLogs).Returns(mockObj.Object);

        var result = await repository.GetActivities(new RecordFilterPagination<ActivityLogFilters>());

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(mockData.Count)); // Should return all entries

        foreach (var item in result) {
            Assert.Multiple(() => {
                Assert.That(mockData.Any(x => x.Id == item.Id), Is.True);
                Assert.That(mockData.Any(x => x.UserId == item.UserId), Is.True);
                Assert.That(mockData.Any(x => x.EntityId == item.EntityId), Is.True);
                Assert.That(mockData.Any(x => x.EntityType == item.EntityType), Is.True);
                Assert.That(mockData.Any(x => x.OperationType == item.OperationType), Is.True);
                Assert.That(mockData.Any(x => x.Activity == item.Activity), Is.True);
                Assert.That(mockData.Any(x => x.Details == item.Details), Is.True);
                Assert.That(mockData.Any(x => x.Status == item.Status), Is.True);
                Assert.That(mockData.Any(x => x.IpAddress == item.IpAddress), Is.True);
                Assert.That(mockData.Any(x => x.UserAgent == item.UserAgent), Is.True);
            });
        }
    }

    [Test, Description("Get Activities without filters and with pagination")]
    [TestCase(10, 10)]
    [TestCase(20, 20)]
    [TestCase(30, 30)]
    [TestCase(30, 10)]
    public async Task GetActivitiesWithoutFilltersAndWithPagination(int totalEntries, int pageSize) {

        var mock = new MockActivityLogService();
        var mockData = mock.GetMany(totalEntries); // Use a fixed number of entries for testing
        var repository = new ActivityLogRepository(Context.Object);

        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.ActivityLogs).Returns(mockObj.Object);

        var result = await repository.GetActivities(new RecordFilterPagination<ActivityLogFilters> {
            PageSize = pageSize
        });

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(pageSize)); // Should return all entries

        foreach (var item in result) {
            Assert.Multiple(() => {
                Assert.That(mockData.Any(x => x.Id == item.Id), Is.True);
                Assert.That(mockData.Any(x => x.UserId == item.UserId), Is.True);
                Assert.That(mockData.Any(x => x.EntityId == item.EntityId), Is.True);
                Assert.That(mockData.Any(x => x.EntityType == item.EntityType), Is.True);
                Assert.That(mockData.Any(x => x.OperationType == item.OperationType), Is.True);
                Assert.That(mockData.Any(x => x.Activity == item.Activity), Is.True);
                Assert.That(mockData.Any(x => x.Details == item.Details), Is.True);
                Assert.That(mockData.Any(x => x.Status == item.Status), Is.True);
                Assert.That(mockData.Any(x => x.IpAddress == item.IpAddress), Is.True);
                Assert.That(mockData.Any(x => x.UserAgent == item.UserAgent), Is.True);
            });
        }
    }
    [Test, Description("Get Activities without filters and with pagination Navigation")]
    [TestCase(30, 1)]
    [TestCase(30, 2)]
    [TestCase(30, 3)]
    public async Task GetActivitiesWithoutFilltersAndWithPaginationNavigation(int totalEntities, int page) {

        var mock = new MockActivityLogService();
        var mockData = mock.GetMany(totalEntities); // Use a fixed number of entries for testing
        var repository = new ActivityLogRepository(Context.Object);

        var pageSize = 10;
        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.ActivityLogs).Returns(mockObj.Object);

        var result = await repository.GetActivities(new RecordFilterPagination<ActivityLogFilters> {
            Page = page,
            PageSize = pageSize
        });

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(pageSize)); // Should return all entries

        foreach (var item in result) {
            Assert.Multiple(() => {
                Assert.That(mockData.Any(x => x.Id == item.Id), Is.True);
                Assert.That(mockData.Any(x => x.UserId == item.UserId), Is.True);
                Assert.That(mockData.Any(x => x.EntityId == item.EntityId), Is.True);
                Assert.That(mockData.Any(x => x.EntityType == item.EntityType), Is.True);
                Assert.That(mockData.Any(x => x.OperationType == item.OperationType), Is.True);
                Assert.That(mockData.Any(x => x.Activity == item.Activity), Is.True);
                Assert.That(mockData.Any(x => x.Details == item.Details), Is.True);
                Assert.That(mockData.Any(x => x.Status == item.Status), Is.True);
                Assert.That(mockData.Any(x => x.IpAddress == item.IpAddress), Is.True);
                Assert.That(mockData.Any(x => x.UserAgent == item.UserAgent), Is.True);
            });
        }
    }
    [Test, Description("Get Activities without filters and pagination With Sort")]
    [TestCase(30, "Id", SortDirection.Ascending)]
    [TestCase(30, "Id", SortDirection.Descending)]
    [TestCase(30, "UserId", SortDirection.Ascending)]
    [TestCase(30, "UserId", SortDirection.Descending)]
    [TestCase(30, "EntityId", SortDirection.Ascending)]
    [TestCase(30, "EntityId", SortDirection.Descending)]
    [TestCase(30, "EntityType", SortDirection.Ascending)]
    [TestCase(30, "EntityType", SortDirection.Descending)]
    [TestCase(30, "Activity", SortDirection.Ascending)]
    [TestCase(30, "Activity", SortDirection.Descending)]
    [TestCase(30, "Status", SortDirection.Ascending)]
    [TestCase(30, "Status", SortDirection.Descending)]
    [TestCase(30, "IpAddress", SortDirection.Ascending)]
    [TestCase(30, "IpAddress", SortDirection.Descending)]
    [TestCase(30, "UserAgent", SortDirection.Ascending)]
    [TestCase(30, "UserAgent", SortDirection.Descending)]
    [TestCase(30, "CreatedAt", SortDirection.Ascending)]
    [TestCase(30, "CreatedAt", SortDirection.Descending)]
    public async Task GetActivitiesWithoutFilltersAndPaginationWithSortBy(int tottalEntities, string sortByField, SortDirection sortDirection) {
        var mock = new MockActivityLogService();
        var mockData = mock.GetMany(tottalEntities); // Use a fixed number of entries for testing
        var repository = new ActivityLogRepository(Context.Object);

        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.ActivityLogs).Returns(mockObj.Object);

        var result = await repository.GetActivities(new RecordFilterPagination<ActivityLogFilters> {
            SortByField = sortByField,
            SortDirection = sortDirection
        });
        if (string.IsNullOrEmpty(sortByField))
            Assert.Fail("SortByField is null or empty");
        //apply sorting
        if (!string.IsNullOrEmpty(sortByField)) {
            switch (sortByField) {
                case "Id":
                    mockData = (sortDirection == SortDirection.Ascending
                        ? mockData.OrderBy(a => a.Id)
                        : mockData.OrderByDescending(a => a.Id)).ToList();
                    break;
                case "UserId":
                    mockData = (sortDirection == SortDirection.Ascending
                        ? mockData.OrderBy(a => a.UserId)
                        : mockData.OrderByDescending(a => a.UserId)).ToList();
                    break;
                case "EntityId":
                    mockData = (sortDirection == SortDirection.Ascending
                        ? mockData.OrderBy(a => a.EntityId)
                        : mockData.OrderByDescending(a => a.EntityId)).ToList();
                    break;
                case "EntityType":
                    mockData = (sortDirection == SortDirection.Ascending
                        ? mockData.OrderBy(a => a.EntityType)
                        : mockData.OrderByDescending(a => a.EntityType)).ToList();
                    break;
                case "Activity":
                    mockData = (sortDirection == SortDirection.Ascending
                        ? mockData.OrderBy(a => a.Activity)
                        : mockData.OrderByDescending(a => a.Activity)).ToList();
                    break;
                case "Status":
                    mockData = (sortDirection == SortDirection.Ascending
                        ? mockData.OrderBy(a => a.Status)
                        : mockData.OrderByDescending(a => a.Status)).ToList();
                    break;
                case "IpAddress":
                    mockData = (sortDirection == SortDirection.Ascending
                        ? mockData.OrderBy(a => a.IpAddress)
                        : mockData.OrderByDescending(a => a.IpAddress)).ToList();
                    break;
                case "UserAgent":
                    mockData = (sortDirection == SortDirection.Ascending
                        ? mockData.OrderBy(a => a.UserAgent)
                        : mockData.OrderByDescending(a => a.UserAgent)).ToList();
                    break;
                case "CreatedAt":
                    mockData = (sortDirection == SortDirection.Ascending
                        ? mockData.OrderBy(a => a.CreatedAt)
                        : mockData.OrderByDescending(a => a.CreatedAt)).ToList();
                    break;
            }
        }

        mockData = mockData.Skip(0).Take(10).ToList();
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(10)); // Should return all entries

        foreach (var item in result) {
            Assert.Multiple(() => {
                Assert.That(mockData.Any(x => x.Id == item.Id), Is.True);
                Assert.That(mockData.Any(x => x.UserId == item.UserId), Is.True);
                Assert.That(mockData.Any(x => x.EntityId == item.EntityId), Is.True);
                Assert.That(mockData.Any(x => x.EntityType == item.EntityType), Is.True);
                Assert.That(mockData.Any(x => x.OperationType == item.OperationType), Is.True);
                Assert.That(mockData.Any(x => x.Activity == item.Activity), Is.True);
                Assert.That(mockData.Any(x => x.Details == item.Details), Is.True);
                Assert.That(mockData.Any(x => x.Status == item.Status), Is.True);
                Assert.That(mockData.Any(x => x.IpAddress == item.IpAddress), Is.True);
                Assert.That(mockData.Any(x => x.UserAgent == item.UserAgent), Is.True);
            });
        }
    }

    [Test, Description("Get Activities with string filter and disabled string filter")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetActivitiesWithStringFilter(bool DisableFilter) {
        var mock = new MockActivityLogService();
        var mockData = mock.GetMany(); // Use a fixed number of entries for testing
        var repository = new ActivityLogRepository(Context.Object);

        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.ActivityLogs).Returns(mockObj.Object);
        var fillters = new RecordFilterPagination<ActivityLogFilters>();
        fillters.Filters.ActivityFilters.Add(new StringFilterField("Activity", FilterStringOperation.Equal) {
            FilterValue = mockEnumerable.First().Activity
        });
        fillters.Filters.ActivityFilters.Add(new StringFilterField("Activity", FilterStringOperation.Equal, DisableFilter) {
            FilterValue = mockEnumerable.Last().Activity
        });
        var result = await repository.GetActivities(fillters);
        var expected = new List<ActivityLog> {
            mockEnumerable.First()
        };
        if (!DisableFilter) {
            expected.Add(mockEnumerable.Last());
        }
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(expected.Count)); // Should return all entries

        foreach (var item in result) {
            Assert.Multiple(() => {
                Assert.That(expected.Any(x => x.Id == item.Id), Is.True);
                Assert.That(expected.Any(x => x.UserId == item.UserId), Is.True);
                Assert.That(expected.Any(x => x.EntityId == item.EntityId), Is.True);
                Assert.That(expected.Any(x => x.EntityType == item.EntityType), Is.True);
                Assert.That(expected.Any(x => x.OperationType == item.OperationType), Is.True);
                Assert.That(expected.Any(x => x.Activity == item.Activity), Is.True);
                Assert.That(expected.Any(x => x.Details == item.Details), Is.True);
                Assert.That(expected.Any(x => x.Status == item.Status), Is.True);
                Assert.That(expected.Any(x => x.IpAddress == item.IpAddress), Is.True);
                Assert.That(expected.Any(x => x.UserAgent == item.UserAgent), Is.True);
            });
        }
    }
    [Test, Description("Get Activities with string filter That has two filters with multi filters")]
    [TestCase("Test", "AABBCCDD", 2)]
    [TestCase("KOKKK", "HHGGFFDD", 5)]
    public async Task GetActivitiesWithStringFilterWithMultiFilters(string filterValue1, string filterValue2, int idx) {
        var mock = new MockActivityLogService();
        var mockData = mock.GetMany(Guid.NewGuid()); // Use a fixed number of entries for testing
        mockData.Last().Activity = $"{filterValue1}";
        mockData.First().Activity = $"{filterValue2}";
        var repository = new ActivityLogRepository(Context.Object);

        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.ActivityLogs).Returns(mockObj.Object);
        var fillters = new RecordFilterPagination<ActivityLogFilters>();
        fillters.Filters.ActivityFilters.Add(new StringFilterField("Activity", FilterStringOperation.Equal) {
            FilterValue = filterValue1
        });
        fillters.Filters.ActivityFilters.Add(new StringFilterField("Activity", FilterStringOperation.Equal) {
            FilterValue = filterValue2
        });
        fillters.Filters.IpAddressFilters.Add(new StringFilterField("IpAddress", FilterStringOperation.Equal) {
            FilterValue = mockEnumerable[idx].IpAddress
        });
        var result = await repository.GetActivities(fillters);
        var expected = new List<ActivityLog> {
            mockEnumerable.First(),
            mockEnumerable[idx],
            mockEnumerable.Last()
        };
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(expected.Count)); // Should return all entries

        foreach (var item in result) {
            Assert.Multiple(() => {
                Assert.That(expected.Any(x => x.Id == item.Id), Is.True);
                Assert.That(expected.Any(x => x.UserId == item.UserId), Is.True);
                Assert.That(expected.Any(x => x.EntityId == item.EntityId), Is.True);
                Assert.That(expected.Any(x => x.EntityType == item.EntityType), Is.True);
                Assert.That(expected.Any(x => x.OperationType == item.OperationType), Is.True);
                Assert.That(expected.Any(x => x.Activity == item.Activity), Is.True);
                Assert.That(expected.Any(x => x.Details == item.Details), Is.True);
                Assert.That(expected.Any(x => x.Status == item.Status), Is.True);
                Assert.That(expected.Any(x => x.IpAddress == item.IpAddress), Is.True);
                Assert.That(expected.Any(x => x.UserAgent == item.UserAgent), Is.True);
            });
        }
    }

    [Test, Description("Get Activities with string filter and Equal operation")]
    public async Task GetActivitiesWithStringFilterOperatorEqual() {
        var mock = new MockActivityLogService();
        var mockData = mock.GetMany(Guid.NewGuid());
        var repository = new ActivityLogRepository(Context.Object);

        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.ActivityLogs).Returns(mockObj.Object);
        var fillters = new RecordFilterPagination<ActivityLogFilters>();
        fillters.Filters.ActivityFilters.Add(new StringFilterField("EntityType", FilterStringOperation.Equal) {
            FilterValue = mockData.First().EntityType
        });
        var result = await repository.GetActivities(fillters);
        var expected = new List<ActivityLog> {
            mockEnumerable.First()
        };
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(expected.Count)); // Should return all entries

        foreach (var item in result) {
            Assert.Multiple(() => {
                Assert.That(expected.Any(x => x.Id == item.Id), Is.True);
                Assert.That(expected.Any(x => x.UserId == item.UserId), Is.True);
                Assert.That(expected.Any(x => x.EntityId == item.EntityId), Is.True);
                Assert.That(expected.Any(x => x.EntityType == item.EntityType), Is.True);
                Assert.That(expected.Any(x => x.OperationType == item.OperationType), Is.True);
                Assert.That(expected.Any(x => x.Activity == item.Activity), Is.True);
                Assert.That(expected.Any(x => x.Details == item.Details), Is.True);
                Assert.That(expected.Any(x => x.Status == item.Status), Is.True);
                Assert.That(expected.Any(x => x.IpAddress == item.IpAddress), Is.True);
                Assert.That(expected.Any(x => x.UserAgent == item.UserAgent), Is.True);
            });
        }
    }

    [Test, Description("Get Activities with string filter and Not Equal operation")]
    public async Task GetActivitiesWithStringFilterOperatorNotEqual() {
        var mock = new MockActivityLogService();
        var mockData = mock.GetMany(Guid.NewGuid());
        var repository = new ActivityLogRepository(Context.Object);

        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.ActivityLogs).Returns(mockObj.Object);
        var fillters = new RecordFilterPagination<ActivityLogFilters>();
        fillters.Filters.ActivityFilters.Add(new StringFilterField("EntityType", FilterStringOperation.NotEqual) {
            FilterValue = mockData.First().EntityType
        });
        var result = await repository.GetActivities(fillters);
        var expected = mockData.ToList();
        expected.RemoveAt(0);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(expected.Count)); // Should return all entries

        foreach (var item in result) {
            Assert.Multiple(() => {
                Assert.That(expected.Any(x => x.Id == item.Id), Is.True);
                Assert.That(expected.Any(x => x.UserId == item.UserId), Is.True);
                Assert.That(expected.Any(x => x.EntityId == item.EntityId), Is.True);
                Assert.That(expected.Any(x => x.EntityType == item.EntityType), Is.True);
                Assert.That(expected.Any(x => x.OperationType == item.OperationType), Is.True);
                Assert.That(expected.Any(x => x.Activity == item.Activity), Is.True);
                Assert.That(expected.Any(x => x.Details == item.Details), Is.True);
                Assert.That(expected.Any(x => x.Status == item.Status), Is.True);
                Assert.That(expected.Any(x => x.IpAddress == item.IpAddress), Is.True);
                Assert.That(expected.Any(x => x.UserAgent == item.UserAgent), Is.True);
            });
        }
    }

    [Test, Description("Get Activities with string filter and Contains operation")]
    public async Task GetActivitiesWithStringFilterOperatorContains() {
        var mock = new MockActivityLogService();
        var mockData = mock.GetMany(Guid.NewGuid());
        var repository = new ActivityLogRepository(Context.Object);
        mockData.First().EntityType = $"{mockData.First().EntityType} Test";
        mockData.Last().EntityType = $"{mockData.Last().EntityType} Test";
        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.ActivityLogs).Returns(mockObj.Object);
        var fillters = new RecordFilterPagination<ActivityLogFilters>();
        fillters.Filters.ActivityFilters.Add(new StringFilterField("EntityType", FilterStringOperation.Contains) {
            FilterValue = "Test"
        });
        var result = await repository.GetActivities(fillters);
        var expected = new List<ActivityLog> {
            mockEnumerable.First(),
            mockEnumerable.Last()
        };
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(expected.Count)); // Should return all entries

        foreach (var item in result) {
            Assert.Multiple(() => {
                Assert.That(expected.Any(x => x.Id == item.Id), Is.True);
                Assert.That(expected.Any(x => x.UserId == item.UserId), Is.True);
                Assert.That(expected.Any(x => x.EntityId == item.EntityId), Is.True);
                Assert.That(expected.Any(x => x.EntityType == item.EntityType), Is.True);
                Assert.That(expected.Any(x => x.OperationType == item.OperationType), Is.True);
                Assert.That(expected.Any(x => x.Activity == item.Activity), Is.True);
                Assert.That(expected.Any(x => x.Details == item.Details), Is.True);
                Assert.That(expected.Any(x => x.Status == item.Status), Is.True);
                Assert.That(expected.Any(x => x.IpAddress == item.IpAddress), Is.True);
                Assert.That(expected.Any(x => x.UserAgent == item.UserAgent), Is.True);
            });
        }
    }

    [Test, Description("Get Activities with string filter and Not Contains operation")]
    public async Task GetActivitiesWithStringFilterOperatorNotContains() {
        var mock = new MockActivityLogService();
        var mockData = mock.GetMany(Guid.NewGuid());
        var repository = new ActivityLogRepository(Context.Object);
        mockData.First().EntityType = $"{mockData.First().EntityType} Test";
        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.ActivityLogs).Returns(mockObj.Object);
        var fillters = new RecordFilterPagination<ActivityLogFilters>();
        fillters.Filters.ActivityFilters.Add(new StringFilterField("EntityType", FilterStringOperation.NotContains) {
            FilterValue = "Test"
        });
        var result = await repository.GetActivities(fillters);
        var expected = mockData.ToList();
        expected.RemoveAt(0);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(expected.Count)); // Should return all entries

        foreach (var item in result) {
            Assert.Multiple(() => {
                Assert.That(expected.Any(x => x.Id == item.Id), Is.True);
                Assert.That(expected.Any(x => x.UserId == item.UserId), Is.True);
                Assert.That(expected.Any(x => x.EntityId == item.EntityId), Is.True);
                Assert.That(expected.Any(x => x.EntityType == item.EntityType), Is.True);
                Assert.That(expected.Any(x => x.OperationType == item.OperationType), Is.True);
                Assert.That(expected.Any(x => x.Activity == item.Activity), Is.True);
                Assert.That(expected.Any(x => x.Details == item.Details), Is.True);
                Assert.That(expected.Any(x => x.Status == item.Status), Is.True);
                Assert.That(expected.Any(x => x.IpAddress == item.IpAddress), Is.True);
                Assert.That(expected.Any(x => x.UserAgent == item.UserAgent), Is.True);
            });
        }
    }

    [Test, Description("Get Activities with string filter and Start With operation")]
    public async Task GetActivitiesWithStringFilterOperatorStartWith() {
        var mock = new MockActivityLogService();
        var mockData = mock.GetMany(Guid.NewGuid());
        var repository = new ActivityLogRepository(Context.Object);
        mockData.First().EntityType = $"Test {mockData.First().EntityType}";
        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.ActivityLogs).Returns(mockObj.Object);
        var fillters = new RecordFilterPagination<ActivityLogFilters>();
        fillters.Filters.ActivityFilters.Add(new StringFilterField("EntityType", FilterStringOperation.StartsWith) {
            FilterValue = "Test"
        });
        var result = await repository.GetActivities(fillters);
        var expected = new List<ActivityLog> {
            mockEnumerable.First()
        };
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(expected.Count)); // Should return all entries

        foreach (var item in result) {
            Assert.Multiple(() => {
                Assert.That(expected.Any(x => x.Id == item.Id), Is.True);
                Assert.That(expected.Any(x => x.UserId == item.UserId), Is.True);
                Assert.That(expected.Any(x => x.EntityId == item.EntityId), Is.True);
                Assert.That(expected.Any(x => x.EntityType == item.EntityType), Is.True);
                Assert.That(expected.Any(x => x.OperationType == item.OperationType), Is.True);
                Assert.That(expected.Any(x => x.Activity == item.Activity), Is.True);
                Assert.That(expected.Any(x => x.Details == item.Details), Is.True);
                Assert.That(expected.Any(x => x.Status == item.Status), Is.True);
                Assert.That(expected.Any(x => x.IpAddress == item.IpAddress), Is.True);
                Assert.That(expected.Any(x => x.UserAgent == item.UserAgent), Is.True);
            });
        }
    }

    [Test, Description("Get Activities with string filter and Not Start With operation")]
    public async Task GetActivitiesWithStringFilterOperatorNotStartWith() {
        var mock = new MockActivityLogService();
        var mockData = mock.GetMany(Guid.NewGuid());
        var repository = new ActivityLogRepository(Context.Object);
        mockData.First().EntityType = $"Test {mockData.First().EntityType}";
        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.ActivityLogs).Returns(mockObj.Object);
        var fillters = new RecordFilterPagination<ActivityLogFilters>();
        fillters.Filters.ActivityFilters.Add(new StringFilterField("EntityType", FilterStringOperation.NotStartsWith) {
            FilterValue = "Test"
        });
        var result = await repository.GetActivities(fillters);
        var expected = mockData.ToList();
        expected.RemoveAt(0);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(expected.Count)); // Should return all entries

        foreach (var item in result) {
            Assert.Multiple(() => {
                Assert.That(expected.Any(x => x.Id == item.Id), Is.True);
                Assert.That(expected.Any(x => x.UserId == item.UserId), Is.True);
                Assert.That(expected.Any(x => x.EntityId == item.EntityId), Is.True);
                Assert.That(expected.Any(x => x.EntityType == item.EntityType), Is.True);
                Assert.That(expected.Any(x => x.OperationType == item.OperationType), Is.True);
                Assert.That(expected.Any(x => x.Activity == item.Activity), Is.True);
                Assert.That(expected.Any(x => x.Details == item.Details), Is.True);
                Assert.That(expected.Any(x => x.Status == item.Status), Is.True);
                Assert.That(expected.Any(x => x.IpAddress == item.IpAddress), Is.True);
                Assert.That(expected.Any(x => x.UserAgent == item.UserAgent), Is.True);
            });
        }
    }

    [Test, Description("Get Activities with string filter and End With operation")]
    public async Task GetActivitiesWithStringFilterOperatorndEndWith() {
        var mock = new MockActivityLogService();
        var mockData = mock.GetMany(Guid.NewGuid());
        var repository = new ActivityLogRepository(Context.Object);
        mockData.First().EntityType = $"{mockData.First().EntityType} Test";
        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.ActivityLogs).Returns(mockObj.Object);
        var fillters = new RecordFilterPagination<ActivityLogFilters>();
        fillters.Filters.ActivityFilters.Add(new StringFilterField("EntityType", FilterStringOperation.EndsWith) {
            FilterValue = "Test"
        });
        var result = await repository.GetActivities(fillters);
        var expected = new List<ActivityLog> {
            mockEnumerable.First()
        };
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(expected.Count)); // Should return all entries

        foreach (var item in result) {
            Assert.Multiple(() => {
                Assert.That(expected.Any(x => x.Id == item.Id), Is.True);
                Assert.That(expected.Any(x => x.UserId == item.UserId), Is.True);
                Assert.That(expected.Any(x => x.EntityId == item.EntityId), Is.True);
                Assert.That(expected.Any(x => x.EntityType == item.EntityType), Is.True);
                Assert.That(expected.Any(x => x.OperationType == item.OperationType), Is.True);
                Assert.That(expected.Any(x => x.Activity == item.Activity), Is.True);
                Assert.That(expected.Any(x => x.Details == item.Details), Is.True);
                Assert.That(expected.Any(x => x.Status == item.Status), Is.True);
                Assert.That(expected.Any(x => x.IpAddress == item.IpAddress), Is.True);
                Assert.That(expected.Any(x => x.UserAgent == item.UserAgent), Is.True);
            });
        }
    }

    [Test, Description("Get Activities with string filter and Not End with operation")]
    public async Task GetActivitiesWithStringFilterOperatorNotEndWith() {
        var mock = new MockActivityLogService();
        var mockData = mock.GetMany(Guid.NewGuid());
        var repository = new ActivityLogRepository(Context.Object);
        mockData.First().EntityType = $"{mockData.First().EntityType} Test";
        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.ActivityLogs).Returns(mockObj.Object);
        var fillters = new RecordFilterPagination<ActivityLogFilters>();
        fillters.Filters.ActivityFilters.Add(new StringFilterField("EntityType", FilterStringOperation.NotEndsWith) {
            FilterValue = "Test"
        });
        var result = await repository.GetActivities(fillters);
        var expected = mockData.ToList();
        expected.RemoveAt(0);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(expected.Count)); // Should return all entries

        foreach (var item in result) {
            Assert.Multiple(() => {
                Assert.That(expected.Any(x => x.Id == item.Id), Is.True);
                Assert.That(expected.Any(x => x.UserId == item.UserId), Is.True);
                Assert.That(expected.Any(x => x.EntityId == item.EntityId), Is.True);
                Assert.That(expected.Any(x => x.EntityType == item.EntityType), Is.True);
                Assert.That(expected.Any(x => x.OperationType == item.OperationType), Is.True);
                Assert.That(expected.Any(x => x.Activity == item.Activity), Is.True);
                Assert.That(expected.Any(x => x.Details == item.Details), Is.True);
                Assert.That(expected.Any(x => x.Status == item.Status), Is.True);
                Assert.That(expected.Any(x => x.IpAddress == item.IpAddress), Is.True);
                Assert.That(expected.Any(x => x.UserAgent == item.UserAgent), Is.True);
            });
        }
    }

    [Test, Description("Get Activities with string filter and In operation With No Values")]
    public async Task GetActivitiesWithStringFilterOperatorInWithNoValues() {
        var mock = new MockActivityLogService();
        var mockData = mock.GetMany(Guid.NewGuid());
        var repository = new ActivityLogRepository(Context.Object);
        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.ActivityLogs).Returns(mockObj.Object);
        var fillters = new RecordFilterPagination<ActivityLogFilters>();
        fillters.Filters.ActivityFilters.Add(new StringFilterField("EntityType", FilterStringOperation.In) {
            FilterValues = new List<string>()
        });
        try {
            await repository.GetActivities(fillters);
            Assert.Fail("Expected exception was not thrown.");
        }
        catch (InvalidOperationException ex) {
            // Assert
            StringAssert.Contains("Filters values are not provided.", ex.Message);
        }
        fillters.Filters.ActivityFilters.Clear();
        fillters.Filters.ActivityFilters.Add(new StringFilterField("EntityType", FilterStringOperation.In));
        try {
            await repository.GetActivities(fillters);
            Assert.Fail("Expected exception was not thrown.");
        }
        catch (InvalidOperationException ex) {
            // Assert
            StringAssert.Contains("Filters values are not provided.", ex.Message);
        }

    }

    [Test, Description("Get Activities with string filter and In operation")]
    public async Task GetActivitiesWithStringFilterOperatorIn() {
        var mock = new MockActivityLogService();
        var mockData = mock.GetMany(Guid.NewGuid());
        var repository = new ActivityLogRepository(Context.Object);
        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.ActivityLogs).Returns(mockObj.Object);
        var fillters = new RecordFilterPagination<ActivityLogFilters>();
        fillters.Filters.ActivityFilters.Add(new StringFilterField("EntityType", FilterStringOperation.In) {
            FilterValues = new List<string> {
                mockData.First().EntityType,
                mockData.Last().EntityType
            }
        });
        var result = await repository.GetActivities(fillters);
        var expected = new List<ActivityLog> {
            mockEnumerable.First(),
            mockEnumerable.Last()
        };
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(expected.Count)); // Should return all entries

        foreach (var item in result) {
            Assert.Multiple(() => {
                Assert.That(expected.Any(x => x.Id == item.Id), Is.True);
                Assert.That(expected.Any(x => x.UserId == item.UserId), Is.True);
                Assert.That(expected.Any(x => x.EntityId == item.EntityId), Is.True);
                Assert.That(expected.Any(x => x.EntityType == item.EntityType), Is.True);
                Assert.That(expected.Any(x => x.OperationType == item.OperationType), Is.True);
                Assert.That(expected.Any(x => x.Activity == item.Activity), Is.True);
                Assert.That(expected.Any(x => x.Details == item.Details), Is.True);
                Assert.That(expected.Any(x => x.Status == item.Status), Is.True);
                Assert.That(expected.Any(x => x.IpAddress == item.IpAddress), Is.True);
                Assert.That(expected.Any(x => x.UserAgent == item.UserAgent), Is.True);
            });
        }
    }
    [Test, Description("Get Activities with string filter and Not In operation")]
    public async Task GetActivitiesWithStringFilterOperatorNotIn() {
        var mock = new MockActivityLogService();
        var mockData = mock.GetMany(Guid.NewGuid());
        var repository = new ActivityLogRepository(Context.Object);
        var mockEnumerable = mockData.ToList();
        var mockObj = mockEnumerable.AsQueryable().BuildMockDbSet();
        Context.Setup(x => x.ActivityLogs).Returns(mockObj.Object);
        var fillters = new RecordFilterPagination<ActivityLogFilters>();
        fillters.Filters.ActivityFilters.Add(new StringFilterField("EntityType", FilterStringOperation.NotIn) {
            FilterValues = new List<string> {
                mockData.First().EntityType
            }
        });
        var result = await repository.GetActivities(fillters);
        var expected = mockEnumerable.ToList();
        expected.RemoveAt(0);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(expected.Count)); // Should return all entries

        foreach (var item in result) {
            Assert.Multiple(() => {
                Assert.That(expected.Any(x => x.Id == item.Id), Is.True);
                Assert.That(expected.Any(x => x.UserId == item.UserId), Is.True);
                Assert.That(expected.Any(x => x.EntityId == item.EntityId), Is.True);
                Assert.That(expected.Any(x => x.EntityType == item.EntityType), Is.True);
                Assert.That(expected.Any(x => x.OperationType == item.OperationType), Is.True);
                Assert.That(expected.Any(x => x.Activity == item.Activity), Is.True);
                Assert.That(expected.Any(x => x.Details == item.Details), Is.True);
                Assert.That(expected.Any(x => x.Status == item.Status), Is.True);
                Assert.That(expected.Any(x => x.IpAddress == item.IpAddress), Is.True);
                Assert.That(expected.Any(x => x.UserAgent == item.UserAgent), Is.True);
            });
        }
    }
}