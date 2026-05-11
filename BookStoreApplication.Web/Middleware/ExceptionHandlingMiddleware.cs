using System.Net;
using System.Text.Json;
using BookStoreApplication.Web.Wrapper;
using BookStoreApplication.Web.Exceptions;
using AutoMapper;
using FluentValidation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApplication.Web.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (context.Response.HasStarted)
        {
            _logger.LogError(exception, "An exception occurred after the response had already started");
            throw exception;
        }

        var statusCode = exception switch
        {
            NotFoundException => HttpStatusCode.NotFound,
            ConflictException => HttpStatusCode.Conflict,
            BadRequestException => HttpStatusCode.BadRequest,
            ValidationException => HttpStatusCode.BadRequest,
            BadHttpRequestException => HttpStatusCode.BadRequest,
            JsonException => HttpStatusCode.BadRequest,
            AutoMapperMappingException => HttpStatusCode.BadRequest,
            DbUpdateException => HttpStatusCode.BadRequest,
            SqlException => HttpStatusCode.ServiceUnavailable,
            _ => HttpStatusCode.InternalServerError
        };

        var message = exception switch
        {
            ValidationException => "Validation failed.",
            BadHttpRequestException => "Invalid request body or route value.",
            JsonException => "Invalid JSON request body.",
            AutoMapperMappingException => "Submitted data could not be mapped. Check the request values.",
            DbUpdateException => "Database update failed. Check related records and submitted values.",
            SqlException => "Database connection failed or SQL Server is unavailable.",
            _ when statusCode == HttpStatusCode.InternalServerError => "An unexpected server error occurred.",
            _ => exception.Message
        };

        var errors = exception is ValidationException validationException
            ? validationException.Errors.Select(e => e.ErrorMessage).ToList()
            : null;

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled API exception");
        }

        var response = ApiResponse<object>.Failure((int)statusCode, message, errors);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
