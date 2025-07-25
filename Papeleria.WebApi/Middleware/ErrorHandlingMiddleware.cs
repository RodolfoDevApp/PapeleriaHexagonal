using Papeleria.WebApi.DTOs;
using System.Net;
using System.Text.Json;

namespace Papeleria.WebApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Continua con el siguiente middleware
            }
            catch (Exception ex) (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = ResultDto<string>.FailureResult("Ocurrió un error inesperado. Detalles: " + ex.Message, 500);
            var json = JsonSerializer.Serialize(result);

            return response.WriteAsync(json);
        }
    }
}
