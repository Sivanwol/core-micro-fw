namespace Application.Exceptions;

public class AuthorizationException : Exception {
    public AuthorizationException() : base("You are not authorized to access this resource") { }
}