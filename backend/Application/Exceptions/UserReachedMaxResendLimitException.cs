using Application.Enums;

namespace Application.Exceptions;

public class UserReachedMaxResendLimitException : BaseException {

    public UserReachedMaxResendLimitException(string userId) : base("ApplicationUser", StatusCodeErrors.Unauthorized, $"User {userId} been lock down too many attempts on Otp") { }
}