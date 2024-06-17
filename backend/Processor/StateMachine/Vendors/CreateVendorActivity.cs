using MassTransit;
using Processor.Contracts.Vendors;

namespace Processor.StateMachine.Vendors;

public class CreateVendorActivity : IExecuteActivity<VendorCreatedArguments>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<VendorCreatedArguments> context)
    {
        return context.Completed();
    }
}
