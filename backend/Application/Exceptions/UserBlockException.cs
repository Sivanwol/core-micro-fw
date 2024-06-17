namespace Application.Exceptions;

public class UserBlockException : Exception {
    public UserBlockException() : base("the user that you been try login with been disabled") { }
}