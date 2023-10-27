using Domain.Entities;
namespace Domain.Interfaces.Mock; 

public interface IReligionsMockService {
    IEnumerable<Religions> GetAll();
    Religions GetOne();
}