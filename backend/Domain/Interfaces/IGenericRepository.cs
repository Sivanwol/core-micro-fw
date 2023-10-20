using Domain.Entities;
namespace Domain.Interfaces;

public interface IGenericRepository<T> where T : class {
    Task<Countries?> GetById(int id);
}