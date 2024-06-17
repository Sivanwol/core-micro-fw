using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Processor.Contracts.Providers;

public class ProviderCreatedProcess
{
    public int ProviderId { get; set; }
    public int? ClientId { get; set; }
    public Guid UserId { get; set; }
}
