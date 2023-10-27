using Domain.Entities;
namespace Domain.Interfaces.Repositories; 

public interface IEthnicitiesRepository : IGenericRepository<Ethnicities> {
    Task<IEnumerable<Ethnicities>> GetAll();
}
