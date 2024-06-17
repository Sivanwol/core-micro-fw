using Application.Events;

namespace Processor.StateMachine.Vendors;

public class VendorState : EventStage
{
    public Guid UserId { get; set; }
    public int? ClientId { get; set; }
    public int VendorId { get; set; }
}
