using Application.Enums;

namespace Application.Exceptions;

public class UserNotCompletedRegistrationException : BaseException {

    public UserNotCompletedRegistrationException(string userId) : base(
        "ApplicationUser",
        StatusCodeErrors.NotFound,
        $"User {userId} is not complete the registration") { }
}