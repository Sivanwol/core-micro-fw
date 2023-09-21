using Application.Elasticsearch;

namespace Processor.Consumers.IndexUser; 

public class IndexUserRequest: ElasticBaseIndex {
    public string Auth0Id { get; set; }
}