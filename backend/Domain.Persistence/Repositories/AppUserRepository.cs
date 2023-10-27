using Domain.Entities;
using Domain.Interfaces.Mock;
using Domain.Interfaces.Repositories;
using Domain.Persistence.Context;
using Domain.Persistence.Repositories.Common;
using Serilog;

namespace Domain.Persistence.Repositories;

public class AppUserRepository : BaseRepository, IAppUserRepository {
    private readonly IAppUserMockService _mock;
    public AppUserRepository(
        IDomainContext context,
        IAppUserMockService mock
    ) : base(context) {
        _mock = mock;
    }

    public Task<Users> GetById(int id) {
        Log.Logger.Information($"Fetching user with id {id}");
        return Task.FromResult(_mock.GetOne());
    }
    public Task<IEnumerable<Users>> GetRecommandPicks() {
        Log.Logger.Information("Fetching all recommends picks");
        return Task.FromResult(_mock.GetPicks());
    }
    public Task<IEnumerable<Users>> GetSessionHistory() {
        Log.Logger.Information("Fetching all session history");
        return Task.FromResult(_mock.GetSessionHistory());
    }
}