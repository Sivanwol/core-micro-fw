using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Processor.Contracts.Vendors;

public interface VendorCreatedArguments
{
    public int ProviderId { get; set; }
    public int? ClientId { get; set; }
    public Guid UserId { get; set; }
}
