using Auth.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Auth.Infrastructure.Middleware;

public class GlobalExceptionHandlingMiddlware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        var probelemDetails = new ProblemDetails
        {
            Title = ex.Message,
            Type = ex.Source,
            
        };

        switch(ex)
        {
            case UserNotFoundException:
                probelemDetails.Status = StatusCodes.Status404NotFound;
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                break;
            case WrongInputException:
                probelemDetails.Status = StatusCodes.Status400BadRequest;
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                break;
            case UnauthorizedAccessException:
                probelemDetails.Status = StatusCodes.Status401Unauthorized;
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                break;
            default:
                probelemDetails.Status = StatusCodes.Status500InternalServerError;
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                break;
        }

        var json = JsonSerializer.Serialize(probelemDetails);
        return context.Response.WriteAsync(json);
    }
}
