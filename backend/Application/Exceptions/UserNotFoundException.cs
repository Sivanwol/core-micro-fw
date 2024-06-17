namespace Application.Exceptions;

public class UserNotFoundException : Exception {
    public UserNotFoundException(string user) : base($"the user {user} not found") { }
}