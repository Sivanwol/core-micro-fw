using Domain.Entities;
namespace Domain.Persistence.Interfaces.Mock;

public interface IMediaMockService {
    IEnumerable<Media> GetUserMedia(int userId);
    Media GetOne(int userId);
}