using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace pathly_backend.Shared.ErrorHandling;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            context.Response.ContentType = "application/json";

            var (status, title) = ex switch
            {
                ArgumentException => (StatusCodes.Status400BadRequest, ex.Message),
                InvalidOperationException => (StatusCodes.Status400BadRequest, ex.Message),
                KeyNotFoundException      => (StatusCodes.Status404NotFound,  ex.Message),
                _ => (StatusCodes.Status500InternalServerError, "Unexpected error")
            };

            context.Response.StatusCode = status;

            var problem = new ProblemDetails
            {
                Status = status,
                Title = title
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}