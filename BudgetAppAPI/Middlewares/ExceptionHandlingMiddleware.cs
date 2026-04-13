using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;
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
            string title = "Error";
            string detail = "Ocurrió un error inesperado";
            var statusCode = HttpStatusCode.InternalServerError;
            var errorMessage = exception.Message;
            object? errors = null;

            switch (exception)
            {
                case UnauthorizedAccessException:
                    statusCode = HttpStatusCode.Unauthorized;
                    title = "Acceso no autorizado";
                    detail = exception.Message;
                    break;
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Formato incorrecto";
                    detail = "El(los) valor(es) ingresado(s) tiene(n) el formato incorrecto";
                    errors = validationException.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g =>
                            {
                                StringBuilder keyBuilder = new StringBuilder();

                                keyBuilder.Append(char.ToLower(g.Key[0]));

                                if (g.Key.Length > 1)
                                {
                                    keyBuilder.Append(g.Key[1..]);
                                }

                                return keyBuilder.ToString();
                            },
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );
                    break;
                case BadHttpRequestException:
                case ArgumentNullException:
                case ArgumentException:
                case ApplicationException:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Solicitud invalida";
                    detail = exception.Message;
                    break;
                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    detail = exception.Message;
                    break;
                case DbUpdateConcurrencyException:
                case ConflictException:
                    statusCode = HttpStatusCode.Conflict;
                    title = "Conflicto de datos";
                    detail = exception.Message;
                    break;
                default:
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new
            {
                Title = title,
                Status = (int)statusCode,
                Detail = detail,
                Errors = errors
            };

            _logger.LogError(exception, exception.Message);
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
