namespace Domain.Interfaces; 

public interface IGenericExtendedRepository<T> where T : class, IGenericRepository<T>
{
    Task<IEnumerable<T>> GetAll();
    Task<IEnumerable<T>> GetPagedReponse(int pageNumber, int pageSize);
    Task<IEnumerable<T>> GetPagedAdvancedReponse(int pageNumber, int pageSize, string orderBy, string fields);
    Task<T> AddAsync(T entity);
    Task BulkInsertAsync(IList<T> entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}