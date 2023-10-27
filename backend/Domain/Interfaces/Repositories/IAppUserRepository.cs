using Domain.Entities;
namespace Domain.Interfaces.Repositories; 

public interface IAppUserRepository : IGenericRepository<Users> {
    Task<IEnumerable<Users>> GetRecommandPicks();
    Task<IEnumerable<Users>> GetSessionHistory();
}