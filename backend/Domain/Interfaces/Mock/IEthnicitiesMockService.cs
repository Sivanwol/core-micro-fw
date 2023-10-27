using Domain.Entities;
namespace Domain.Interfaces.Mock; 

public interface IEthnicitiesMockService {
    IEnumerable<Ethnicities> GetAll();
    Ethnicities GetOne();
}