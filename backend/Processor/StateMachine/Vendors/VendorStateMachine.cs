using MassTransit;
using Processor.Contracts.Vendors;

namespace Processor.StateMachine.Vendors;

public class VendorStateMachine : MassTransitStateMachine<VendorState>
{

    public VendorStateMachine()
    {
        Event(() => EventCreateStageInitial, x => x.CorrelateById(c => Guid.Parse($"{c.Message.VendorId}-{c.Message.ClientId ?? 0}-{c.Message.UserId}")));
        Event(() => EventCreateStageProcess, x => x.CorrelateById(c => Guid.Parse($"{c.Message.VendorId}-{c.Message.ClientId ?? 0}-{c.Message.UserId}")));
        Event(() => EventCreateStageFinished, x => x.CorrelateById(c => Guid.Parse($"{c.Message.VendorId}-{c.Message.ClientId ?? 0}-{c.Message.UserId}")));
        Initially(
            When(EventCreateStageProcess)
                .PublishAsync(x => x.Init<VendorCreatedFinished>(x.Message))
                .TransitionTo(CreateStageProcessDone)
        );

        During(CreateStageInitial,
            When(EventCreateStageInitial)
                .PublishAsync(x => x.Init<VendorCreatedProcess>(x.Message))
                .TransitionTo(CreateStageToProcess)
        );
        During(CreateStageToProcess,
            When(EventCreateStageProcess)
                .PublishAsync(x => x.Init<VendorCreatedFinished>(x.Message))
                .TransitionTo(CreateStageProcessDone)
        );

        During(CreateStageProcessDone,
            Ignore(EventCreateStageFinished));
    }
    public Event<VendorCreatedFinished> EventCreateStageFinished { get; set; }
    public Event<VendorCreatedProcess> EventCreateStageProcess { get; set; }
    public Event<VendorCreatedInitial> EventCreateStageInitial { get; set; }

    public State CreateStageInitial { get; set; }
    public State CreateStageToProcess { get; set; }
    public State CreateStageProcessDone { get; set; }
}
