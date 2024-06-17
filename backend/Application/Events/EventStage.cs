using MassTransit;
namespace Application.Events;

public class EventStage : SagaStateMachineInstance {

    /// <summary>
    ///     the current saga state
    /// </summary>
    public string CurrentState { get; set; }

    /// <inheritdoc />
    public Guid CorrelationId { get; set; }
}