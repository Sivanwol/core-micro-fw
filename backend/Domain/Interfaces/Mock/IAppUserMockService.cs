using Domain.Entities;
namespace Domain.Interfaces.Mock; 

public interface IAppUserMockService {
    IEnumerable<Users> GetPicks();
    IEnumerable<Users> GetSessionHistory();
    Users GetOne();
}