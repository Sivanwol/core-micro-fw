namespace Application.Exceptions;

public class EntityFoundException : BaseException {
    public EntityFoundException(string entityName, int entityId) : base(entityName, StatusCodeErrors.Found,
        $"Entity {entityName} with id {entityId} been found") { }


    public EntityFoundException(string entityName, string entityId) : base(entityName, StatusCodeErrors.Found,
        $"Entity {entityName} with id {entityId} been found") { }
}