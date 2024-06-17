using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Events;

namespace Processor.StateMachine.Providers;

public class ProviderState: EventStage
{
    public Guid UserId { get; set; }
    public int? ClientId { get; set; }
    public int ProviderId { get; set; }
}
