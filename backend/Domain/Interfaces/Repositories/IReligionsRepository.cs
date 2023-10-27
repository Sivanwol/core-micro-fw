using Domain.Entities;
namespace Domain.Interfaces.Repositories; 

public interface IReligionsRepository : IGenericRepository<Religions> {
    Task<IEnumerable<Religions>> GetAll();
}
