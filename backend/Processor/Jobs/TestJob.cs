using Application.Configs;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Serilog;
namespace Processor.Jobs;

public class TestJob : IJob {
    private readonly BackendApplicationConfig _config;
    private readonly IServiceScopeFactory _scopeFactory;
    public TestJob(BackendApplicationConfig config, IServiceScopeFactory scopeFactory) {
        _config = config;
        _scopeFactory = scopeFactory;
    }
    public Task Execute(IJobExecutionContext context) {
        Log.Logger.Information($"TestJob: {DateTime.Now}, {_config.AzureServiceBusUri}");
        // Code that sends a periodic email to the user (for example)
        // Note: This method must always return a value 
        // This is especially important for trigger listers watching job execution 
        return Task.FromResult(true);
    }
}