using Domain.Entities;
namespace Domain.Persistence.Interfaces.Mock;

public interface IEthnicitiesMockService {
    IEnumerable<Ethnicities> GetAll();
    Ethnicities GetOne();
}