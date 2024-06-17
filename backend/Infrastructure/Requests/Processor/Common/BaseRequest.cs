using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using MediatR;

namespace Infrastructure.Requests.Processor.Common;

public class BaseRequest<T> :  IRequest<T>
{
    public Guid LoggedInUserId { get; set; }
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
}
