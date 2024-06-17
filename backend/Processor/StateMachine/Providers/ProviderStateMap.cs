using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Processor.StateMachine.Providers;

public class ProviderStateMap: SagaClassMap<ProviderState>
{
     protected override void Configure(EntityTypeBuilder<ProviderState> entity, ModelBuilder model)
    {
        entity.Property(x => x.CurrentState);
        entity.Property(x => x.ClientId);
        entity.Property(x => x.ProviderId);
        entity.Property(x => x.CorrelationId);
    }
}
