using Application.Exceptions;
using Domain.Entities;
using Domain.Persistence.Context;
using Domain.Persistence.Interfaces.Mock;
using Domain.Persistence.Interfaces.Repositories;
using Domain.Persistence.Repositories.Common;
using Microsoft.Extensions.Logging;
namespace Domain.Persistence.Repositories;

public class MediaRepository : BaseRepository, IMediaRepository {
    private readonly IAppUserRepository _appUserRepository;

    private readonly ILogger _logger;
    private readonly IMediaMockService _mock;
    public MediaRepository(
        IDomainContext context,
        ILoggerFactory loggerFactory,
        IAppUserRepository appUserRepository,
        IMediaMockService mock
    ) : base(context) {
        _logger = loggerFactory.CreateLogger<MediaRepository>();
        _mock = mock;
        _appUserRepository = appUserRepository;
    }
    public Task<Media> GetById(int id) {
        _logger.LogInformation($"Fetching media with id {id}");
        return Task.FromResult(_mock.GetOne(id));
    }
    public async Task<IEnumerable<Media>> GetUserMedia(int userId) {
        _logger.LogInformation($"Fetching all media for user with id {userId}");
        var user = await _appUserRepository.GetById(userId);
        if (user == null) {
            throw new EntityNotFoundException("User", userId.ToString());
        }
        return await Task.FromResult(_mock.GetUserMedia(userId));
    }
    public async Task DeleteAllUserMedia(int userId) {
        _logger.LogInformation($"Deleting all media for user with id {userId}");
        var user = await _appUserRepository.GetById(userId);
        if (user == null) {
            throw new EntityNotFoundException("User", userId.ToString());
        }
    }
    public async Task DeleteUserSelectedMedia(int userId, IEnumerable<int> mediaIds) {
        _logger.LogInformation($"Deleting selected media for user with id {userId}");
        var user = await _appUserRepository.GetById(userId);
        if (user == null) {
            throw new EntityNotFoundException("User", userId.ToString());
        }
    }
}