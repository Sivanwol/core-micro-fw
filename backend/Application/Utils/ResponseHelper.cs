using System.Net;
using Application.Responses.Base;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
namespace Application.Utils;

public static class ResponseHelper {
    public static IActionResult CreateResponse<TObject>(TObject data, HttpStatusCode statusCode = HttpStatusCode.OK) where TObject : class {
        return CreateResponse(data, new List<string>(), statusCode);
    }

    public static IActionResult CreateResponse<TObject>(TObject data, List<string> errors, HttpStatusCode statusCode = HttpStatusCode.OK) where TObject : class {
        var response = new DataResponse<TObject>(data);
        response.StatusCode = (int)statusCode;
        if (statusCode != HttpStatusCode.OK) {
            response.Status = false;
        }
        response.Errors = errors;
        return new ObjectResult(response) {
            StatusCode = (int)statusCode
        };
    }

    public static DataResponse<TObject> CreateResponseGQL<TObject>(TObject data, List<string> errors, HttpStatusCode statusCode = HttpStatusCode.OK) where TObject : class {
        var response = new DataResponse<TObject>(data);
        response.StatusCode = (int)statusCode;
        if (statusCode != HttpStatusCode.OK) {
            response.Status = false;
        }
        response.Errors = errors;
        return response;
    }


    public static IActionResult CreateEmptyResponse(HttpStatusCode statusCode = HttpStatusCode.OK) {
        return CreateResponse(new EmptyResponse(), new List<string>(), statusCode);
    }

    public static IActionResult CreateEmptyResponse(List<string> errors, HttpStatusCode statusCode = HttpStatusCode.OK) {
        return CreateResponse(new EmptyResponse(), errors, statusCode);
    }
    public static List<string> HandlerErrorResponse(ValidationResult result) {
        return result.Errors.Select(error => error.ErrorMessage).ToList();
    }
}