namespace Application.Exceptions;

public class InvalidLoginException : Exception {
    public InvalidLoginException() : base("invalid login credentials") { }
}