namespace Application.Elasticsearch; 

public interface IBaseElasticRepository<T> : IElasticBaseRepository<T> where T : class
{
}