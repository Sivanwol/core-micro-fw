namespace Application.Exceptions;

public class EntityNotFoundException : BaseException {
    public EntityNotFoundException(string entityName, int entityId) : base(entityName, StatusCodeErrors.Found,
        $"Entity {entityName} with id {entityId} not been found") { }

    public EntityNotFoundException(string entityName, string entityId) : base(entityName, StatusCodeErrors.Found,
        $"Entity {entityName} with id {entityId} not been found") { }
}