using Domain.Entities;

namespace Processor.Consumers.IndexUser; 

public class IndexUserEvent {
    public User User { get; set; }
}