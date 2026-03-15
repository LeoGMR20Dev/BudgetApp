using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TierlistBackend.Exceptions;

namespace BudgetAppAPI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger
            )
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
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var statusCode = HttpStatusCode.InternalServerError;
            var errorMessage = exception.Message;
            string title = "Ha ocurrido un error inesperado";

            switch (exception)
            {
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    title = "Acceso no autorizado";
                    break;
                case BadHttpRequestException:
                case ArgumentNullException:
                case ArgumentException:
                case ValidationException:
                case ApplicationException:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Solicitud invalida";
                    break;
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;
                case DbUpdateConcurrencyException:
                case ConflictException:
                    statusCode = HttpStatusCode.Conflict;
                    title = "Conflicto de datos";
                    break;
                default:
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new
            {
                Success = false,
                Message = errorMessage,
                StatusCode = (int)statusCode,
                ErrorDetails = exception.StackTrace,
                Detail = exception.Message
            };

            _logger.LogError(exception, errorResponse.Message);
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
