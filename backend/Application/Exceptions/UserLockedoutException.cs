namespace Application.Exceptions;

public class UserLockedoutException : Exception {
    public UserLockedoutException() : base("the user that you been try login with been lockout try later") { }
}