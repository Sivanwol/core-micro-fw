namespace Application.Exceptions;

public class EntityValidationException : BaseException {
    public EntityValidationException(string entityName) : base(entityName, StatusCodeErrors.BadRequest,
        $"Entity {entityName} has validation error") { }
}