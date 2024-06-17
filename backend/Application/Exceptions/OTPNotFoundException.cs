namespace Application.Exceptions;

public class OTPNotFoundException : Exception {
    public OTPNotFoundException(string userToken) : base($"Otp Code Not found for user {userToken}") { }
}