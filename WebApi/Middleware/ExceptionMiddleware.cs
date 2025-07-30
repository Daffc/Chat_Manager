
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
        catch (FluentValidation.ValidationException ex)
        {
            _logger.LogWarning("Validation failed: {Errors}", ex.Errors);
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Bad Request: {Message}", ex.Message);
            await HandleGenericAsValidationAsync(context, StatusCodes.Status400BadRequest, "Bad Request", ex.Message);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning("NotFound: {Message}", ex.Message);
            await HandleGenericAsValidationAsync(context, StatusCodes.Status404NotFound, "Not Found", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleGenericAsValidationAsync(context, StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occurred. Please try again later.");
        }
    }

    private async Task HandleValidationExceptionAsync(HttpContext context, FluentValidation.ValidationException ex)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        var validationErrors = ex.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        var problemDetails = new ValidationProblemDetails(validationErrors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Validation Error",
            Status = StatusCodes.Status400BadRequest,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private async Task HandleGenericAsValidationAsync(HttpContext context, int statusCode, string title, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var errors = new Dictionary<string, string[]>
        {
            { "general", new[] { message } }
        };

        var problemDetails = new ValidationProblemDetails(errors)
        {
            Type = statusCode switch
            {
                StatusCodes.Status400BadRequest => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                StatusCodes.Status404NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                StatusCodes.Status500InternalServerError => "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                _ => "about:blank"
            },
            Title = title,
            Status = statusCode,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}
