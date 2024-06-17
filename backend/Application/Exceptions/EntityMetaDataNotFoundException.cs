using Application.Enums;

namespace Application.Exceptions;

public class EntityMetaDataNotFoundException : BaseException {
    public EntityMetaDataNotFoundException(string entityName, int gameId, string metaDataKey) : base(entityName,
        StatusCodeErrors.NotFound,
        $"Entity {entityName} on entity id {gameId} with meta data key {metaDataKey} been found") { }
}