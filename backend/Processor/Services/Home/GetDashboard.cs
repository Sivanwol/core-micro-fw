using AutoBogus;
using Domain.Persistence.Repositories.Interfaces;
using Infrastructure.GQL;
using Infrastructure.GQL.Common.Widgets;
using Infrastructure.Requests.Processor.Services.Home;
using Infrastructure.Services.Cache;
using MediatR;
namespace Processor.Services.Home;

public class GetDashboard : IRequestHandler<GetDashboardRequest, Dashboard> {
    private readonly ICacheService _cacheService;
    private readonly IConfigurableUserViewRepository _configurableUserViewRepository;

    public GetDashboard(IConfigurableUserViewRepository configurableUserViewRepository, ICacheService cacheService) {
        _configurableUserViewRepository = configurableUserViewRepository;
        _cacheService = cacheService;
    }
    public async Task<Dashboard> Handle(GetDashboardRequest request, CancellationToken cancellationToken) {
        var faker = AutoFaker.Create();
        var dashboard = new Dashboard {
            MissionStatus = new MissionStatusWidget(),
            Missions = new List<MissionsWidget>(),
            Profits = new List<ProfitWidget>(),
            Users = new List<UsersWidget>(),
            TrafficSource = new List<TrafficSourceWidget>()
        };
        var missions = new List<MissionsWidget>();
        for (var i = 0; i < 20; i++) {
            var entity = faker.Generate<MissionsWidget>();
            entity.Profit = GetRandomInt();
            entity.Losses = GetRandomInt(15000);
            entity.Status = GetRandomStatus();
            missions.Add(entity);
        }
        dashboard.Missions = missions;
        
        dashboard.MissionStatus = new MissionStatusWidget {
            TotalBacklog = GetRandomInt(100),
            TotalDone = GetRandomInt(10),
            TotalPending = GetRandomInt(30),
            TotalInProcess = GetRandomInt(30),
            AverageTaskComplete = GetRandomInt(50),
            AvgProfitPercentage = GetRandomInt(30),
            UserAvgProfit = getRandomDecimal(300),
            UserCPC = getRandomDecimal(8),
            UserInstalls = GetRandomInt(75)
        };

        var profits = new List<ProfitWidget>();
        for (var i = 0; i < 10; i++) {
            var entity = faker.Generate<ProfitWidget>();
            entity.Profit = GetRandomInt();
            profits.Add(entity);
        }
        dashboard.Profits = profits;

        var users = new List<UsersWidget>();
        for (var i = 0; i < 5; i++) {
            var entity = faker.Generate<UsersWidget>();
            entity.Cpc = getRandomDecimal(8);
            entity.TotalInstall = GetRandomInt(50);
            users.Add(entity);
        }
        dashboard.Users = users;

        var trafficSources = new List<TrafficSourceWidget>();
        for (var i = 0; i < 3; i++) {
            var entity = faker.Generate<TrafficSourceWidget>();
            entity.Total = GetRandomInt();
            trafficSources.Add(entity);
        }
        dashboard.TrafficSource = trafficSources;
        return dashboard;
    }
    private string GetRandomStatus() {
        string[] status = {"Backlog", "Panding" , "In Progesss", "Done"};
        return status[new Random().Next(status.Length)];
    }
    private int GetRandomInt(int max = 20000) {
        return new Random().Next(1, max);
    }
    private decimal getRandomDecimal(int max = 100) {
        return Math.Abs(Convert.ToDecimal(Math.Round(new Random().Next(1, max) * ((new Random().Next() / 1073741824.0f)),2, MidpointRounding.ToEven)));
    }
}