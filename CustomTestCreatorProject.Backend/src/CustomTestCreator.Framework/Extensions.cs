using CustomTestCreator.SharedKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomTestCreator.Framework;

public static class Extensions
{
    public static ActionResult ToResponse(this Error error)
    {
        var envelope = Envelope.Error(error);
        
        return new ObjectResult(envelope) { StatusCode = GetStatusCode(error) };
    }

    public static ActionResult ToResponse(this ErrorList errors)
    {
        if (errors.Any() == false)
            return new ObjectResult(null) { StatusCode = StatusCodes.Status500InternalServerError };
        
        Envelope envelope = Envelope.Error(errors);
        
        return new ObjectResult(envelope) { StatusCode = GetStatusCode(errors) };
    }

    private static int GetStatusCode(ErrorList errors)
    {
        var statusCode = errors.Count() > 1 
            ? StatusCodes.Status500InternalServerError
            : errors.First().ErrorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Required => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
        
        return statusCode;
    }
}