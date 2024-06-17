using Application.Enums;

namespace Application.Exceptions;

public class InvalidRequestException : BaseException {

    public InvalidRequestException(string message) : base("Processors", StatusCodeErrors.InternalServerError, message) { }
}