namespace Application.Exceptions;

public enum StatusCodeErrors {
    // 400
    BadRequest = 400,

    // 401
    Unauthorized = 401,

    // 403
    Forbidden = 403,

    // 404
    NotFound = 404,

    // 302
    Found = 302,

    // 409
    Conflict = 409,

    // 500
    InternalServerError = 500
}

public class BaseException : Exception {
    public string EntityName { get; private set; }
    public StatusCodeErrors StatusCode { get; private set; }

    public BaseException(string entityName, StatusCodeErrors statusCode, string message) : base(
        $"ERR-{statusCode}: {message}") {
        EntityName = entityName;
        StatusCode = statusCode;
    }
}