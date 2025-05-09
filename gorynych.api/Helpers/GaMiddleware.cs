using gorynych.common;
using Microsoft.AspNetCore.Mvc;

namespace gorynych.api.Helpers;

public sealed class GaMiddleware(RequestDelegate next, ILogger<GaMiddleware> logger)
{
    private const string XRequestId = "X-Request-ID";

    public async Task InvokeAsync(HttpContext context)
    {
        var requestId = string.Empty;
        try
        {
            requestId = context.Request.Headers[XRequestId].FirstOrDefault()
                        ?? Guid.NewGuid().ToString();
            using var scope = logger.BeginScope(
                new LogDictionary<string, object> { [XRequestId] = requestId }
            );
            context.Request.Headers[XRequestId] = requestId;
            await next(context);
        }
        catch (Exception e)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server Error",
                Detail = $"Request {requestId} failed"
            };
            logger.LogError(e, problemDetails.Detail);
            
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}