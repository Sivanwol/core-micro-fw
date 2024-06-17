using MassTransit;
using Processor.Consumers.Common;
using Processor.Contracts.Vendors;
using Processor.StateMachine.Vendors;

namespace Processor.Consumers.Vendors;

public class CreateVendorProcessConsumer : BaseConsumer<VendorCreatedProcess, VendorState>
{

    public CreateVendorProcessConsumer(ISendEndpointProvider sendEndpointProvider, 
    IEndpointNameFormatter endpointNameFormatter) : base(sendEndpointProvider, endpointNameFormatter)
    {
    }
    public override async Task Consume(ConsumeContext<VendorCreatedProcess> context)
    {
        LogContext.Info?.Log($"CreateVendorProcessConsumer: {context.ConversationId} - {context.Message.VendorId} - {context.Message.ClientId ?? 0} - {context.Message.UserId}");
        // await Publish<VendorCreatedInitial, CreateVendorActivity, VendorCreatedArguments>(context);
    }
}
