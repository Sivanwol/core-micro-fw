namespace Application.Elasticsearch;

public abstract class ElasticBaseIndex {
    public string Id { get; set; }
    public DateTime? UpdateTime { get; set; }
}