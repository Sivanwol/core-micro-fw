using Domain.Entities;
namespace Domain.Persistence.Interfaces.Repositories;

public interface IReligionsRepository : IGenericRepository<Religions> {
    Task<IEnumerable<Religions>> GetAll();
}