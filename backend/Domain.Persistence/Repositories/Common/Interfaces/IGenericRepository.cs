namespace Domain.Persistence.Repositories.Common.Interfaces;

public interface IGenericRepository<T, F> where T : class {
    Task<T?> GetById(F id);
}