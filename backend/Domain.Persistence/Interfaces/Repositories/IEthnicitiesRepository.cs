using Domain.Entities;
namespace Domain.Persistence.Interfaces.Repositories;

public interface IEthnicitiesRepository : IGenericRepository<Ethnicities> {
    Task<IEnumerable<Ethnicities>> GetAll();
}