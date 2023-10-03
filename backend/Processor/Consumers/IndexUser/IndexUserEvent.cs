using Domain.Entities;

namespace Processor.Consumers.IndexUser;

public class IndexUserEvent {
    public ApplicationUser User { get; set; }
}