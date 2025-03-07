using System.Net;
using CustomTestCreator.Framework;
using CustomTestCreator.SharedKernel;

namespace CustomTestCreator.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next, 
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            var error = Error.Failure("server.internal", e.Message);
            var envelope = Envelope.Error(error);
            
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(envelope);
            
            _logger.LogError(e, e.Message);
        }
    }
}