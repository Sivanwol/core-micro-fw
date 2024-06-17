using MassTransit;
using Processor.Consumers.Common;
using Processor.Contracts.Vendors;
using Processor.StateMachine.Vendors;

namespace Processor.Consumers.Vendors;

public class CreateVendorDoneConsumer : BaseConsumer<VendorCreatedFinished, VendorState>
{

    public CreateVendorDoneConsumer(ISendEndpointProvider sendEndpointProvider, IEndpointNameFormatter endpointNameFormatter) : base(sendEndpointProvider, endpointNameFormatter)
    {
    }
    public override async Task Consume(ConsumeContext<VendorCreatedFinished> context)
    {
        LogContext.Info?.Log($"CreateVendorDoneConsumer: {context.ConversationId} - {context.Message.VendorId} - {context.Message.ClientId ?? 0} - {context.Message.UserId}");
    }
}
