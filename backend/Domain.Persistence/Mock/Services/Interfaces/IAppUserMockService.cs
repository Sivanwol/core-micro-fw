using Domain.Entities;
namespace Domain.Persistence.Mock.Services.Interfaces;

public interface IAppUserMockService {
    ApplicationUser GetOne(Guid id);
    ApplicationUser GetOneByToken(string userToken);
}