using Application.Enums;

namespace Application.Exceptions;

public class EntityMetaDataFoundException : BaseException {
    public EntityMetaDataFoundException(string entityName, int gameId, string metaDataKey) : base(entityName, StatusCodeErrors.Found,
        $"Entity {entityName} on entity id {gameId} with meta data key {metaDataKey} been found") { }
}