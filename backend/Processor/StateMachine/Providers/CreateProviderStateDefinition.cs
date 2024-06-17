using Domain.Persistence.Context;
using MassTransit;
using Processor.StateMachine.Common;

namespace Processor.StateMachine.Providers;

public class CreateProviderStateDefinition : SagaDefinition<ProviderState>
{

    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator,
        ISagaConfigurator<ProviderState> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(10, 50, 100, 1000, 1000, 1000, 1000, 1000));
        endpointConfigurator.UseEntityFrameworkOutbox<SageDbContext>(context);
    }
}
