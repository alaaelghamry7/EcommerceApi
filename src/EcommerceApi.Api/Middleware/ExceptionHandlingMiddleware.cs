using System.Net;
using System.Text.Json;
using EcommerceApi.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var problemDetails = MapException(exception);

        LogException(exception, problemDetails.Status);

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, SerializerOptions));
    }

    private ProblemDetails MapException(Exception exception)
    {
        switch (exception)
        {
            case NotFoundException notFoundException:
                return new ProblemDetails
                {
                    Title = "Resource not found",
                    Status = (int)HttpStatusCode.NotFound,
                    Detail = notFoundException.Message
                };

            case ValidationException validationException:
                return new ValidationProblemDetails(new Dictionary<string, string[]>(validationException.Errors))
                {
                    Title = "Validation failed",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = validationException.Message
                };

            case UnauthorizedAccessException unauthorizedException:
                return new ProblemDetails
                {
                    Title = "Unauthorized",
                    Status = (int)HttpStatusCode.Unauthorized,
                    Detail = unauthorizedException.Message
                };

            default:
                return new ProblemDetails
                {
                    Title = "An unexpected error occurred",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Detail = _environment.IsDevelopment() ? exception.ToString() : "An internal server error occurred. Please try again later."
                };
        }
    }

    private void LogException(Exception exception, int? statusCode)
    {
        if (statusCode >= 500 || statusCode is null)
        {
            _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);
        }
        else
        {
            _logger.LogWarning(exception, "A handled exception occurred: {Message}", exception.Message);
        }
    }
}
