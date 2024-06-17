using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Processor.Contracts.Providers;

namespace Processor.StateMachine.Providers;

public class ProvideStateMachine : MassTransitStateMachine<ProviderState>
{

    public ProvideStateMachine()
    {
        Event(() => EventCreateStageInitial, x => x.CorrelateById(c => Guid.Parse($"{c.Message.ProviderId}-{c.Message.ClientId ?? 0}-{c.Message.UserId}")));
        Event(() => EventCreateStageProcess, x => x.CorrelateById(c => Guid.Parse($"{c.Message.ProviderId}-{c.Message.ClientId ?? 0}-{c.Message.UserId}")));
        Event(() => EventCreateStageFinished, x => x.CorrelateById(c => Guid.Parse($"{c.Message.ProviderId}-{c.Message.ClientId ?? 0}-{c.Message.UserId}")));
        Initially(
            When(EventCreateStageProcess)
                .PublishAsync(x => x.Init<ProviderCreatedFinished>(x.Message))
                .TransitionTo(CreateStageProcessDone)
        );

        During(CreateStageInitial,
            When(EventCreateStageInitial)
                .PublishAsync(x => x.Init<ProviderCreatedProcess>(x.Message))
                .TransitionTo(CreateStageToProcess)
        );
        During(CreateStageToProcess,
            When(EventCreateStageProcess)
                .PublishAsync(x => x.Init<ProviderCreatedFinished>(x.Message))
                .TransitionTo(CreateStageProcessDone)
        );

        During(CreateStageProcessDone,
            Ignore(EventCreateStageFinished));
    }
    public Event<ProviderCreatedFinished> EventCreateStageFinished { get; set; }
    public Event<ProviderCreatedProcess> EventCreateStageProcess { get; set; }
    public Event<ProviderCreatedInitial> EventCreateStageInitial { get; set; }

    public State CreateStageInitial { get; set; }
    public State CreateStageToProcess { get; set; }
    public State CreateStageProcessDone { get; set; }
}
