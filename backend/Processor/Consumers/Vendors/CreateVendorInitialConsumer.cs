using MassTransit;
using Processor.Consumers.Common;
using Processor.Contracts.Vendors;
using Processor.StateMachine.Vendors;

namespace Processor.Consumers.Vendors;

public class CreateVendorInitialConsumer : BaseConsumer<VendorCreatedInitial, VendorState>
{

    public CreateVendorInitialConsumer(ISendEndpointProvider sendEndpointProvider, IEndpointNameFormatter endpointNameFormatter) : base(sendEndpointProvider, endpointNameFormatter)
    {
    }
    public override async Task Consume(ConsumeContext<VendorCreatedInitial> context)
    {
        LogContext.Info?.Log($"CreateVendorInitialConsumer: {context.ConversationId} - {context.Message.VendorId} - {context.Message.ClientId ?? 0} - {context.Message.UserId}");
        // await Publish<VendorCreatedInitial, CreateVendorActivity, VendorCreatedArguments>(context);
    }
}
