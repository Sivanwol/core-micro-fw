using Nest;
namespace Application.Elasticsearch;

public class ElasticRepository<T> : ElasticBaseRepository<T>, IBaseElasticRepository<T> where T : ElasticBaseIndex {

    public ElasticRepository(IElasticClient elasticClient)
        : base(elasticClient) {
        IndexName = typeof(T).Name.ToLower();
    }
    public override string IndexName { get; }
}