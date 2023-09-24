using Application.Elasticsearch;
using MassTransit;

namespace Processor.Consumers.IndexUser; 

public class IndexUserConsumerHandler: IConsumer<IndexUserEvent> {
    private readonly IBaseElasticRepository<IndexUserRequest> _indexUser;

    public IndexUserConsumerHandler(IBaseElasticRepository<IndexUserRequest> entity)
    {
        _indexUser = entity;
    }

    public async Task Consume(ConsumeContext<IndexUserEvent> context) {
        await _indexUser.CreateIndexAsync();
        var user = context.Message.User;
        await _indexUser.InsertAsync(new IndexUserRequest {
            Auth0Id = user.Auth0Id
        });
    }
}

public class IndexUserConsumerDefinition : ConsumerDefinition<IndexUserConsumerHandler> {
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<IndexUserConsumerHandler> consumerConfigurator)
    {
        //consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(3)));
    }
}