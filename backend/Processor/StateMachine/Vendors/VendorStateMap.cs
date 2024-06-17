using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Processor.StateMachine.Vendors;

namespace Processor.StateMachine.Providers;

public class VendorStateMap: SagaClassMap<VendorState>
{
     protected override void Configure(EntityTypeBuilder<VendorState> entity, ModelBuilder model)
    {
        entity.Property(x => x.CurrentState);
        entity.Property(x => x.ClientId);
        entity.Property(x => x.VendorId);
        entity.Property(x => x.CorrelationId);
    }
}
