using Domain.Entities;
namespace Domain.Persistence.Interfaces.Repositories;

public interface IMediaRepository : IGenericRepository<Media> {
    Task<IEnumerable<Media>> GetUserMedia(int userId);
    Task DeleteAllUserMedia(int userId);
    Task DeleteUserSelectedMedia(int userId, IEnumerable<int> mediaIds);
}