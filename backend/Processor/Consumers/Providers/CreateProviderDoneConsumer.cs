using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier.Contracts;
using Processor.Consumers.Common;
using Processor.Contracts.Providers;
using Processor.StateMachine.Providers;

namespace Processor.Consumers.Providers;

public class CreateProviderDoneConsumer : BaseConsumer<ProviderCreatedFinished, ProviderState>
{

    public CreateProviderDoneConsumer(ISendEndpointProvider sendEndpointProvider, IEndpointNameFormatter endpointNameFormatter): base(sendEndpointProvider, endpointNameFormatter)
    {
    }
    public override async Task Consume(ConsumeContext<ProviderCreatedFinished> context)
    {
        LogContext.Info?.Log($"CreateProviderDoneConsumer: {context.ConversationId} - {context.Message.ProviderId} - {context.Message.ClientId ?? 0} - {context.Message.UserId}");
    }
}
