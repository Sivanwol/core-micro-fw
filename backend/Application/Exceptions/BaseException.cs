using Application.Enums;

namespace Application.Exceptions;

public class BaseException : Exception {

    public BaseException(string entityName, StatusCodeErrors statusCode, string message) : base(
        $"ERR-{statusCode}: {message}") {
        EntityName = entityName;
        StatusCode = statusCode;
    }
    public string EntityName { get; private set; }
    public StatusCodeErrors StatusCode { get; private set; }
}