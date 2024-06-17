using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Processor.Contracts.Providers;

namespace Processor.StateMachine.Providers;

public class CreateProviderActivity : IExecuteActivity<ProviderCreatedArguments>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<ProviderCreatedArguments> context)
    {
        return context.Completed();
    }
}
