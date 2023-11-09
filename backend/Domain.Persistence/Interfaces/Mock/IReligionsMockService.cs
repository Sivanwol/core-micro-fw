using Domain.Entities;
namespace Domain.Persistence.Interfaces.Mock;

public interface IReligionsMockService {
    IEnumerable<Religions> GetAll();
    Religions GetOne();
}