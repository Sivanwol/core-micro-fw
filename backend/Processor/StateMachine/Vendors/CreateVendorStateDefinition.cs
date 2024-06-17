using MassTransit;
using Processor.StateMachine.Common;

namespace Processor.StateMachine.Vendors;

public class CreateVendorStateDefinition : SagaDefinition<VendorState>
{

    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator,
        ISagaConfigurator<VendorState> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(10, 50, 100, 1000, 1000, 1000, 1000, 1000));
        endpointConfigurator.UseEntityFrameworkOutbox<SageDbContext>(context);
    }
}
