using Application.Events.Data;
using MassTransit;
namespace Application.Events.Flow;

public class GeneralEventStateMachineFlow : MassTransitStateMachine<EventStage> {

    public GeneralEventStateMachineFlow() {
        InstanceState(x => x.CurrentState);
        Event(() => OnStart,
            x => x.CorrelateById(m => m.Message.EventId)
                .SelectId(m => m.Message.EventId));
        Event(() => OnSubmit,
            x => x.CorrelateById(m => m.Message.EventId)
                .SelectId(m => m.Message.EventId));
        Event(() => OnSave,
            x => x.CorrelateById(m => m.Message.EventId)
                .SelectId(m => m.Message.EventId));
        Initially(
            When(OnStart)
                .TransitionTo(Initial),
            When(OnSubmit)
                .TransitionTo(Submitted),
            When(OnSave)
                .TransitionTo(Save)
        );
    }
    public State Initial { get; } = null!;
    public State Submitted { get; } = null!;
    public State Save { get; } = null!;
    public Event<GeneralData> OnStart { get; } = null!;
    public Event<GeneralData> OnSubmit { get; } = null!;
    public Event<GeneralData> OnSave { get; } = null!;
}