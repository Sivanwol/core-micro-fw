using Application.Enums;

namespace Application.Exceptions;

public class AuthenticationException : BaseException {

    public AuthenticationException(string message) : base("user", StatusCodeErrors.Unauthorized, message) { }
}