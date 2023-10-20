using Application.Configs;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NCrontab;
namespace Application.Utils.Service;

public abstract class BaseSchedulerSystem : BackgroundService {
    readonly IServiceScopeFactory _scopeFactory;
    private DateTime _nextRun;
    private CrontabSchedule _schedule;

    public BaseSchedulerSystem(IServiceScopeFactory scopeFactory, string schedule = "1 * * * *") {
        _scopeFactory = scopeFactory;
        Schedule = schedule;
        _schedule = CrontabSchedule.Parse(Schedule);
        _nextRun = _schedule.GetNextOccurrence(DateTime.Now);

    }

    /// <summary>
    /// this is the cron expression that defines when the job will run see https://crontab.guru/#1_*_*_*_* for more info
    /// </summary>
    public string Schedule { get; private set; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        do {
            var now = DateTime.Now;
            if (now > _nextRun) {
                await using var scope = _scopeFactory.CreateAsyncScope();
                var applicationConfig = scope.ServiceProvider.GetRequiredService<BackendApplicationConfig>();
                var messageScheduler = scope.ServiceProvider.GetRequiredService<IMessageScheduler>();
                await Process(scope, applicationConfig, messageScheduler, stoppingToken);
                _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
            }
            await Task.Delay(5000, stoppingToken); //5 seconds delay
        } while (!stoppingToken.IsCancellationRequested);
    }

    protected abstract Task Process(AsyncServiceScope scope, BackendApplicationConfig config, IMessageScheduler messageScheduler, CancellationToken stoppingToken);
}