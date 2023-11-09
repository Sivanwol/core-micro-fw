namespace Domain.Persistence.Interfaces;

public interface IGenericRepository<T> where T : class {
    Task<T> GetById(int id);
}