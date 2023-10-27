using System.Net;
using System.Text.Json;
using Application.Responses.Base;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
namespace Application.Middleware;

public class ErrorHandlingMiddleware {
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger) {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context) {
        try {
            await _next(context);
        }
        catch (Exception ex) {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception error) {
        var response = context.Response;
        response.ContentType = "application/json";
        var responseModel = new DataResponse<EmptyResponse> {
            Errors = new List<string>(),
            Status = false
        };

        switch (error) {
            case ValidationException:
                // custom application error
                responseModel.StatusCode = response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errors = new List<string>();
                foreach (var err in ((ValidationException)error).Errors) {
                    errors.Add(err.ErrorMessage);
                }
                responseModel.Errors = errors;
                break;

            case KeyNotFoundException:
                // not found error
                responseModel.StatusCode = response.StatusCode = (int)HttpStatusCode.BadRequest;
                responseModel.Errors.Add("Not Found");
                response.StatusCode = (int)HttpStatusCode.NotFound;
                break;

            default:
                // unhandled error
                responseModel.StatusCode = response.StatusCode = context.Response.StatusCode;
                responseModel.Errors.Add("Internal Server Error");
                if (context.Response.StatusCode != (int)HttpStatusCode.InternalServerError) {
                    responseModel.Errors = new List<string>();
                    responseModel.Errors.Add("Error Processing Request");
                }
                break;
        }
        // use ILogger to log the exception message
        _logger.LogError(error.Message, error.StackTrace);
        var result = JsonSerializer.Serialize(responseModel);
        await response.WriteAsync(result);
    }
}