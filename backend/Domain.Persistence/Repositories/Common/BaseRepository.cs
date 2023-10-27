using Domain.Persistence.Context;
namespace Domain.Persistence.Repositories.Common; 

public abstract class BaseRepository {
    protected readonly IDomainContext Context;
    public BaseRepository(
        IDomainContext context) {
        Context = context;
    }
    
}